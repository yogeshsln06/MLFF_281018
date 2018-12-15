using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLFFWebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }

       
    }
}