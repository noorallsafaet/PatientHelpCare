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
    public class ChambersController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();

        // GET: Chambers
        public async Task<ActionResult> Index()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
            var chambers = _repository.Chambers.Include(c => c.Doctor);
            return View(await chambers.Where(x => x.DoctorId == Id).ToListAsync());
        }

        // GET: Chambers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamber chamber = await _repository.Chambers.FindAsync(id);
            if (chamber == null)
            {
                return HttpNotFound();
            }
            return View(chamber);
        }

        // GET: Chambers/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname");
            return View();
        }

        // POST: Chambers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ChamberId,Name,FromDay,ToDay,StartTime,EndTime,Phone,Email,City,Area,Address1,Address2,ZipCode,DoctorId")] Chamber chamber)
        {
            if (ModelState.IsValid)
            {
                var user = this._repository.AspNetUsers.FirstOrDefault(x => x.Email == this.HttpContext.User.Identity.Name);
                chamber.DoctorId = user.Id;
                _repository.Chambers.Add(chamber);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", chamber.DoctorId);
            return View(chamber);
        }

        // GET: Chambers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamber chamber = await _repository.Chambers.FindAsync(id);
            if (chamber == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", chamber.DoctorId);
            return View(chamber);
        }

        // POST: Chambers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ChamberId,Name,FromDay,ToDay,StartTime,EndTime,Phone,Email,City,Area,Address1,Address2,ZipCode,DoctorId")] Chamber chamber)
        {
            if (ModelState.IsValid)
            {
                var entry = _repository.Entry(chamber);
                entry.State = EntityState.Modified;
                entry.Property(e => e.DoctorId).IsModified = false;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(_repository.Doctors, "DoctorId", "Firstname", chamber.DoctorId);
            return View(chamber);
        }

        // GET: Chambers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chamber chamber = await _repository.Chambers.FindAsync(id);
            if (chamber == null)
            {
                return HttpNotFound();
            }
            return View(chamber);
        }

        // POST: Chambers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Chamber chamber = await _repository.Chambers.FindAsync(id);
            _repository.Chambers.Remove(chamber);
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
