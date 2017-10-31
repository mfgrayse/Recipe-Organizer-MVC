using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recipe_Organizer_MVC.Controllers
{
    public class MasterPageController : Controller
    {
        // GET: MasterPage
        public ActionResult Index()
        {
            return View();
        }
    }
}