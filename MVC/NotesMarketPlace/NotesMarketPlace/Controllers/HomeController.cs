using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Context;

namespace NotesMarketPlace.Controllers
{
    public class HomeController : Controller
    {
        readonly private database1Entities dobj = new database1Entities();

        [AllowAnonymous]
        public ActionResult Index()
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
            return View();
        }
    }
}