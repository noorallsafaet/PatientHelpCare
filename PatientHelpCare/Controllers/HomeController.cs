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
    public class HomeController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();
        private const int DefaultPageSize = 10;
        public async Task<ActionResult> Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int currentPageIndexConversation = page.HasValue ? page.Value - 1 : 0;
            var DoctorList = _repository.Doctors.Where(x => x.BMDC != null).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);
            return View(DoctorList);
        }
        [HttpPost]
        public async Task<ActionResult> Index(int? page, string Name, string Location, string Area, string Specialist, string ddlDhakaArea)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int currentPageIndexConversation = page.HasValue ? page.Value - 1 : 0;
            var Search = _repository.Doctors.Where(x=>x.BMDC != null).OrderBy(x => x.DoctorId).ToList();

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Doctors(int? page, string Name, string Location, string ddl2Dhaka, string Disease, string ddlDhakaArea)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int currentPageIndexConversation = page.HasValue ? page.Value - 1 : 0;
            var Search = _repository.Doctors.Where(x => x.BMDC != null).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);

            if (Name != "")
            {
                Search = Search.Where(x => (x.Firstname + " " + x.Lastname).IndexOf(Name, StringComparison.OrdinalIgnoreCase) != -1).ToPagedList(currentPageIndex, DefaultPageSize);
            }
            if (Disease != "0")
            {
                Search = Search.Where(x => x.Specialist == Disease).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);
            }
            if (Location != "0")
            {
                Search = Search.Where(x => x.Location == Location).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);
            }
            if (ddl2Dhaka != "0")
            {
                Search = Search.Where(x => x.Area == ddl2Dhaka).OrderBy(x => x.DoctorId).ToPagedList(currentPageIndex, DefaultPageSize);
                if (ddlDhakaArea != "0")
                {
                    Search = Search.Where(x => (x.Address1 + " " + x.Address2).IndexOf(ddlDhakaArea, StringComparison.OrdinalIgnoreCase) != -1).ToPagedList(currentPageIndex, DefaultPageSize);
                }
            }
            return View(Search);
        }

        public async Task<ActionResult> Details(string id)
        {
            ViewBag.Message = "Doctor Details";
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
    }
}