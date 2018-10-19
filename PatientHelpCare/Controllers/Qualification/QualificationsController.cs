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
    public class QualificationsController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Qualifications
        public async Task<ActionResult> Index()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
            var qualifications = _repository.Qualifications.Include(q => q.Doctor);
            return View(await qualifications.Where(x => x.DoctorId == Id).ToListAsync());
        }

        // GET: Qualifications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = await _repository.Qualifications.FindAsync(id);
            if (qualification == null)
            {
                return HttpNotFound();
            }
            return View(qualification);
        }

        // GET: Qualifications/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname");
            return View();
        }

        // POST: Qualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Country,University,Degree,StartYear,EndYear,DoctorId")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                var UserId = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
                qualification.DoctorId = UserId;
                _repository.Qualifications.Add(qualification);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", qualification.DoctorId);
            return View(qualification);
        }

        // GET: Qualifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = await _repository.Qualifications.FindAsync(id);
            if (qualification == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", qualification.DoctorId);
            return View(qualification);
        }

        // POST: Qualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Country,University,Degree,StartYear,EndYear,DoctorId")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                var UserId = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
                qualification.DoctorId = UserId;
                _repository.Entry(qualification).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", qualification.DoctorId);
            return View(qualification);
        }

        // GET: Qualifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qualification qualification = await _repository.Qualifications.FindAsync(id);
            if (qualification == null)
            {
                return HttpNotFound();
            }
            return View(qualification);
        }

        // POST: Qualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Qualification qualification = await _repository.Qualifications.FindAsync(id);
            _repository.Qualifications.Remove(qualification);
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
