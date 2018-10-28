using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ReportSelection()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                #region Report Filter
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection();
                subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(Convert.ToInt16(Session["LoggedUserId"]), 7);  // only for report submodules
                List<SelectListItem> reports = new List<SelectListItem>();
                reports.Add(new SelectListItem() { Text = "--Select Report--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule in subModules)
                {
                    reports.Add(new SelectListItem
                    {
                        Value = Convert.ToString(submodule.SubModuleId),
                        Text = submodule.SubModuleName
                    });

                }
                ViewBag.ReportType = reports;
                #endregion

                #region Gantry Class Dropdown
                List<SelectListItem> gantryList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

                gantryList.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                {
                    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                }

                ViewBag.Gantry = gantryList;

                #endregion

                #region Lane Class Dropdown
                List<SelectListItem> laneList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> Lane = new List<Libraries.CommonLibrary.CBE.LaneCBE>();
                Lane = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>().ToList();

                laneList.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE cr in Lane)
                {
                    laneList.Add(new SelectListItem() { Text = cr.LaneName, Value = System.Convert.ToString(cr.LaneId) });
                }

                ViewBag.Lane = laneList;

                #endregion

                #region Vehicle Class Dropdown
                List<SelectListItem> vehicleClass = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicle = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                vehicleClass.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
                {
                    vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
                }

                ViewBag.VehicleClass = vehicleClass;

                #endregion

                #region Transaction Category
                ViewBag.TransactionCategory = HelperClass.GetReportTransactionCategory();
                #endregion

            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to load Report Selection Page" + ex);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ReportPage()
        {
            return View();
        }

        public JsonResult GenerateReport(string startDate, string endDate, string rptname)
        {
            JsonResult result = new JsonResult();

            Session["startDate"] = null;
            Session["endDate"] = null;
            Session["rptname"] = null;

            Session["startDate"] = startDate;
            Session["endDate"] = endDate;
            Session["rptname"] = rptname;

            result.Data = "saved";

            return result;

        }


        public JsonResult GetAllReports()
        {

            VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection();
            subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(Convert.ToInt16(Session["LoggedUserId"]), 7);  // only for report submodules
            List<SelectListItem> reports = new List<SelectListItem>();
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule in subModules)
            {
                reports.Add(new SelectListItem
                {
                    Value = Convert.ToString(submodule.SubModuleId),
                    Text = submodule.SubModuleName
                });

            }

            return Json(reports);
        }


        #region Helper Method


      
        #endregion

    }
}