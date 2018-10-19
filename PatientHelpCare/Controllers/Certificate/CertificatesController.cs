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
    public class CertificatesController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Certificates
        public async Task<ActionResult> Index()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
            var certificates = _repository.Certificates.Include(c => c.Doctor);
            return View(await certificates.Where(x => x.DoctorId == Id).ToListAsync());
        }

        // GET: Certificates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = await _repository.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        // GET: Certificates/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname");
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CertificateId,ProfessionalCertificateorAward,ConferringOrganization,Description,Year,DoctorId")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                var User = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
                certificate.DoctorId = User.Id;
                _repository.Certificates.Add(certificate);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", certificate.DoctorId);
            return View(certificate);
        }

        // GET: Certificates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = await _repository.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", certificate.DoctorId);
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CertificateId,ProfessionalCertificateorAward,ConferringOrganization,Description,Year,DoctorId")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                _repository.Entry(certificate).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", certificate.DoctorId);
            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = await _repository.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Certificate certificate = await _repository.Certificates.FindAsync(id);
            _repository.Certificates.Remove(certificate);
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
