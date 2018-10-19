using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatientHelpCare.DataModel;

namespace PatientHelpCare.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Appointments
        public async Task<ActionResult> Index()
        {
            var User = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();

            if (User.UserType == "Doctor")
            {
                ViewBag.UserType = User.UserType;
                var appointments = _repository.Appointments.Include(a => a.Patient);
                return View(appointments.Where(x => x.DoctorId == User.Id).OrderByDescending(z => z.id).ToList());
            }
            else
            {
                var appointments = _repository.Appointments.Include(a => a.Doctor);
                return View(appointments.Where(x => x.PatientId == User.Id).OrderByDescending(z => z.id).ToList());
            }
            return View();
        }

        // GET: Appointments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await _repository.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.Patient_ID = new SelectList(_repository.Patients, "Id", "Firstname");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "id,PatientId,DoctorId,Appointment_date,Appointment_Time")] DateTime Date, String Time, String Email)
        {
            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment();
                var check = _repository.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault();
                if (check == null)
                {
                    ViewBag.Error = "This email is not registered yet! Please check email of your patient";
                    return View(appointment);
                }
                if (Date == null)
                {
                    ViewBag.Date = "Please select Date";
                    return View(appointment);
                }
                if (Time == null)
                {
                    ViewBag.Time = "Please select time";
                    return View(appointment);
                }
                var Doctor = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();

                appointment.DoctorId = Doctor.Id;
                appointment.PatientId = check.Id;
                appointment.Appointment_date = Date;
                appointment.Appointment_Time = Time;
                appointment.Status = "Awaiting Acceptence";
                _repository.Appointments.Add(appointment);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // ViewBag.Patient_ID = new SelectList(_repository.Patients, "Id", "Firstname", appointment.Patient);
            return View();
        }

        // GET: Appointments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _repository.Appointments.Where(w => w.id == id).FirstOrDefault();
            var PatientEmail = _repository.AspNetUsers.Where(x => x.Id == appointment.PatientId).FirstOrDefault();
            ViewBag.Email = PatientEmail.Email;
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_ID = new SelectList(_repository.Patients, "Id", "Firstname", appointment.Patient);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = "id,PatientId,DoctorId,Appointment_date,Appointment_Time")] int id, DateTime Date, String Time, String Email)
        {
            if (ModelState.IsValid)
            {
                Appointment lcl_Appointment = _repository.Appointments.Where(x => x.id == id).FirstOrDefault();
                var check = _repository.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault();
                if (check == null)
                {
                    ViewBag.Error = "This email is not registered yet! Please check email of your patient";
                    return View(lcl_Appointment);
                }
                if (Date == null)
                {
                    ViewBag.Date = "Please select Date";
                    return View(lcl_Appointment);
                }
                if (Time == null)
                {
                    ViewBag.Time = "Please select time";
                    return View(lcl_Appointment);
                }

                lcl_Appointment.PatientId = check.Id;
                lcl_Appointment.Appointment_date = Date;
                lcl_Appointment.Appointment_Time = Time;
                _repository.Entry(lcl_Appointment).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();

        }

        // GET: Appointments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await _repository.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Appointment appointment = await _repository.Appointments.FindAsync(id);
            _repository.Appointments.Remove(appointment);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _repository.Appointments.Where(w => w.id == id).FirstOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            appointment.Status = "Accepted";
            _repository.Entry(appointment).State = EntityState.Modified;
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Decline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _repository.Appointments.Where(w => w.id == id).FirstOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            appointment.Status = "Declined";
            _repository.Entry(appointment).State = EntityState.Modified;
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _repository.Appointments.Where(w => w.id == id).FirstOrDefault();

            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete([Bind(Exclude = "id,PatientId, DoctorId,Appointment_date,Appointment_Time")] int? id, String Success, int? Rating)
        {
            if (ModelState.IsValid)
            {
                Appointment lcl_Appointment = _repository.Appointments.Where(x => x.id == id).FirstOrDefault();
                if (Success == null)
                {
                    ViewBag.Error = "Please select Success or Unsuccess";
                    return View(lcl_Appointment);
                }
                if (Rating == null)
                {
                    ViewBag.Date = "Please select Rating";
                    return View(lcl_Appointment);
                }
                if(Success == "Success")
                {
                    lcl_Appointment.Success = true;
                }
                else
                {
                    lcl_Appointment.Success = false;
                }

                lcl_Appointment.Rating = Rating;
                _repository.Entry(lcl_Appointment).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
