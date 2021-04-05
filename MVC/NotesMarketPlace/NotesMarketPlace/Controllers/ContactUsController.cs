using NotesMarketPlace.Context;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class ContactUsController : Controller
    {
        database1Entities dbobj = new database1Entities();
      
        [HttpGet]
        [Route("ContactUs")]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [Route("ContactUs")]
        public ActionResult ContactUs(Models.ContactUs model)
        {
            if (ModelState.IsValid)
            {
                Context.ContactUs obj = new Context.ContactUs();
                obj.FullName = model.FullName;
                obj.EmailID = model.EmailID;
                obj.Subjects = model.Subjects;
                obj.Comments = model.Comments;

                dbobj.ContactUs.Add(obj);
                SendEmailToAdmin(obj);
                dbobj.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("ContactUs");
        }

        [NonAction]
        public void SendEmailToAdmin(Context.ContactUs obj)
        {
            SystemConfiguration s = new SystemConfiguration();
            var fromEmail = new MailAddress(s.EmailID1); 
            var toEmail = new MailAddress(s.EmailID2);
            var fromEmailPassword = "*****"; 
            string subject = obj.FullName + " - Query";

            string body = "Hello," +
                "<br/><br/>" + obj.Comments +
                "<br/><br/>Regards, " + obj.FullName;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}