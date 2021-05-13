using NotesMarketPlace.Context;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NotesMarketPlace.Models;

namespace NotesMarketPlace.Controllers
{ 
    public class SignUpController : Controller
    {
        database1Entities dbobj = new database1Entities();
     
        [HttpGet]
        [AllowAnonymous]
        [Route("SignUp")]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("SignUp")]
        public ActionResult SignUp(Models.Users model)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(model.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailID","Email already exist");
                    return View(model);
                }
              
                Context.Users obj = new Context.Users();
                obj.RoleID = 3;
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = model.Password;
                obj.IsEmailVerified = model.IsEmailVerified;
                obj.IsActive = true;
                obj.CreatedDate = DateTime.Now;
                obj.SecretCode = Guid.NewGuid();

                dbobj.Users.Add(obj);
                dbobj.SaveChanges();
                SendVerificationLinkEmail(model.EmailID, model.FirstName, obj.SecretCode.ToString());
                TempData["Success"] = "Your account has been created.";
               
            }
           
                ModelState.Clear();
                return RedirectToAction("SignUp");
            
        }

        [NonAction]  
        public bool IsEmailExist(string emailID)
        {
            using (database1Entities dbobj = new database1Entities())
            {
                var v = dbobj.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }
        

        [Route("EmailVerification/{code}")]
        public ActionResult EmailVerification(string code)
        {
            Context.Users obj = dbobj.Users.Where(x => x.SecretCode.ToString() == code).FirstOrDefault();
            ViewBag.name = obj.FirstName;
            return View(obj);
        }

        [Route("Verify/{code}")]
        public ActionResult Verify(string code)
        {
            Context.Users obj = dbobj.Users.Where(x => x.SecretCode.ToString() == code).FirstOrDefault();
            obj.IsEmailVerified = true;
            dbobj.SaveChanges();
            return RedirectToAction("Login");
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string username, string SecretCode)
        {
            var verifyUrl = "/EmailVerification/" + SecretCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var email = dbobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
            var fromEmail = new MailAddress(email);
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "******"; // Replace with original password
            string subject = "Note Marketplace - Email Verification";

            string body = "Hello " + username + "," +
                "<br/><br/>Thank you for signing up with us. Please click on below link to verify your email address and to do login." +
                "<br/><br/><a href='" + link + "'>" + link + "</a> " +
                "<br/><br/>Regards,<br/>Notes Marketplace";

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

        [HttpGet]
        [AllowAnonymous]
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public ActionResult Login(Models.Users user, string ReturnUrl = "")
        {
            using (database1Entities dbobj = new database1Entities())
            {
                var v = dbobj.Users.Where(a => a.EmailID == user.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (v.IsEmailVerified == true)
                    {
                        if (string.Compare( user.Password, v.Password) == 0)
                        {
                            int timeout = user.RememberMe ? 525600 : 20; // 525600 minute = 1 year
                            var ticket = new FormsAuthenticationTicket(user.EmailID, user.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            if (v.RoleID == 3)
                            {
                                
                                var upobj = dbobj.UserProfileDetail.Where(a => a.UserID == v.ID).FirstOrDefault();
                                if (upobj == null)
                                {
                                    return RedirectToAction("UserProfile", "UserProfile");
                                }
                                else if (!String.IsNullOrEmpty(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("SearchNotes", "SearchNotes");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "Admin");

                            }
                        }
                        else
                        {
                          
                            ModelState.AddModelError("Password", "Invalid Password");
                            return View(user);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("EmailID", "Email is not verified");
                        return View(user);
                    }
                }
                else
                {
                   
                    ModelState.AddModelError("EmailID", "Invalid Email");
                    return View(user);
                }
            }  
        }

        [Authorize(Roles = "SuperAdmin,Admin,Member")]
        [Route("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(Models.ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(model.EmailID);
                if (isExist == false)
                {
                    ModelState.AddModelError("EmailID", "Email Does not exist");
                    return View(model);
                }
                Context.Users obj = dbobj.Users.Where(x => x.EmailID == model.EmailID).FirstOrDefault();
                string pwd = Membership.GeneratePassword(6, 2);
                obj.Password = pwd;
                dbobj.SaveChanges();
                SendPassword(obj.EmailID, pwd);
                TempData["Success"] = "New password has been sent to your email";
                return RedirectToAction("ForgotPassword");
            }
            else
            {
                ModelState.AddModelError("EmailID", "Email is required");
                return View(model);
            }
        }

        [NonAction]
        public void SendPassword(string emailID, string pwd)
        {
            var email = dbobj.SystemConfiguration.Select(x => x.EmailID1).FirstOrDefault();
            var fromEmail = new MailAddress(email);
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "*****"; //replace with original 
            string subject = "Note Marketplace - Forgot Password";

            string body = "Hello," +
                "<br/><br/>We have generated a new password for you" +
                "<br/><br/>Password: " + pwd +
                "<br/><br/>Regards,<br/>Notes Marketplace";

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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin,Member")]
        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Member")]
        [Route("ChangePassword")]
        public ActionResult ChangePassword(Models.ChangePassword model)
        {
            var emailid = User.Identity.Name.ToString();
            Context.Users obj = dbobj.Users.Where(x => x.EmailID == emailid).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (string.Compare(model.OldPassword, obj.Password) == 0)
                {
                    obj.Password = model.NewPassword;
                    obj.ModifiedDate = DateTime.Now;

                    dbobj.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    dbobj.SaveChanges();

                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "OldPassword Is Incorrect");
                }
            }
            return View();
        }


    }
}
