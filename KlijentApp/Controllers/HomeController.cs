using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KlijentApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if ( Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else { return View(); }
               
        }

        public ActionResult About()
        {
            ViewBag.Message = "Opis poslova.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt informacije.";

            return View();
        }
    }
}