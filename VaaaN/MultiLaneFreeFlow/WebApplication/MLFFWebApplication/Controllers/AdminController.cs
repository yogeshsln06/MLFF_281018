using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            DataTable dt = new DataTable();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewData["apiPath"] = System.Configuration.ConfigurationManager.AppSettings["apiPath"];
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                string strQuery = " WHERE 1=1 ";
              
                    strQuery += " AND T.IS_BALANCE_UPDATED = " + 1 + " ORDER BY T.TRANSACTION_ID DESC";
                
               
                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Dashboard" + ex);
            }
            return View("Dashboard",dt);
        }
    }
}