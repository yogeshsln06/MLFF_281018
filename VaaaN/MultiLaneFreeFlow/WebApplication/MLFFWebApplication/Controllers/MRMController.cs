using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{
    public class MRMController : Controller
    {
        // GET: MRM
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TransactionList()
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

                gantryList.Add(new SelectListItem() { Text = "--Sel Gantry--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                {
                    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                }

                ViewBag.Gantry = gantryList;

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
                ViewBag.TransactionCategory = HelperClass.GetReviewTransactionCategory();
                #endregion
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Transaction List" + ex);
            }
            return View();
        }

        public string FilterManulReview(Libraries.CommonLibrary.CBE.ViewTransactionCBE transaction)
        {

            //DateTime dtstartTime = transaction.StartTime;
            DateTime dtstartTime = Convert.ToDateTime(transaction.StartDate);
            string strstarttime = dtstartTime.ToString("dd/MM/yyyy HH:MM:ss");
            //DateTime dtendTime = transaction.EndTime;
            DateTime dtendTime = Convert.ToDateTime(transaction.EndDate);
            string strendtime = dtendTime.ToString("dd/MM/yyyy HH:MM:ss");

            string strQuery = " WHERE 1=1 ";

            if (strstarttime != null && strendtime != null)
            {
                strQuery += " AND  TRANSACTION_DATETIME BETWEEN TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strstarttime != null)
            {
                strQuery += " AND  TRANSACTION_DATETIME >= TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strendtime != null)
            {
                strQuery += " AND  TRANSACTION_DATETIME <= TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            if (transaction.GantryId > 0)
            {
                strQuery += " AND T.PLAZA_ID = " + transaction.GantryId;
            }
            if (transaction.VehicleClassId > 0)
            {
                strQuery += " AND (CV_CTP.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + " OR  NFPF.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + " OR  NFPR.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + ")";
            }
            if (!String.IsNullOrEmpty(transaction.PlateNumber))
            {
                strQuery += " AND (CV_CTP.VEH_REG_NO = '" + transaction.PlateNumber + "' OR NFPF.PLATE_NUMBER = '" + transaction.PlateNumber + "' OR  NFPR.PLATE_NUMBER = '" + transaction.PlateNumber + "')";
            }
            if (transaction.TransactionCategoryId == 0)
            {
                strQuery += " AND ((NVL(T.CT_ENTRY_ID, 0) = 0 AND (NVL(T.NF_ENTRY_ID_FRONT, 0) > 0 OR NVL(T.NF_ENTRY_ID_REAR, 0) > 0)) OR (NVL(T.CT_ENTRY_ID, 0) > 0 AND(NVL(T.NF_ENTRY_ID_FRONT, 0) = 0 AND NVL(T.NF_ENTRY_ID_REAR, 0) = 0)))";
            }
            else if (transaction.TransactionCategoryId == 1)
            {
                strQuery += "AND NVL(T.CT_ENTRY_ID,0) > 0 AND (NVL(T.NF_ENTRY_ID_FRONT,0) = 0 OR NVL(T.NF_ENTRY_ID_REAR,0) = 0)";
            }
            else if (transaction.TransactionCategoryId == 2)
            {
                strQuery += " AND NVL(T.CT_ENTRY_ID,0) = 0 AND (NVL(T.NF_ENTRY_ID_FRONT,0) > 0 OR NVL(T.NF_ENTRY_ID_REAR,0) > 0)";
            }
            else if (transaction.TransactionCategoryId == 3)
            {
                strQuery += " AND (NVL(CV_CTP.VEHICLE_CLASS_ID,0) <> NVL(NFPF.VEHICLE_CLASS_ID,0) AND ((NVL(CV_CTP.VEHICLE_CLASS_ID,0) <> NVL(NFPR.VEHICLE_CLASS_ID,0))))";
            }
            else if (transaction.TransactionCategoryId == 4)
            {
                strQuery += " AND NVL(T.AUDIT_STATUS,0)=1";
            }

            string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public JsonResult UpdateAuditer(Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            JsonResult result = new JsonResult();
            try
            {
                transaction.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();
                transaction.PlazaId = Libraries.CommonLibrary.Constants.GetCurrentPlazaId();
                transaction.AuditDate = DateTime.Now;
                transaction.AuditorId = Convert.ToInt32(Session["LoggedUserId"].ToString());
                transaction.AuditStatus = (int)Libraries.CommonLibrary.Constants.AuditStatus.Reviewed;
                Libraries.CommonLibrary.BLL.TransactionBLL.UpdateAuditSection(transaction);
                result.Data = "Sucess";
            }
            catch (Exception ex)
            {
                result.Data = "Failure";
                HelperClass.LogMessage("Failed To Post Transaction List" + ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetTransactionData(Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_GetById(transaction), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        #region Manual Review

        //Get 
        public ActionResult ManualReview()
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

                gantryList.Add(new SelectListItem() { Text = "--Sel Gantry--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                {
                    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                }

                ViewBag.Gantry = gantryList;

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
                ViewBag.TransactionCategory = HelperClass.GetManualReviewTransactionCategory();
                #endregion
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Transaction List" + ex);
            }
            return View();
        }
        public ActionResult ShowTransaction(Libraries.CommonLibrary.CBE.ViewTransactionCBE transaction)
        {
            DateTime dtstartTime = transaction.StartTime;
            string strstarttime = dtstartTime.ToString("dd/MM/yyyy HH:mm:ss");
            DateTime dtendTime = transaction.EndTime;
            string strendtime = dtendTime.ToString("dd/MM/yyyy HH:mm:ss");

            string strQuery = " WHERE 1=1 ";

            //if (strstarttime != null && strendtime != null)
            //{
            //    strQuery += " AND  TRANSACTION_DATETIME BETWEEN TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            //}
            //else if (strstarttime != null)
            //{
            //    strQuery += " AND  TRANSACTION_DATETIME >= TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS')";
            //}
            //else if (strendtime != null)
            //{
            //    strQuery += " AND  TRANSACTION_DATETIME <= TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            //}
            if (transaction.GantryId > 0)
            {
                strQuery += " AND T.PLAZA_ID = " + transaction.GantryId;
            }
            if (transaction.VehicleClassId > 0)
            {
                strQuery += " AND (CV_CTP.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + " OR  NFPF.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + " OR  NFPR.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + ")";
            }
            if (!String.IsNullOrEmpty(transaction.PlateNumber))
            {
                strQuery += " AND (CV_CTP.VEH_REG_NO = '" + transaction.PlateNumber + "' OR NFPF.PLATE_NUMBER = '" + transaction.PlateNumber + "' OR  NFPR.PLATE_NUMBER = '" + transaction.PlateNumber + "')";
            }
            if (transaction.TransactionCategoryId == 0)
            {
                strQuery += " AND NVL(IS_BALANCE_UPDATED,0) <> 1";
                //strQuery += " AND ((NVL(T.CT_ENTRY_ID, 0) = 0 AND (NVL(T.NF_ENTRY_ID_FRONT, 0) > 0 OR NVL(T.NF_ENTRY_ID_REAR, 0) > 0)) OR (NVL(T.CT_ENTRY_ID, 0) > 0 AND(NVL(T.NF_ENTRY_ID_FRONT, 0) = 0 AND NVL(T.NF_ENTRY_ID_REAR, 0) = 0)))";
            }
            else if (transaction.TransactionCategoryId == 1)
            {
                strQuery += " AND T.CT_ENTRY_ID IS NOT NULL ";
                //strQuery += "AND NVL(T.CT_ENTRY_ID,0) > 0 AND (NVL(T.NF_ENTRY_ID_FRONT,0) = 0 OR NVL(T.NF_ENTRY_ID_REAR,0) = 0)";
            }
            else if (transaction.TransactionCategoryId == 2)
            {
                strQuery += " AND T.NF_ENTRY_ID_FRONT IS NOT NULL ";
                // strQuery += " AND NVL(T.CT_ENTRY_ID,0) = 0 AND (NVL(T.NF_ENTRY_ID_FRONT,0) > 0 OR NVL(T.NF_ENTRY_ID_REAR,0) > 0)";
            }
            else if (transaction.TransactionCategoryId == 3)
            {
                strQuery += " AND T.NF_ENTRY_ID_REAR IS NOT NULL ";
                //strQuery += " AND (NVL(CV_CTP.VEHICLE_CLASS_ID,0) <> NVL(NFPF.VEHICLE_CLASS_ID,0) AND ((NVL(CV_CTP.VEHICLE_CLASS_ID,0) <> NVL(NFPR.VEHICLE_CLASS_ID,0))))";
            }

            ////  string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery), Formatting.Indented);
            //// return Det.Replace("\r", "").Replace("\n", "");
            //// List<Libraries.CommonLibrary.CBE.TransactionCBE> transactionList = new List<Libraries.CommonLibrary.CBE.TransactionCBE>();

            DataTable dt = new DataTable();
            dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);

            return PartialView("_ManualTransactions", dt);
        }

        public ActionResult AssociatedTransaction(string transactionId)
        {
            DateTime transactiondatetime = DateTime.Now;
            string strTransactionTime = string.Empty;
            int ctEntryId = 0;
            int nffrontEntryId = 0;
            int nfRearEntryId = 0;

            string strfilter = " WHERE 1=1 ";
            strfilter += " AND T.TRANSACTION_ID = " + transactionId;
            Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();

            transaction.TransactionId = Convert.ToInt32(transactionId);
            //Get TransactionTime from Transaction Id
            DataTable transactiondata = Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strfilter);

            if (transactiondata != null && transactiondata.Rows != null && transactiondata.Rows.Count > 0)
            {
                transactiondatetime = Convert.ToDateTime(transactiondata.Rows[0]["TRANSACTION_DATETIME"].ToString());
                strTransactionTime = transactiondatetime.ToString("dd/MM/yyyy HH:mm:ss");

                ViewBag.TransactionId = transactionId;
                ViewBag.TransactionTime = strTransactionTime;
                ViewBag.CrossTalkVRN = transactiondata.Rows[0]["CTP_VRN"].ToString();
                ViewBag.Class = transactiondata.Rows[0]["CTP_VEHICLE_CLASS_NAME"].ToString();
                ViewBag.NFRear = transactiondata.Rows[0]["FRONT_IMAGE"].ToString();
                ViewBag.NFFront = transactiondata.Rows[0]["REAR_IMAGE"].ToString();

                //check CT_Entry_ID exists if yes we will not fetch CT_entry_ID associated transaction
                if (transactiondata.Rows[0]["CT_ENTRY_ID"].ToString()!=null && transactiondata.Rows[0]["CT_ENTRY_ID"].ToString()!="")
                {
                    ctEntryId = Convert.ToInt32(transactiondata.Rows[0]["CT_ENTRY_ID"].ToString());
                }
                //check NFFrontEntryID exists if yes we will not fetch NFFrontEntryID associated transaction
                if (transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"].ToString() != null && transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"].ToString() != "")
                {
                    nffrontEntryId = Convert.ToInt32(transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"].ToString());
                }
                //check NFREAREntryID exists if yes we will not fetch NFREAREntryID associated transaction
                if (transactiondata.Rows[0]["NF_ENTRY_ID_REAR"].ToString() != null && transactiondata.Rows[0]["NF_ENTRY_ID_REAR"].ToString() != "")
                {
                    nfRearEntryId = Convert.ToInt32(transactiondata.Rows[0]["NF_ENTRY_ID_REAR"].ToString());
                }


                #region Vehicle Class Dropdown 
                List<SelectListItem> vehicleClass = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicle = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                vehicleClass.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
                {
                    vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
                }

                ViewBag.AuditorVehicleClass = vehicleClass;

                #endregion
            }

            string strQuery = " WHERE 1=1 ";
            strQuery += " AND T.TRANSACTION_ID <> " + transactionId;
            //Make Query if CT_ENTRY_ID not exists i.e. CT_ENTRY_ENTRY_ID=0
            if (ctEntryId == 0)
            {
                strQuery += " AND T.CT_ENTRY_ID IS NOT NULL";
            }
            //Make Query if NF_FRONT_ENTRY_ID not exists i.e. NF_FRONT_ENTRY_ID=0
            if (nffrontEntryId == 0)
            {
                strQuery += " AND T.NF_ENTRY_ID_FRONT IS NOT NULL";
            }
            //Make Query if NF_FRONT_ENTRY_ID not exists i.e. NF_FRONT_ENTRY_ID=0
            if (nfRearEntryId == 0)
            {
                strQuery += " AND T.NF_ENTRY_ID_REAR IS NOT NULL";
            }
            strQuery += " AND  TRANSACTION_DATETIME BETWEEN TO_DATE('" + strTransactionTime + "','DD/MM/YYYY HH24:MI:SS') - INTERVAL '1' MINUTE AND  TO_DATE('" + strTransactionTime + "','DD/MM/YYYY HH24:MI:SS')  + INTERVAL '1' MINUTE";

           
            DataTable dt = new DataTable();
            dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);

            return PartialView("_AssociatedTimeTransaction", dt);
        }

        [HttpPost]
        public JsonResult JoinTransactions(string[] AssociatedTransactionIds, string TransactionId)
        {
            int associatedtransactionId = 0;
            int ctEntryId = 0;
            int nfRearEntryId = 0;
            int nffrontEntryId = 0;

            Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
            //Get TransactionTime from Transaction Id
            DataTable transactiondata = new DataTable();
            JsonResult result = new JsonResult();
            for (int i = 0; i < AssociatedTransactionIds.Length; i++)
            {
                associatedtransactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                string strfilter = " WHERE 1=1 ";
                strfilter += " AND T.TRANSACTION_ID = " + associatedtransactionId;
                transaction.TransactionId = associatedtransactionId;

                //transactiondata = Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strfilter);
                transactiondata = Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_GetById(transaction);

                if (transactiondata != null && transactiondata.Rows != null && transactiondata.Rows.Count > 0)
                {

                    HelperClass.LogMessage("Trying To Read CTEnrtyId, NodeFlux Rear Entry Id, NodeFlux Front Entry Id");
                    if (transactiondata.Rows[0]["CT_ENTRY_ID"] != null && transactiondata.Rows[0]["CT_ENTRY_ID"].ToString() != "")
                    {
                        HelperClass.LogMessage("CTEnrtyId Found");
                        ctEntryId = Convert.ToInt32(transactiondata.Rows[0]["CT_ENTRY_ID"].ToString());
                    }
                    HelperClass.LogMessage("CTEnrtyId not Found");
                    if (transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"] != null && transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"].ToString() != "")
                    {
                        HelperClass.LogMessage("NodeFlux Front Entry Id Found");
                        nffrontEntryId = Convert.ToInt32(transactiondata.Rows[0]["NF_ENTRY_ID_FRONT"].ToString());
                    }
                    HelperClass.LogMessage("NodeFlux Front Entry Id not Found");
                    if (transactiondata.Rows[0]["NF_ENTRY_ID_REAR"] != null && transactiondata.Rows[0]["NF_ENTRY_ID_REAR"].ToString() != "")
                    {
                        HelperClass.LogMessage("NodeFlux Rear Entry Id Found");
                        nfRearEntryId = Convert.ToInt32(transactiondata.Rows[0]["NF_ENTRY_ID_REAR"].ToString());
                    }
                    HelperClass.LogMessage("NodeFlux Rear Entry Id not Found");
                    HelperClass.LogMessage("Ready to update Transaction");

                    #region Update Transaction By Manual Review

                    #endregion

                }

            }
            //Update transaction
            //  Libraries.CommonLibrary.BLL.TransactionBLL.up
            return Json(result);
        }
        #endregion


    }
}