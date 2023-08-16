using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
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
        [Authorize]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [Authorize]
        public ActionResult AdministrationDashboard()
        { 
            return View(); 
        }
        [Authorize]
        public ActionResult DepartmentDashboard()
        {
            return View();
        }

        
    }
}