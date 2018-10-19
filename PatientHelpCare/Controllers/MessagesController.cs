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
    public class MessagesController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Messages
        public async Task<ActionResult> Index()
        {
            var User = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            ViewBag.UserType = User.UserType;
            if(User.UserType == "Doctor")
            {
                var messages = _repository.Messages.Include(m => m.Patient);
                return View(await messages.Where(x => x.DoctorId == User.Id).ToListAsync());
            }
            else
            {
                var messages = _repository.Messages.Include(m => m.Doctor);
                return View(await messages.Where(x => x.PatientId == User.Id).ToListAsync());
            }
            return View();
        }

        public ActionResult EmailView()
        {
            var CurrentUser = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            if (CurrentUser.UserType == "Doctor")
            {
                var LastMessage = _repository.Messages.Where(x => x.DoctorId == CurrentUser.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                ViewBag.LastMessage = LastMessage;
                var Sentby = _repository.AspNetUsers.Where(x => x.Id == LastMessage.Sentby).FirstOrDefault();
                ViewBag.From = Sentby;
            }
            else
            {
                var LastMessage = _repository.Messages.Where(x => x.PatientId == CurrentUser.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                ViewBag.LastMessage = LastMessage;
                var Sentby = _repository.AspNetUsers.Where(x => x.Id == LastMessage.Sentby).FirstOrDefault();
                ViewBag.From = Sentby;
            }
            return View();
        }

        // GET: Messages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentUser = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            if (CurrentUser.UserType == "Doctor")
            {
                var LastMessage = _repository.Messages.Where(x => x.Id==id && x.DoctorId == CurrentUser.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                ViewBag.LastMessage = LastMessage;
                var Sentby = _repository.AspNetUsers.Where(x => x.Id == LastMessage.Sentby).FirstOrDefault().Email;
                ViewBag.From = Sentby;
            }
            else
            {
                var LastMessage = _repository.Messages.Where(x => x.Id == id && x.PatientId == CurrentUser.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                ViewBag.LastMessage = LastMessage;
                var Sentby = _repository.AspNetUsers.Where(x => x.Id == LastMessage.Sentby).FirstOrDefault().Email;
                ViewBag.From = Sentby;
            }

            Message message = await _repository.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(_repository.Patients, "Id", "Firstname");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(string Email, string Subject, string Message1)
        {
            Message message = new Message();
            if (ModelState.IsValid)
            {               
                var User = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
                if(User.UserType == "Doctor")
                {
                    message.DoctorId = User.Id;
                    message.Sentby = User.Id;

                    var Patient = _repository.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault();
                    message.PatientId = Patient.Id;
                }
                else
                {
                    message.PatientId = User.Id;
                    message.Sentby = User.Id;

                    var Doctor = _repository.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault();
                    message.DoctorId = Doctor.Id;
                }
                message.Message1 = Message1;
                message.Subject = Subject;
                message.MarkRead = false;
                message.DateTime = DateTime.Now;
                _repository.Messages.Add(message);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }            
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await _repository.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(_repository.Patients, "Id", "Firstname", message.PatientId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Subject,Message1,DoctorId,PatientId,DateTime,MarkRead,Sentby")] Message message)
        {
            if (ModelState.IsValid)
            {
                _repository.Entry(message).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(_repository.Patients, "Id", "Firstname", message.PatientId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await _repository.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Message message = await _repository.Messages.FindAsync(id);
            _repository.Messages.Remove(message);
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
    }
}
