using NotesMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace NotesMarketPlace.Controllers
{
    public class FAQController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();

        [AllowAnonymous]
        [Route("FAQs")]
        public ActionResult FAQ()
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

            ViewBag.FAQ = "active";

            return View();
        }
    }
}