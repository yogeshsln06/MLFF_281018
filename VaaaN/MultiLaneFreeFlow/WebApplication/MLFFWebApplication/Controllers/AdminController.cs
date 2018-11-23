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
                HelperClass.LogMessage("API URL : " + HelperClass.GetAPIUrl());
                ViewData["apiPath"] = HelperClass.GetAPIUrl();
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                string strQuery = " WHERE 1=1 ";

                strQuery += " AND (NVL(T.CT_ENTRY_ID,0) > 0 AND (NVL(T.NF_ENTRY_ID_FRONT,0) > 0 OR NVL(T.NF_ENTRY_ID_REAR,0) > 0))";
                
               
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