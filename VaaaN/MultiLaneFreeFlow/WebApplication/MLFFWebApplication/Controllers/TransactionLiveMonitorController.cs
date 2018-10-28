using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.Common;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{

    public class TransactionLiveMonitorController : Controller
    {
        static MessageQueue eventQueue;
        // GET: TransactionLiveMonitor
        public ActionResult Index()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewData["apiPath"] = System.Configuration.ConfigurationManager.AppSettings["apiPath"];
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                #region Gantry Class Dropdown
                List<SelectListItem> gantryList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

                gantryList.Add(new SelectListItem() { Text = "--Select Gantry--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                {
                    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                }

                ViewBag.Gantry = gantryList;

                #endregion

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Transaction List" + ex);
            }
            return View();
        }

      

        [HttpGet]
        public string Transaction_LiveData()
        {
            string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_LiveData(), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }



    }
}