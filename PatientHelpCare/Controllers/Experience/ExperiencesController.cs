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
    public class ExperiencesController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Experiences
        public async Task<ActionResult> Index()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
            var experiences = _repository.Experiences.Include(e => e.Doctor);
            return View(await experiences.Where(x => x.DoctorId == Id).ToListAsync());
        }

        // GET: Experiences/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await _repository.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }
            return View(experience);
        }

        // GET: Experiences/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname");
            return View();
        }

        // POST: Experiences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Designation,Organization,StartDate,EndDate,Running,Summery")] Experience experience)
        {
            if (ModelState.IsValid)
            {
                var UserId = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
                experience.DoctorId = UserId;
                _repository.Experiences.Add(experience);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", experience.DoctorId);
            return View(experience);
        }

        // GET: Experiences/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await _repository.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", experience.DoctorId);
            return View(experience);
        }

        // POST: Experiences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExperienceId,Designation,Organization,StartDate,EndDate,Running,Summery,DoctorId")] Experience experience)
        {
            if (ModelState.IsValid)
            {
                _repository.Entry(experience).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", experience.DoctorId);
            return View(experience);
        }

        // GET: Experiences/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await _repository.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }
            return View(experience);
        }

        // POST: Experiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Experience experience = await _repository.Experiences.FindAsync(id);
            _repository.Experiences.Remove(experience);
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
