using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatientHelpCare.DataModel;

namespace PatientHelpCare.Controllers
{
    public class PatientsController : Controller
    {
        private Doctor_InformationEntities db = new Doctor_InformationEntities();

        // GET: Patients
        public ActionResult Index()
        {
            var CurrentUser = db.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return View(db.Patients.Where(x=>x.Id == CurrentUser.Id).ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,Firstname,Lastname,DateOfBirth,Age,Gender,Blood,Address1,Address2,City,Area,Zip,HomePhone,CellPhone,WorkPhone,Email,MaritialStatus,Username,Status")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "PatientId,Firstname,Lastname,DateOfBirth,Age,Gender,Blood,Address1,Address2,City,Area,Zip,HomePhone,CellPhone,WorkPhone,Email,MaritialStatus,Username,Status")] Patient patient, String Firstname, String Lastname, DateTime DateOfBirth, int Age, String Gender, String Blood, String Address1, String Address2, String City, String Zip, String HomePhone, String CellPhone, String MaritialStatus)
        {
            if (ModelState.IsValid)
            {
                patient.Firstname = Firstname;
                patient.Lastname = Lastname;
                patient.DateOfBirth = DateOfBirth;
                patient.Age = Age;
                patient.Gender = Gender;
                patient.Blood = Blood;
                patient.Address1 = Address1;
                patient.Address2 = Address2;
                patient.City = City;
                patient.Zip = Zip;
                patient.HomePhone = HomePhone;
                patient.CellPhone = CellPhone;
                patient.MaritialStatus = MaritialStatus;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Appointments()
        {
            var Email = HttpContext.User.Identity.Name;
            var Id = db.AspNetUsers.Where(z => z.Email == Email).FirstOrDefault();
            var Appointment = db.Appointments.Where(x=>x.PatientId == Id.Id).ToList();
            return RedirectToAction("Appointments", Appointment);
        }
    }
}
