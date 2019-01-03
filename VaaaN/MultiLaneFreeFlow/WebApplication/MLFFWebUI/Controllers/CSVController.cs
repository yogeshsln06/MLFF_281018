using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;

namespace MLFFWebUI.Controllers
{
    public class CSVController : Controller
    {
        // GET: CSV
        public ActionResult Index()
        {
            return View();
        }

        #region Customer Account
        public string ExportCSVCustomer(string ViewId)
        {
            var filename = "Customer_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            try
            {
                FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
                Int16 IsDataFound = CSVUtility.CreateCsv(file.FullName, CustomerAccountBLL.GetAllAsCSV());
                if (IsDataFound == 0)
                    filename = "No Data to Export.";
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed Export Customer CSV " + ex);
            }
            string Det = JsonConvert.SerializeObject(filename, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion
    }
}