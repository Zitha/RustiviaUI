using System;
using System.Web.Mvc;
using IntroductionMVC5.Web.Models;
using System.Linq;

namespace IntroductionMVC5.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(string date)
        {
            ViewBag.SearchDate = date ?? DateTime.Now.ToString("d");
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
    }
}