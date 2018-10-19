using PatientHelpCare.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientHelpCare.Controllers
{
    public class MailboxController : Controller
    {
        private Doctor_InformationEntities _repository = new Doctor_InformationEntities();
        public ActionResult Inbox()
        {
            string Id = _repository.AspNetUsers.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault().Id;
         //   var Messages = _repository.Messages.Include(q => q.Patient);
            return View(_repository.Messages.Where(x => x.DoctorId == Id).ToList());
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
    
        public ActionResult ComposeEmail()
        {
            return View();
        }
    
        public ActionResult EmailTemplates()
        {
            return View();
        }

        public ActionResult BasicActionEmail()
        {
            return View();
        }

        public ActionResult AlertEmail()
        {
            return View();
        }

        public ActionResult BillingEmail()
        {
            return View();
        }
	}
}