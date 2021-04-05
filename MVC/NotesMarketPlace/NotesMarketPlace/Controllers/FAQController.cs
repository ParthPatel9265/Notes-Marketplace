using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class FAQController : Controller
    {
        [Route("FAQ")]
        public ActionResult FAQ()
        {
           
            ViewBag.FAQ = "active";

            return View();
        }
    }
}