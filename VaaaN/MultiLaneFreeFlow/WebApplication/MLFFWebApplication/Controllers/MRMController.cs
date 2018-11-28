using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{
    public class MRMController : Controller
    {
        private MessageQueue smsMessageQueue;

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas;

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
                ViewData["apiPath"] = HelperClass.GetAPIUrl();
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
                ViewData["apiPath"] = HelperClass.GetAPIUrl();
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
            DateTime dtstartTime = Convert.ToDateTime(transaction.StartDate);
            string strstarttime = dtstartTime.ToString("dd/MM/yyyy HH:mm:ss");
            DateTime dtendTime = Convert.ToDateTime(transaction.EndDate);
            string strendtime = dtendTime.ToString("dd/MM/yyyy HH:mm:ss");
            //string strstarttime = transaction.StartDate;
            //string strendtime = transaction.EndDate;
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
                strQuery += " AND NVL(IS_BALANCE_UPDATED,0) <> 1";
            }
            else if (transaction.TransactionCategoryId == 1)// Only IKE
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND NVL(IS_BALANCE_UPDATED,0) <> 1";
            }
            else if (transaction.TransactionCategoryId == 2)//IKE + FRONT ALPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0  AND LOWER(NFPF.PLATE_NUMBER)<>'not detected' AND NVL(T.NF_ENTRY_ID_REAR,0) = 0 AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 3)//IKE + REAR ALPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) = 0 AND NVL(T.NF_ENTRY_ID_REAR,0) > 0  AND LOWER(NFPR.PLATE_NUMBER)<>'not detected' AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 4)//FRONT ALPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0  AND LOWER(NFPF.PLATE_NUMBER)<>'not detected' AND NVL(T.NF_ENTRY_ID_REAR,0) = 0 AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 5)//REAR ALPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) = 0 AND NVL(T.NF_ENTRY_ID_REAR,0) > 0  AND LOWER(NFPR.PLATE_NUMBER)<>'not detected' AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 6)//FRONT ALPR + REAR ALPR
            {
                strQuery += "  AND NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0  AND LOWER(NFPF.PLATE_NUMBER)<>'not detected' AND NVL(T.NF_ENTRY_ID_REAR,0) > 0  AND LOWER(NFPR.PLATE_NUMBER)='not detected' AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 7)//UNIDENTIFIED FRONT ALPR
            {
                strQuery += " AND LOWER(NFPF.PLATE_NUMBER)='not detected'  AND LOWER(NFPR.PLATE_NUMBER)='not detected' AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND NVL(T.NF_ENTRY_ID_REAR,0) = 0 AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (transaction.TransactionCategoryId == 8)//UNIDENTIFIED REAR ALPR
            {
                strQuery += " AND LOWER(NFPR.PLATE_NUMBER)='not detected' AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) = 0 AND NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }

            DataTable dt = new DataTable();
            dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);
            ViewData["apiPath"] = HelperClass.GetAPIUrl();
            return PartialView("_ManualTransactions", dt);
        }

        public ActionResult AssociatedTransaction(string transactionId, string TransactionCategoryId)
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
                ViewBag.NFRear = transactiondata.Rows[0]["REAR_IMAGE"].ToString();
                ViewBag.NFFront = transactiondata.Rows[0]["FRONT_IMAGE"].ToString();
                ViewBag.NFFrontVideo = transactiondata.Rows[0]["FRONT_VIDEO_URL"].ToString();
                ViewBag.NFRearVideo = transactiondata.Rows[0]["REAR_VIDEO_URL"].ToString();
                ViewBag.VehicleSpeed = transactiondata.Rows[0]["VEHICLESPEED"].ToString();

                //check CT_Entry_ID exists if yes we will not fetch CT_entry_ID associated transaction
                if (transactiondata.Rows[0]["CT_ENTRY_ID"].ToString() != null && transactiondata.Rows[0]["CT_ENTRY_ID"].ToString() != "")
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
            strQuery += " AND  TRANSACTION_DATETIME BETWEEN TO_DATE('" + strTransactionTime + "','DD/MM/YYYY HH24:MI:SS') - INTERVAL '1' MINUTE AND  TO_DATE('" + strTransactionTime + "','DD/MM/YYYY HH24:MI:SS')  + INTERVAL '1' MINUTE";
            if (Convert.ToInt32(TransactionCategoryId) == 0)
            {
                //Case 1 Cross Talk entry Id Exists and Nodeflux Front Entry Id Exists and Nodeflux Rear Entry Id Exists
                if (ctEntryId != 0 && nffrontEntryId != 0 && nfRearEntryId == 0)
                {
                    //Return No Transaction
                }
                //Case 2 Cross Talk entry Id Exists and Nodeflux Front Entry Id Exists and Nodeflux Rear Entry Id not Exists
                else if (ctEntryId != 0 && nffrontEntryId != 0 && nfRearEntryId == 0)
                {
                    strQuery += " AND T.NF_ENTRY_ID_REAR IS NOT NULL";
                }
                //Case 3 Cross Talk entry Id Exists and Nodeflux Front Entry Id Not Exists and Nodeflux Rear Entry Id Exists
                else if (ctEntryId != 0 && nffrontEntryId == 0 && nfRearEntryId != 0)
                {
                    strQuery += " AND T.NF_ENTRY_ID_FRONT IS NOT NULL";
                }
                //Case 4 Cross Talk entry Id Exists and Nodeflux Front Entry Id Not Exists and Nodeflux Rear Entry Id NOT Exists
                else if (ctEntryId != 0 && nffrontEntryId == 0 && nfRearEntryId == 0)
                {
                    strQuery += " AND (T.NF_ENTRY_ID_FRONT IS NOT NULL OR T.NF_ENTRY_ID_REAR IS NOT NULL)";
                }
                //Case 5 Cross Talk entry Id Not Exists and Nodeflux Front Entry Id  Exists and Nodeflux Rear Entry Id Exists
                else if (ctEntryId == 0 && nffrontEntryId != 0 && nfRearEntryId != 0)
                {
                    strQuery += " AND T.CT_ENTRY_ID IS NOT NULL ";
                }
                //Case 6 Cross Talk entry Id Not Exists and Nodeflux Front Entry Id  Exists and Nodeflux Rear Entry Id NOT Exists
                else if (ctEntryId == 0 && nffrontEntryId != 0 && nfRearEntryId == 0)
                {
                    strQuery += " AND (T.CT_ENTRY_ID IS NOT NULL OR T.NF_ENTRY_ID_REAR IS NOT NULL) ";
                }
                //Case 7 Cross Talk entry Id Not Exists and Nodeflux Front Entry Id  Not Exists and Nodeflux Rear Entry Id Exists
                else if (ctEntryId == 0 && nffrontEntryId == 0 && nfRearEntryId != 0)
                {
                    strQuery += " AND (T.CT_ENTRY_ID IS NOT NULL OR T.NF_ENTRY_ID_FRONT IS NOT NULL) ";
                }
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 1)
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0  AND ((NVL(T.NF_ENTRY_ID_FRONT, 0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'not detected') OR (NVL(T.NF_ENTRY_ID_REAR, 0) > 0 AND LOWER(NFPR.PLATE_NUMBER) <> 'not detected')) AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 2)
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0  AND NVL(T.NF_ENTRY_ID_FRONT, 0)= 0 AND NVL(T.NF_ENTRY_ID_REAR,0)>0  AND LOWER(NFPR.PLATE_NUMBER)<>'not detected' AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 3)
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0  AND NVL(T.NF_ENTRY_ID_FRONT,0)>0  AND LOWER(NFPF.PLATE_NUMBER)<>'not detected' AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 4)
            {
                strQuery += " AND  NVL(T.NF_ENTRY_ID_FRONT,0)  = 0  AND (NVL(T.CT_ENTRY_ID,0)>0  OR (NVL(T.NF_ENTRY_ID_REAR,0)>0 AND LOWER(NFPR.PLATE_NUMBER)<>'not detected'))  AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 5)
            {
                strQuery += " AND  NVL(T.NF_ENTRY_ID_REAR,0)  = 0  AND (NVL(T.CT_ENTRY_ID,0)>0  OR (NVL(T.NF_ENTRY_ID_FRONT,0)>0 AND LOWER(NFPF.PLATE_NUMBER)<>'not detected')) AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 6)
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0  AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 7)
            {
                strQuery += " AND (LOWER(NFPR.PLATE_NUMBER)='not detected'  OR  NVL(T.CT_ENTRY_ID,0)  > 0 ) AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }
            else if (Convert.ToInt32(TransactionCategoryId) == 8)
            {
                strQuery += " AND (LOWER(NFPF.PLATE_NUMBER)='not detected'  OR  NVL(T.CT_ENTRY_ID,0)  > 0 ) AND NVL(IS_BALANCE_UPDATED,0) <> 1 ";
            }


            DataTable dt = new DataTable();
            dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);
            ViewData["apiPath"] = HelperClass.GetAPIUrl();
            return PartialView("_AssociatedTimeTransaction", dt);
        }

        //GET :VRN Search
        public JsonResult SearchVRN(string VehRegNo)
        {
            JsonResult result = new JsonResult();
            Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle = new Libraries.CommonLibrary.CBE.CustomerVehicleCBE();
            customerVehicle.VehRegNo = VehRegNo;
            //Check this VRN Exists in record
            customerVehicle = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleByVehRegNo(customerVehicle);
            if (customerVehicle != null)//i.e customer exists
            {
                result.Data = "VRN Exists";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Data = "VRN Not Exists";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

      

        [HttpPost]
        public JsonResult JoinTransactions(string[] AssociatedTransactionIds, string TransactionId, string VehRegNo, string vehicleClassID)
        {
            JsonResult result = new JsonResult();
            int vehicleClassId = Convert.ToInt32(vehicleClassID);//Get by Audited
            DateTime transactionDatetime = DateTime.Now;
            int isBalanceUpdated = 0;
            long childOneTranasactionId = 0;
            long childTwoTranasactionId = 0;
            StringBuilder returnMessage = new StringBuilder();

            //get Parent Transaction Data
            Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
            transaction.TransactionId = Convert.ToInt32(TransactionId);
            DataTable parentTransaction = new DataTable();
            parentTransaction = Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_GetById(transaction);

            if (parentTransaction != null && parentTransaction.Rows != null && parentTransaction.Rows.Count > 0)
            {
                if (parentTransaction.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                {
                    isBalanceUpdated = 1;
                }
                transaction.LaneId = Convert.ToInt32(parentTransaction.Rows[0]["LANE_ID"].ToString());
                transactionDatetime = Convert.ToDateTime(parentTransaction.Rows[0]["TRANSACTION_DATETIME"].ToString());
            }
            if (AssociatedTransactionIds != null)
            {
                //Check Reviewer Selected One Child
                if (AssociatedTransactionIds.Length > 0)
                {
                    childOneTranasactionId = Convert.ToInt64(AssociatedTransactionIds[0]);
                }
                if (AssociatedTransactionIds.Length > 1)
                {
                    childTwoTranasactionId = Convert.ToInt64(AssociatedTransactionIds[1]);
                }
            }
            transaction.IsBalanceUpdated = isBalanceUpdated;
            transaction.TransactionDateTime = transactionDatetime;
            transaction.ModifierId = Convert.ToInt32(Session["LoggedUserId"].ToString());
            transaction.ModificationDate = DateTime.Now;
            transaction.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();
            transaction.PlazaId = Libraries.CommonLibrary.Constants.GetCurrentPlazaId();
            #region Financial and Notification By Manual Review
            if (isBalanceUpdated != 1)
            {
                //Charging and Notification Logics
                //get Customer Vehicle from customer VRN
                Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = new Libraries.CommonLibrary.CBE.CustomerVehicleCBE();
                customerVehicleInfo.VehRegNo = VehRegNo;
                customerVehicleInfo = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleByVehRegNo(customerVehicleInfo);
                customerVehicleInfo.VehicleClassId = vehicleClassId;

                //get customer account info from customer VRN
                Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo = new Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                customerAccountInfo.AccountId = customerVehicleInfo.AccountId;
                customerAccountInfo = Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customerAccountInfo);

                //financial operation here
                FinancialProcessing(customerVehicleInfo, customerAccountInfo, transaction);
                //Update Is Blanace Updated =1
                isBalanceUpdated = 1;
                HelperClass.LogMessage("Financial processing has been done.");
                //notification operation here
                //NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction);

            }
            else
            {
                returnMessage.Append("Balance not Deducted as Balance already deducted for this Transaction");

            }
            #endregion
            //In this region i have to call new package
            #region Join and Audit Section of Transactions
            //Call Function UpdateAuditJoinTransaction Here
            Libraries.CommonLibrary.BLL.TransactionBLL.JoinAuditTransaction(transaction.TransactionId, childOneTranasactionId, childTwoTranasactionId, VehRegNo, vehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()));
            #endregion
            returnMessage.Append("SucessFully Audited and Join Transaction");
            result.Data = returnMessage;
            return Json(result);
        }
        #endregion

        #region-------------Helper Methods--------------------
        private void FinancialProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            #region LaneType and TollRate Section
            decimal tollToDeduct = -1;
            try
            {
                HelperClass.LogMessage("Finding out LaneType and toll rate to deduct...");
                int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                HelperClass.LogMessage("LaneType is: " + laneTypeId);
                tollToDeduct = GetTollRate(Libraries.CommonLibrary.Constants.GetCurrentPlazaId(), laneTypeId, transaction.TransactionDateTime, customerVehicleInfo.VehicleClassId);
                HelperClass.LogMessage("Toll to deduct is (for motorcycle it may be 0.00): " + tollToDeduct);


            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Exception in finding out lane type and toll to deduct. " + ex.ToString());
                tollToDeduct = -1;
            }
            #endregion

            if (tollToDeduct > -1)
            {
                Decimal CurrentAccountBalance = customerAccountInfo.AccountBalance;
                Decimal AfterDeduction = CurrentAccountBalance - tollToDeduct;
                if (AfterDeduction > 0)
                {
                    #region Account History Section
                    try
                    {
                        HelperClass.LogMessage("Trying to record in account history table...");
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory = new Libraries.CommonLibrary.CBE.AccountHistoryCBE();
                        accountHistory.TMSId = transaction.TMSId;
                        //accountHistory.EntryId = 0;//this  is the auto incremented and primery key of table
                        accountHistory.AccountId = customerAccountInfo.AccountId;
                        accountHistory.CustomerVehicleEntryId = customerVehicleInfo.EntryId; //<============================= 
                        accountHistory.TransactionTypeId = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransactionType.LaneDebit;
                        accountHistory.TransactionId = transaction.TransactionId;
                        accountHistory.Amount = tollToDeduct;
                        accountHistory.IsSMSSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent; //will be updated later on
                        accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent; ;//will be updated later on
                                                                                                                                //accountHistory.ModifierId = 1;//will be updated later on
                        accountHistory.CreationDate = DateTime.Now;
                        accountHistory.ModificationDate = DateTime.Now;
                        accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
                        HelperClass.LogMessage("Recorded in account history table successfully.");
                    }
                    catch (Exception ex)
                    {
                        HelperClass.LogMessage("Exception in recording in the Account History table. " + ex.ToString());
                    }
                    #endregion

                    #region Update Balance Section
                    try
                    {
                        HelperClass.LogMessage("Trying to update balance in customer account table...");
                        //should be by by trigger defined in TBL_ACCOUNT_HISTORY
                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateBalance(customerAccountInfo, (-1 * tollToDeduct));
                        HelperClass.LogMessage("Balance updated successfully in the customer account.");
                    }
                    catch (Exception ex)
                    {
                        HelperClass.LogMessage("Exception in updating customer's account balance. " + ex.ToString());
                    }
                    #endregion

                    #region Mark transaction as balance updated
                    try
                    {
                        HelperClass.LogMessage("Trying to update isBalanceUpdated field in transaction table...");
                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsBalanceUpdated(transaction);
                        HelperClass.LogMessage("Transaction is marked as balance updated.");
                    }
                    catch (Exception ex)
                    {
                        HelperClass.LogMessage("Exception in marking the transaction as balance updated. " + ex.ToString());
                    }
                    #endregion

                }
                else {
                    HelperClass.LogMessage("Due to insufficient balance.");
                    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, AfterDeduction);
                }

            }
            else
            {
                HelperClass.LogMessage("Toll to deduct is -1.00. There is some error somewhere.");
            }
        }

        private void NotificationProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Decimal tollToDeduct, Decimal AfterDeduction)
        {
            try
            {
                HelperClass.LogMessage("Trying to push SMS to MSMQ...");

                Message smsMessage = new Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                CultureInfo culture = new CultureInfo("id-ID");
                string RechareDate = transaction.TransactionDateTime.AddDays(4).ToString("dd-MMM-yyyy") + " 23:59:59";
                if (AfterDeduction > 0)
                {
                    smsDetail.SMSMessage = "Pelanggan Yth, telah dilakukan pemotongan senilai Rp " + Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", "") + " terhadap saldo SJBE anda atas transaksi kendaraan " + customerVehicleInfo.VehRegNo + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS) + " di tempat " + GetPlazaNameById(transaction.PlazaId) + ". Sisa saldo SJBE anda saat ini Rp " + Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture).Replace("Rp", "") + " Ref: [" + transaction.TransactionId.ToString() + "]";
                }
                else {
                    smsDetail.SMSMessage = "Pelanggan Yth, Saldo SJBE anda saat ini tidak mencukupi untuk dilakukan pemotongan senilai Rp " + Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", "") + " atas transaksi kendaraan " + customerVehicleInfo.VehRegNo + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS) + " di Gantry - Medan Merdeka Barat 1. Silahkan melakukan pengisian ulang saldo SJBE anda sebelum " + RechareDate + ". Keterlambatan pengisian ulang saldo akan dikenakan denda sebesar Rp 1.000.000,00. Sisa saldo SJBE anda saat ini Rp " + Decimal.Parse((AfterDeduction + tollToDeduct).ToString()).ToString("C", culture).Replace("Rp", "") + " Ref: [" + transaction.TransactionId.ToString() + "]";
                }

                HelperClass.LogMessage(smsDetail.SMSMessage);
                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;

                smsMessage.Body = smsDetail;

                HelperClass.LogMessage("Detail:" + smsDetail.ToString());
                smsMessageQueue.Send(smsMessage);
                HelperClass.LogMessage("Message pushed successfully to MSMQ.");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Exception in pushing SMS to SMS MSMQ. " + ex.ToString());
            }
        }
        private int GetLaneTypeByLaneId(int laneId)
        {
            int result = -1;

            try
            {
                lanes = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll();
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
                {
                    if (lane.LaneId == laneId)
                    {
                        result = lane.LaneTypeId;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        private decimal GetTollRate(int plazaId, int laneType, DateTime transactionTime, int vehicleClass)
        {
            decimal result = -1;

            try
            {
                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection currentTimeTollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();

                //DateTime currentStartDate = new DateTime();
                //DateTime currentEndDate = new DateTime();
                //DateTime actualEndDate = new DateTime(); //CJS
                tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection currentTimeTollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();

                currentTimeTollRates = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetTollRateCollection(transactionTime, tollRates);
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in currentTimeTollRates)
                {
                    if (tr.PlazaId == plazaId && tr.LaneTypeId == laneType && tr.VehicleClassId == vehicleClass)
                    {
                        result = tr.Amount;
                        break;
                    }
                }
                // Get toll rate as per transaction time
                //foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in tollRates)
                //{
                //    DateTime currentDate = transactionTime;

                //    // Get Start hour and minute
                //    int startHour = Convert.ToInt32(tr.StartTime.Substring(0, 2));
                //    int startMinute = Convert.ToInt32(tr.StartTime.Substring(3, 2));

                //    int endHour = Convert.ToInt32(tr.EndTime.Substring(0, 2));
                //    int endMinute = Convert.ToInt32(tr.EndTime.Substring(3, 2));

                //    Console.WriteLine(startHour + ", " + startMinute + " -> " + endHour + ", " + endMinute);

                //    currentStartDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startHour, startMinute, currentDate.Second);
                //    currentEndDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endHour, endMinute, currentDate.Second);

                //    if (startHour > endHour)// Cross day
                //    {
                //        actualEndDate = currentEndDate.AddDays(1); //this value need to be assigned to another vehicle CJS
                //    }
                //    else
                //    {
                //        actualEndDate = currentEndDate; //CJS
                //    }

                //    if (currentDate > currentStartDate && currentDate < actualEndDate)
                //    {
                //        currentTimeTollRates.Add(tr);
                //    }
                //}

                //foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in currentTimeTollRates)
                //{

                //    if (tr.PlazaId == plazaId && tr.LaneTypeId == laneType && tr.VehicleClassId == vehicleClass)
                //    {
                //        result = tr.Amount;
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to get toll rate." + ex.Message);
                result = -1;
            }

            return result;
        }
        private string GetPlazaNameById(int plazaId)
        {
            string result = string.Empty;

            try
            {
                plazas = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsCollection();
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza in plazas)
                {
                    if (plaza.PlazaId == plazaId)
                    {
                        result = plaza.PlazaName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Exception in getting plaza name for plazaId " + plazaId);
                result = string.Empty;
            }

            return result;
        }

        public ActionResult SessionPage()
        {
            return View();
        }
        #endregion
    }
}