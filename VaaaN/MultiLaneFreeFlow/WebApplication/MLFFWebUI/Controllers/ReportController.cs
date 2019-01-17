using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace MLFFWebUI.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        #region VehicleBalanceReport
        public ActionResult VehicleBalanceReport()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Report", "VehicleBalanceReport");
            List<CustomerVehicleCBE> SortedList = CustomerVehicleBLL.GetAllAsList().OrderBy(o => o.VehRegNo).ToList();
            ViewData["CustomerVehicle"] = SortedList;
            return View();
        }

        [HttpPost]
        public string VehicleBalanceReportFilter(Int32 VehicleId, int Month, int Year)
        {
            DataSet ds = CustomerVehicleBLL.GetVehicleBalanceReport(VehicleId, Month, Year);
            string Det = JsonConvert.SerializeObject(ds, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        #endregion
    }
}