using NotesMarketPlace.Context;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class ContactUsController : Controller
    {
        database1Entities dobj = new database1Entities();
      
        [HttpGet]
        [AllowAnonymous]
        [Route("ContactUs")]
        public ActionResult ContactUs()
        {

            //if logged in user's userrole is not member then redirect to admin dashboard
            if (User.Identity.IsAuthenticated)
            {
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                if (user.RoleID != dobj.UserRole.Where(x => x.Name.ToLower() == "member").Select(x => x.ID).FirstOrDefault())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            // viewbag for active class in navigation
            ViewBag.Contactus = "active";
            // check if user is authenticated then we need to show full name and email
            if (User.Identity.IsAuthenticated)
            {
                // if user is authenticated then get user
                var user = dobj.Users.Where(x => x.EmailID == User.Identity.Name).FirstOrDefault();
                // create contact us viewmodel
                Models.ContactUs viewmodel = new Models.ContactUs();

                viewmodel.FullName = user.FirstName + " " + user.LastName;
                viewmodel.EmailID = user.EmailID;
                // return viewmodel
                return View(viewmodel);
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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

                dobj.ContactUs.Add(obj);
                SendEmailToAdmin(obj);
                dobj.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ContactUs");
            }
           else
            {
                return View(model);

            }
           
        }

        [NonAction]
        public void SendEmailToAdmin(Context.ContactUs obj)
        {
            var email = dobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
            var email2 = dobj.SystemConfiguration.Select(x => x.EmailID2).FirstOrDefault();

            var fromEmail = new MailAddress(email); 
            var toEmail = new MailAddress(email2);
            var fromEmailPassword = "#######"; //Replace with original password
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