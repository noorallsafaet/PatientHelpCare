using PatientHelpCare.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientHelpCare.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();
        public ActionResult Dashboard()
        {
            var CurrentUser = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            ViewBag.UserType = CurrentUser.UserType;
            if (CurrentUser.UserType == "Doctor")
            {
                var LastMessage = _repository.Messages.Where(x => x.DoctorId == CurrentUser.Id).OrderByDescending(x=>x.DateTime).ToList();
                ViewBag.LastMessage = LastMessage.Take(5);

                var LastAppointment = _repository.Appointments.Where(x => x.DoctorId == CurrentUser.Id).OrderByDescending(x=>x.Appointment_date).ToList();
                ViewBag.LastAppointment = LastAppointment.Take(5);
            }
            else
            {
                var LastMessage = _repository.Messages.Where(x => x.PatientId == CurrentUser.Id).OrderByDescending(x => x.DateTime).ToList();
                ViewBag.LastMessage = LastMessage.Take(5);
                var LastAppointment = _repository.Appointments.Where(x => x.PatientId == CurrentUser.Id).OrderByDescending(x => x.Appointment_date).ToList();
                ViewBag.LastAppointment = LastAppointment.Take(5);
            }
            return View();
        }

        public ActionResult Dashboard_2()
        {
            return View();
        }

        public ActionResult Dashboard_3()
        {
            return View();
        }
        
        public ActionResult Dashboard_4()
        {
            return View();
        }
        
        public ActionResult Dashboard_4_1()
        {
            return View();
        }

        public ActionResult Dashboard_5()
        {
            return View();
        }

    }
}