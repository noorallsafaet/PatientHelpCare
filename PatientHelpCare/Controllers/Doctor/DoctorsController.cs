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
using MvcPaging;

namespace PatientHelpCare.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();
        private const int DefaultPageSize = 10;
        // GET: Doctors
        public async Task<ActionResult> Index()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
            return View(await _repository.Doctors.Where(x=>x.DoctorId == Id).ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = await _repository.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            var Chamber = _repository.Chambers.Where(x => x.DoctorId == id).ToList();
            ViewBag.Chamber = Chamber;
            var Experience = _repository.Experiences.Where(x => x.DoctorId == id).ToList();
            ViewBag.Experience = Experience;
            var Qualification = _repository.Qualifications.Where(x => x.DoctorId == id).ToList();
            ViewBag.Qualification = Qualification;
            var Certificate = _repository.Certificates.Where(x => x.DoctorId == id).ToList();
            ViewBag.Certificate = Certificate;
            var TotalAppoinment = _repository.Appointments.Where(x => x.DoctorId == id).ToList().Count;
            ViewBag.TotalAppoinment = TotalAppoinment;
            var Successful = _repository.Appointments.Where(x => x.DoctorId == id && x.Success == true).ToList().Count;
            ViewBag.Successful = Successful;
            var Unsuccessful = _repository.Appointments.Where(x => x.DoctorId == id && x.Success == false).ToList().Count;
            ViewBag.Unsuccessful = Unsuccessful;
            var Declined = _repository.Appointments.Where(x => x.DoctorId == id && x.Status == "Declined").ToList().Count;
            ViewBag.Declined = Declined;
            return View(doctor);            
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(_repository.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DoctorId,Firstname,Lastname,Title,BMDC,Specialist,OverView,DateOfBirth,Gender,PhotoLocation,PhotoName,Mobile,Homephone,Officephone,Address1,Address2,City,ZipCode,Active")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.JoinDate = DateTime.Now;
                _repository.Doctors.Add(doctor);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = await _repository.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = "Firstname,Lastname,Title,BMDC,Specialist,OverView,Fee,DateOfBirth,Gender,PhotoLocation,PhotoName,Mobile,Homephone,Officephone,Address1,Address2,City,ZipCode,JoinDate,Active")] string DoctorId, string Firstname, string Lastname, string Title, string BMDC, string Specialist, string OverView, int Fee, DateTime DateOfBirth, string Gender, HttpPostedFileBase PhotoLocation, string PhotoName, string Mobile, string Homephone, string Officephone, string City, string Location, string Area, string Address1, string Address2, int ZipCode)
        {
            if (String.IsNullOrWhiteSpace(Firstname))
            {
                ModelState.AddModelError("Firstname", "Firstname is required");
            }
            if (String.IsNullOrWhiteSpace(Lastname))
            {
                ModelState.AddModelError("Lastname", "Lastname is required");
            }
            if (String.IsNullOrWhiteSpace(Title))
            {
                ModelState.AddModelError("Title", "Title is required");
            }
            if (String.IsNullOrWhiteSpace(BMDC))
            {
                ModelState.AddModelError("BMDC", "BMDC is required");
            }
            if (String.IsNullOrWhiteSpace(Specialist) || Specialist == "0")
            {
                ModelState.AddModelError("Specialist", "Specialist is required");
            }
            if (String.IsNullOrWhiteSpace(OverView))
            {
                ModelState.AddModelError("OverView", "OverView is required");
            }

            if (Fee == null)
            {
                ModelState.AddModelError("Fee", "Fee is required");
            }

            if (String.IsNullOrWhiteSpace(Gender) || Gender == "0")
            {
                ModelState.AddModelError("Gender", "Gender is required");
            }
            if (String.IsNullOrWhiteSpace(Location) || Location == "0")
            {
                ModelState.AddModelError("Location", "Location is required");
            }
            if (String.IsNullOrWhiteSpace(Area) || Area == "0")
            {
                ModelState.AddModelError("Area", "Area is required");
            }
            if (String.IsNullOrWhiteSpace(Address1))
            {
                ModelState.AddModelError("Address", "Address is required");
            }
            if (ZipCode == 0 || ZipCode == null)
            {
                ModelState.AddModelError("ZipCode", "ZipCode is required");
            }

            if (ModelState.IsValid)
            {
                Doctor lcl_Doctor = _repository.Doctors.Where(x=>x.DoctorId == DoctorId).FirstOrDefault();
                if (PhotoLocation != null)
                {
                    string pic = Guid.NewGuid() + ".jpeg";
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images"), pic);
                    // file is uploaded
                    PhotoLocation.SaveAs(path);
                    lcl_Doctor.PhotoLocation = @"/Content/Images/" + pic;
                }
                lcl_Doctor.Firstname = Firstname;
                lcl_Doctor.Lastname = Lastname;
                lcl_Doctor.Title = Title;
                lcl_Doctor.BMDC = BMDC;
                lcl_Doctor.Specialist = Specialist;
                lcl_Doctor.OverView = OverView;
                lcl_Doctor.Fee = Fee;
                lcl_Doctor.DateOfBirth = DateOfBirth;
                lcl_Doctor.Gender = Gender;
                lcl_Doctor.Mobile = Mobile;
                lcl_Doctor.Homephone = Homephone;
                lcl_Doctor.Officephone = Officephone;
                lcl_Doctor.City = City;
                lcl_Doctor.Location = Location;
                lcl_Doctor.Area = Area;
                lcl_Doctor.Address1 = Address1;
                lcl_Doctor.Address2 = Address2;
                lcl_Doctor.ZipCode = ZipCode;
                lcl_Doctor.Active = true;
                _repository.Entry(lcl_Doctor).State = EntityState.Modified;
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Doctors/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = await _repository.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Doctor doctor = await _repository.Doctors.FindAsync(id);
            _repository.Doctors.Remove(doctor);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        public async Task<ActionResult> Doctors(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int currentPageIndexConversation = page.HasValue ? page.Value - 1 : 0;
            var DoctorList = _repository.Doctors.Where(x=> x.BMDC != null).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);
            return View(DoctorList);
        }

        [HttpPost]
        public async Task<ActionResult> Doctors(int? page, string Name,string Location, string Area, string Specialist, string ddlDhakaArea)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int currentPageIndexConversation = page.HasValue ? page.Value - 1 : 0;
            var Search = _repository.Doctors.Where(x => x.BMDC != null).OrderBy(x => x.DoctorId).ToList();

            if (Name != "")
            {
                Search = Search.Where(x => (x.Firstname + " " + x.Lastname).IndexOf(Name, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
            if (Specialist != "0")
            {
                Search = Search.Where(x => x.Specialist == Specialist).ToList();
            }
            if (Location != "0")
            {
                Search = Search.Where(x => (x.Location).IndexOf(Location, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
            if (Area != "0")
            {
                Search = Search.Where(x => (x.Area).IndexOf(Area, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                if (ddlDhakaArea != "0")
                {
                    Search = Search.Where(x => (x.Address1 + " " + x.Address2).IndexOf(ddlDhakaArea, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                }
            }
            return View(Search);
        }

        public ActionResult SendMessage(string DoctorId, String Subject, String Message)
        {
            var CurrentUser = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            Message lcl_Message = new Message();
            lcl_Message.DoctorId = DoctorId;
            lcl_Message.PatientId = CurrentUser.Id;
            lcl_Message.Subject = Subject;
            lcl_Message.Message1 = Message;
            lcl_Message.DateTime = DateTime.Now;
            lcl_Message.MarkRead = false;
            lcl_Message.Sentby = CurrentUser.Id;
            _repository.Messages.Add(lcl_Message);
            _repository.SaveChanges();
            return RedirectToAction("Doctors");
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
