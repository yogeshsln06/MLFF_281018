using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using static MLFFWebUI.Models.HelperClass;
using System.Messaging;
using System.Globalization;
using Newtonsoft.Json;

namespace MLFFWebUI.Controllers
{

    public class CustomerTransactionController : Controller
    {
        #region Globle Valarible 
        LaneCollection lanes;
        TollRateCollection tollRates;
        PlazaCollection plazas;
        DataTable dt = new DataTable();
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration smsFileConfig;
        private MessageQueue smsMessageQueue;
        #endregion

        // GET: CustomerTransaction
        public ActionResult Index()
        {
            return View();
        }

        #region Unreviewed
        public ActionResult Unreviewed()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Unreviewed");
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

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

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

            return View();
        }

        [HttpPost]
        public string UnreviewedListScroll(int pageindex, int pagesize)
        {
            JsonResult result = new JsonResult();
            dt = TransactionBLL.GetUnReviewedDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string UnreviewedFilter(ViewTransactionCBE transaction)
        {
            JsonResult result = new JsonResult();
            string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
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
            if (transaction.TransactionCategoryId == 1)// IKE Only
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) = 0  AND NVL(T.NF_ENTRY_ID_REAR,0) = 0";
            }
            else if (transaction.TransactionCategoryId == 2)//IKE + Front/Rear ANPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND ((NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'not detected')  OR (NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)<>'not detected')) ";
            }
            else if (transaction.TransactionCategoryId == 3)//Front/Rear ANPR Only
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND ((NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'not detected')  OR (NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)<>'not detected')) ";
            }
            else if (transaction.TransactionCategoryId == 4)//Unidentified Front/Rear ANPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND ((NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)='not detected')  OR (NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)='not detected')) ";
            }
            else if (transaction.TransactionCategoryId == 5)//Front & Rear ANPR Only
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'not detected'  AND NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)<>'not detected' ";
            }
            dt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        #region Associated Transcation
        public ActionResult AssociatedTransaction(int TransactionId)
        {
            try
            {
                dt = TransactionBLL.GetUnReviewedDataTableById(TransactionId);
                Session["CurrentTransaction"] = dt;

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Associated Transaction finding Controller" + ex);
            }

            return PartialView("AssociatedTransaction", dt);
        }

        [HttpPost]
        public string GetAssociated(int Seconds)
        {
            string Det = "No Record Found";
            dt = (DataTable)Session["CurrentTransaction"];
            int IKEEntryId = 0;
            int ANPRFrontEntryId = 0;
            int ANPRRearEntryId = 0;
            DateTime TransactionDateTime;
            int TransactionId;
            if (dt.Rows.Count > 0)
            {
                TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"]);
                TransactionId = Convert.ToInt32(dt.Rows[0]["TRANSACTION_ID"]);
                if (!string.IsNullOrEmpty(dt.Rows[0]["CT_ENTRY_ID"].ToString()))
                {
                    IKEEntryId = Convert.ToInt32(dt.Rows[0]["CT_ENTRY_ID"]);
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                {
                    ANPRFrontEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                {
                    ANPRRearEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_REAR"]);
                }

                if (IKEEntryId > 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId > 0)
                {
                    Det = "No Record Found";

                }
                else
                {
                    string strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + TransactionDateTime.AddSeconds(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID <> " + TransactionId;

                    if (IKEEntryId > 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId == 0)
                    {
                        strfilter += " AND (NVL(CT_ENTRY_ID,0) = 0 AND (NVL(NF_ENTRY_ID_FRONT,0) > 0 OR NVL(NF_ENTRY_ID_REAR,0) > 0))";
                    }
                    else if (IKEEntryId == 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId == 0)
                    {
                        strfilter += " AND (NVL(NF_ENTRY_ID_FRONT,0) = 0 AND (NVL(CT_ENTRY_ID,0) > 0 OR NVL(NF_ENTRY_ID_REAR,0) > 0))";
                    }
                    else if (IKEEntryId == 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId > 0)
                    {
                        strfilter += " AND (NVL(NF_ENTRY_ID_REAR,0) = 0 AND (NVL(CT_ENTRY_ID,0) > 0 OR NVL(NF_ENTRY_ID_FRONT,0) > 0))";
                    }
                    else if (IKEEntryId > 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId == 0)
                    {
                        strfilter += " AND NVL(CT_ENTRY_ID,0) = 0 AND NVL(NF_ENTRY_ID_FRONT,0) = 0) AND (NVL(NF_ENTRY_ID_REAR,0) <> 0 ";
                    }
                    else if (IKEEntryId > 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId > 0)
                    {
                        strfilter += " AND NVL(CT_ENTRY_ID,0) = 0 AND NVL(NF_ENTRY_ID_FRONT,0) <> 0 AND NVL(NF_ENTRY_ID_REAR,0) = 0 ";
                    }
                    else if (IKEEntryId == 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId > 0)
                    {
                        strfilter += " AND NVL(CT_ENTRY_ID,0) <> 0 AND NVL(NF_ENTRY_ID_FRONT,0) = 0 AND NVL(NF_ENTRY_ID_REAR,0) = 0 ";
                    }
                    DataTable Assodt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strfilter);
                    Det = JsonConvert.SerializeObject(Assodt, Formatting.Indented);
                    Det = Det.Replace("\r", "").Replace("\n", "");
                }

            }
            return Det;
        }
        #endregion

        #region Unidentified
        [HttpPost]
        public JsonResult SaveUnidentified(int TransactionId)
        {
            JsonResult result = new JsonResult();
            try
            {
                dt = TransactionBLL.GetUnReviewedDataTableById(TransactionId);
                if (dt.Rows.Count > 0)
                {
                    TransactionCBE objtransaction = new TransactionCBE();
                    objtransaction.TransactionId = TransactionId;
                    objtransaction.TMSId = Constants.GetCurrentTMSId();
                    objtransaction.PlazaId = Constants.GetCurrentPlazaId();
                    objtransaction.ModifierId = Convert.ToInt32(Session["LoggedUserId"].ToString());
                    objtransaction.ModificationDate = DateTime.Now;
                    objtransaction.TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"].ToString());
                    objtransaction.LaneId = Convert.ToInt32(dt.Rows[0]["LANE_ID"].ToString());
                    objtransaction.AuditDate = DateTime.Now;
                    objtransaction.AuditStatus = 1;
                    objtransaction.AuditorId = Convert.ToInt32(Session["LoggedUserId"].ToString());
                    objtransaction.AuditedVRN = "";
                    objtransaction.AuditedVehicleClassId = 0;
                    objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Unidentified;
                    TransactionBLL.UpdateAuditSection(objtransaction);
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "success";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "This transaction already reviewed.";
                    objResponseMessage.Add(objModelState);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Save Unidentified transaction in Customer Transaction Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }

            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Audited
        [HttpPost]
        public JsonResult CompleteReviewed(string[] AssociatedTransactionIds, int TransactionId, string VehRegNo, int vehicleClassID, int Seconds)
        {
            JsonResult result = new JsonResult();

            Int32 AuditedVehicleClassId = Convert.ToInt32(vehicleClassID);
            Int32 ParentTransactionId = TransactionId;
            String AuditedVRN = VehRegNo;
            bool IsAuditedVRNExists = false;

            Int16 AssociatedTransactionCount = 0;
            Int32 FirstChildTranasactionId = 0;
            Int32 SecondChildTranasactionId = 0;

            Int16 ParentPacketCount = 0;
            Int32 ParentIKEEntryId = 0;
            Int32 ParentANPRFrontEntryId = 0;
            Int32 ParentIKEVehicleClassId = 0;
            Int32 ParentANPRFrontVehicleClassId = 0;
            Int32 ParentANPRRearEntryId = 0;
            Int32 ParentANPRRearVehicleClassId = 0;

            bool ParentBalanceAlreadyDeducated = false;
            bool ParentViolation = false;
            bool ParentAuditied = false;
            bool FirstChildAuditied = false;
            bool SecondChildAuditied = false;
            bool ChildViolation = false;
            bool ValidTransactionsProcess = true;
            bool FirstChildBalanceAlreadyDeducated = false;
            bool SecondChildBalanceAlreadyDeducated = false;

            Int32 ChildIKEEntryId = 0;
            Int32 ChildIKEVehicleClassId = 0;
            Int32 ChildAnprFrontEntryId = 0;
            Int32 ChildANPRFrontVehicleClassId = 0;
            Int32 ChildAnprRearEntryId = 0;
            Int32 ChildANPRRearVehicleClassId = 0;
            Int32 ChildPacketCount = 0;



            if (string.IsNullOrEmpty(AuditedVRN))
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "VRN is required for reviewing transaction.";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                try
                {
                    #region Get transaction Data  by id

                    dt = TransactionBLL.GetDataTableFilteredRecordById(TransactionId);
                    if (dt.Rows.Count > 0)
                    {

                        TransactionCBE objtransaction = new TransactionCBE();
                        objtransaction.TransactionId = ParentTransactionId;
                        objtransaction.TMSId = Constants.GetCurrentTMSId();
                        objtransaction.PlazaId = Constants.GetCurrentPlazaId();
                        objtransaction.ModifierId = Convert.ToInt32(Session["LoggedUserId"].ToString());
                        objtransaction.ModificationDate = DateTime.Now;
                        objtransaction.TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"].ToString());
                        objtransaction.LaneId = Convert.ToInt32(dt.Rows[0]["LANE_ID"].ToString());
                        objtransaction.AuditDate = DateTime.Now;
                        objtransaction.AuditStatus = 1;
                        objtransaction.AuditorId = Convert.ToInt32(Session["LoggedUserId"].ToString());
                        objtransaction.AuditedVRN = AuditedVRN;
                        objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                        #region Customer Vehicle Info by VRN
                        CustomerVehicleCBE CustomerVehicleDetails = new CustomerVehicleCBE();
                        CustomerVehicleCBE objCustomerVehicleDetails = new CustomerVehicleCBE();
                        CustomerAccountCBE customerAccountInfo = new CustomerAccountCBE();
                        CustomerVehicleDetails.VehRegNo = VehRegNo;
                        objCustomerVehicleDetails = CustomerVehicleBLL.GetCustomerVehicleByVehRegNo(CustomerVehicleDetails);
                        if (objCustomerVehicleDetails == null)
                            CustomerVehicleDetails.VehRegNo = "";
                        else
                        {
                            CustomerVehicleDetails = objCustomerVehicleDetails;
                            customerAccountInfo.AccountId = CustomerVehicleDetails.AccountId;
                            customerAccountInfo.TmsId = 1;
                            customerAccountInfo = CustomerAccountBLL.GetCustomerById(customerAccountInfo);
                        }
                        #endregion



                        #region Check auditied VRN Exists or not
                        if (!string.IsNullOrEmpty(CustomerVehicleDetails.VehRegNo))
                        {
                            IsAuditedVRNExists = true;
                        }
                        #endregion

                        if (IsAuditedVRNExists)
                        {
                            #region Check which pakcet is missing in parent
                            if (!string.IsNullOrEmpty(dt.Rows[0]["CT_ENTRY_ID"].ToString())) //IKE is missing
                            {
                                ParentPacketCount++;
                                ParentIKEEntryId = Convert.ToInt32(dt.Rows[0]["CT_ENTRY_ID"]);
                                ParentIKEVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))//Front ANPR missing
                            {
                                ParentPacketCount++;
                                ParentANPRFrontEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                ParentANPRFrontVehicleClassId = Convert.ToInt32(dt.Rows[0]["NFP_VEHICLE_CLASS_ID_FRONT"]);
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))// Rear ANPR missing
                            {
                                ParentPacketCount++;
                                ParentANPRRearEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                ParentANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["NFP_VEHICLE_CLASS_ID_REAR"]);
                            }
                            #endregion

                            #region check Balance already deduct or not for parent
                            if (dt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                            {
                                ParentBalanceAlreadyDeducated = true;
                            }
                            #endregion

                            #region check already Audited or not for parent
                            if (dt.Rows[0]["AUDIT_STATUS"].ToString() == "1")
                            {
                                ParentAuditied = true;
                            }
                            #endregion
                            #region Check Associated TransactionIds is avaliable or not 
                            if (AssociatedTransactionIds != null)
                            {
                                if (AssociatedTransactionIds.Length > 0)
                                {
                                    AssociatedTransactionCount++;
                                    FirstChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[0]);
                                }
                                if (AssociatedTransactionIds.Length > 1)
                                {
                                    AssociatedTransactionCount++;
                                    SecondChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[1]);
                                }
                            }
                            #endregion
                            if (AssociatedTransactionCount > 0)
                            {
                                #region Associated transaction Found Going to check its selection is valid or not
                                #region Find 1st Associated transaction releated pakcet id and is balance already deducated or not
                                if (FirstChildTranasactionId > 0)
                                {
                                    #region First Child Pakcet Details
                                    DataTable FirstChilddt = TransactionBLL.GetDataTableFilteredRecordById(FirstChildTranasactionId);
                                    if (FirstChilddt.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(FirstChilddt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                        {
                                            if (ParentIKEEntryId == 0)
                                            {
                                                ChildIKEEntryId = Convert.ToInt32(FirstChilddt.Rows[0]["CT_ENTRY_ID"]);
                                                ChildIKEVehicleClassId = Convert.ToInt32(FirstChilddt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(FirstChilddt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                        {
                                            if (ParentANPRFrontEntryId == 0)
                                            {
                                                ChildAnprFrontEntryId = Convert.ToInt32(FirstChilddt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                ChildANPRFrontVehicleClassId = Convert.ToInt32(FirstChilddt.Rows[0]["NFP_VEHICLE_CLASS_ID_FRONT"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(FirstChilddt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                        {
                                            if (ParentANPRRearEntryId == 0)
                                            {
                                                ChildAnprRearEntryId = Convert.ToInt32(FirstChilddt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                ChildANPRRearVehicleClassId = Convert.ToInt32(FirstChilddt.Rows[0]["NFP_VEHICLE_CLASS_ID_REAR"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(FirstChilddt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                        {
                                            if (FirstChilddt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                            {
                                                FirstChildBalanceAlreadyDeducated = true;
                                            }
                                        }
                                        if (FirstChilddt.Rows[0]["AUDIT_STATUS"].ToString() == "1")
                                        {
                                            FirstChildAuditied = true;
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region Find 2nd Associated transaction releated pakcet id and is balance already deducated or not
                                if (SecondChildTranasactionId > 0)
                                {
                                    #region Second Child Pakcet Details
                                    DataTable SecondChilddt = TransactionBLL.GetDataTableFilteredRecordById(SecondChildTranasactionId);
                                    if (SecondChilddt.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(SecondChilddt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                        {
                                            if (ParentIKEEntryId == 0 && ChildIKEEntryId == 0)
                                            {
                                                ChildIKEEntryId = Convert.ToInt32(SecondChilddt.Rows[0]["CT_ENTRY_ID"]);
                                                ChildIKEVehicleClassId = Convert.ToInt32(SecondChilddt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(SecondChilddt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                        {
                                            if (ParentANPRFrontEntryId == 0 && ChildAnprFrontEntryId == 0)
                                            {
                                                ChildAnprFrontEntryId = Convert.ToInt32(SecondChilddt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                ChildANPRFrontVehicleClassId = Convert.ToInt32(SecondChilddt.Rows[0]["NFP_VEHICLE_CLASS_ID_FRONT"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(SecondChilddt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                        {
                                            if (ParentANPRRearEntryId == 0 && ChildAnprRearEntryId == 0)
                                            {
                                                ChildAnprRearEntryId = Convert.ToInt32(SecondChilddt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                ChildANPRRearVehicleClassId = Convert.ToInt32(SecondChilddt.Rows[0]["NFP_VEHICLE_CLASS_ID_REAR"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                ValidTransactionsProcess = false;
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(SecondChilddt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                        {
                                            if (SecondChilddt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                            {
                                                SecondChildBalanceAlreadyDeducated = true;
                                            }
                                        }
                                        if (SecondChilddt.Rows[0]["AUDIT_STATUS"].ToString() == "1")
                                        {
                                            SecondChildAuditied = true;
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #endregion

                                if (ValidTransactionsProcess && (ParentPacketCount + ChildPacketCount) <= 3)
                                {
                                    if (ParentBalanceAlreadyDeducated || FirstChildBalanceAlreadyDeducated || SecondChildBalanceAlreadyDeducated)
                                    {
                                        #region Balance Already Deducated Only Need mearge
                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                        objtransaction.TransactionId = ParentTransactionId;
                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "Reviewed Success! Balance already deducted, Transactions is merged.";
                                        objResponseMessage.Add(objModelState);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Deduct the balance
                                        #region Check for voilation
                                        if (ParentIKEVehicleClassId > 0)
                                        {
                                            if (ParentIKEVehicleClassId != AuditedVehicleClassId)
                                            {
                                                ParentViolation = true;
                                            }

                                        }
                                        if (ChildIKEVehicleClassId > 0)
                                        {
                                            if (ChildIKEVehicleClassId != AuditedVehicleClassId)
                                            {
                                                ChildViolation = true;
                                            }

                                        }
                                        if (!ParentViolation && !ChildViolation)
                                        {
                                            if (CustomerVehicleDetails.VehicleClassId != AuditedVehicleClassId)
                                            {
                                                ParentViolation = true;
                                            }
                                        }
                                        #endregion
                                        if (ParentViolation || ChildViolation)
                                        {
                                            #region Violation Occured
                                            #region Status for Parent Only
                                            objtransaction.TransactionId = ParentTransactionId;
                                            if (ParentIKEVehicleClassId > ChildIKEVehicleClassId && ParentIKEVehicleClassId > AuditedVehicleClassId)
                                            {
                                                objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;
                                            }
                                            else if (ChildIKEVehicleClassId > ParentIKEVehicleClassId && ChildIKEVehicleClassId > AuditedVehicleClassId)
                                            {
                                                objtransaction.AuditedVehicleClassId = ChildIKEVehicleClassId;
                                            }
                                            else
                                            {
                                                objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                            }


                                            FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                            TransactionBLL.MarkAsViolation(objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            #endregion

                                            #region Status for Child Only
                                            objtransaction.TransactionId = FirstChildTranasactionId;
                                            TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                            //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                            //TransactionBLL.UpdateAuditSection(objtransaction);

                                            objtransaction.TransactionId = SecondChildTranasactionId;
                                            TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                            //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                            //TransactionBLL.UpdateAuditSection(objtransaction);
                                            #endregion

                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Reviewed Success! Balance deducted, Transactions is marked as violation.";
                                            objResponseMessage.Add(objModelState);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Only Mearged and Charged --Done
                                            #region Status for Parent Only
                                            objtransaction.TransactionId = ParentTransactionId;
                                            if (ParentIKEVehicleClassId > ChildIKEVehicleClassId && ParentIKEVehicleClassId > AuditedVehicleClassId)
                                            {
                                                objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;
                                            }
                                            else if (ChildIKEVehicleClassId > ParentIKEVehicleClassId && ChildIKEVehicleClassId > AuditedVehicleClassId)
                                            {
                                                objtransaction.AuditedVehicleClassId = ChildIKEVehicleClassId;
                                            }
                                            else
                                            {
                                                objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                            }

                                            FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Charged;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            #endregion

                                            #region Status for Child Only
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                            objtransaction.TransactionId = FirstChildTranasactionId;
                                            TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                            //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                            //TransactionBLL.UpdateAuditSection(objtransaction);

                                            objtransaction.TransactionId = SecondChildTranasactionId;
                                            TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                            //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                            //TransactionBLL.UpdateAuditSection(objtransaction);
                                            #endregion

                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Transactions ID " + TransactionId + " is successfully charged !!! /n Transactions ID " + FirstChildTranasactionId + " successfully merged  to Transactions ID " + TransactionId + "!!!";
                                            objResponseMessage.Add(objModelState);
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Please select valid associate transactions!";
                                    objResponseMessage.Add(objModelState);
                                }
                            }
                            else
                            {
                                if (ParentIKEEntryId > 0 && ParentANPRFrontEntryId > 0 && ParentANPRRearEntryId > 0)
                                {
                                    if (!ParentBalanceAlreadyDeducated)
                                    {
                                        #region Balance Deduct
                                        if (ParentIKEVehicleClassId > 0)
                                        {
                                            if (ParentIKEVehicleClassId > AuditedVehicleClassId)
                                            {
                                                ParentViolation = true;
                                                objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;

                                            }
                                            else
                                                objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;

                                        }

                                        objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                        if (!ParentViolation)
                                        {
                                            if (CustomerVehicleDetails.VehicleClassId != AuditedVehicleClassId)
                                            {
                                                ParentViolation = true;
                                            }
                                        }
                                        if (ParentViolation)
                                        {
                                            TransactionBLL.MarkAsViolation(objtransaction);
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Reviewed Success! Balance deducted, Transactions is marked as violation.";
                                            objResponseMessage.Add(objModelState);
                                        }
                                        else
                                        {
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Charged;
                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Reviewed Success! Balance deducted.";
                                            objResponseMessage.Add(objModelState);
                                        }
                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                        #endregion
                                    }
                                    else
                                    {
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "Reviewed failed! Unable to process this Transactions.";
                                        objResponseMessage.Add(objModelState);

                                    }

                                }
                                else
                                {
                                    string strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + objtransaction.TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + objtransaction.TransactionDateTime.AddMinutes(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID <> " + TransactionId + " AND ( T.AUDITED_VRN='" + AuditedVRN + "' OR CTP.PLATE_NUMBER='" + AuditedVRN + "')";
                                    DataTable Assodt = TransactionBLL.GetDataTableFilteredRecords(strfilter);
                                    if (Assodt.Rows.Count > 0)
                                    {
                                        #region Looking for Associated
                                        ValidTransactionsProcess = false;
                                        if (ParentPacketCount == 1 && Assodt.Rows.Count <= 2)
                                        {
                                            ValidTransactionsProcess = true;
                                        }
                                        else if (ParentPacketCount == 2 && Assodt.Rows.Count == 1)
                                        {
                                            ValidTransactionsProcess = true;
                                        }

                                        if (ValidTransactionsProcess)
                                        {
                                            #region Mearged and Audit Transcation
                                            FirstChildBalanceAlreadyDeducated = false;
                                            SecondChildBalanceAlreadyDeducated = false;
                                            ChildPacketCount = 0;
                                            #region Get the Childs packets Details
                                            for (int i = 0; i < Assodt.Rows.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    #region Get First child Details
                                                    FirstChildTranasactionId = Convert.ToInt32(Assodt.Rows[0]["TRANSACTION_ID"].ToString());
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                                    {
                                                        if (ParentIKEEntryId == 0)
                                                        {
                                                            ChildIKEEntryId = Convert.ToInt32(Assodt.Rows[0]["CT_ENTRY_ID"]);
                                                            ChildIKEVehicleClassId = Convert.ToInt32(Assodt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                                    {
                                                        if (ParentANPRFrontEntryId == 0)
                                                        {
                                                            ChildAnprFrontEntryId = Convert.ToInt32(Assodt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                            ChildANPRFrontVehicleClassId = Convert.ToInt32(Assodt.Rows[0]["NFP_VEHICLE_CLASS_ID_FRONT"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                                    {
                                                        if (ParentANPRRearEntryId == 0)
                                                        {
                                                            ChildAnprRearEntryId = Convert.ToInt32(Assodt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                            ChildANPRRearVehicleClassId = Convert.ToInt32(Assodt.Rows[0]["NFP_VEHICLE_CLASS_ID_REAR"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Get Second Child Pakcet Details
                                                    SecondChildTranasactionId = Convert.ToInt32(Assodt.Rows[i]["TRANSACTION_ID"].ToString());
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[i]["CT_ENTRY_ID"].ToString()))
                                                    {
                                                        if (ParentIKEEntryId == 0 && ChildIKEEntryId == 0)
                                                        {
                                                            ChildIKEEntryId = Convert.ToInt32(Assodt.Rows[i]["CT_ENTRY_ID"]);
                                                            ChildIKEVehicleClassId = Convert.ToInt32(Assodt.Rows[i]["CTP_VEHICLE_CLASS_ID"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[i]["NF_ENTRY_ID_FRONT"].ToString()))
                                                    {
                                                        if (ParentANPRFrontEntryId == 0 && ChildAnprFrontEntryId == 0)
                                                        {
                                                            ChildAnprFrontEntryId = Convert.ToInt32(Assodt.Rows[i]["NF_ENTRY_ID_FRONT"]);
                                                            ChildANPRFrontVehicleClassId = Convert.ToInt32(Assodt.Rows[i]["NFP_VEHICLE_CLASS_ID_FRONT"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[i]["NF_ENTRY_ID_REAR"].ToString()))
                                                    {
                                                        if (ParentANPRRearEntryId == 0 && ChildAnprRearEntryId == 0)
                                                        {
                                                            ChildAnprRearEntryId = Convert.ToInt32(Assodt.Rows[i]["NF_ENTRY_ID_REAR"]);
                                                            ChildANPRRearVehicleClassId = Convert.ToInt32(Assodt.Rows[i]["NFP_VEHICLE_CLASS_ID_REAR"]);
                                                            ChildPacketCount++;
                                                        }
                                                        else
                                                        {
                                                            ValidTransactionsProcess = false;
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(Assodt.Rows[i]["IS_BALANCE_UPDATED"].ToString()))
                                                    {
                                                        if (Assodt.Rows[i]["IS_BALANCE_UPDATED"].ToString() == "1")
                                                        {
                                                            SecondChildBalanceAlreadyDeducated = true;
                                                        }
                                                    }
                                                    if (Assodt.Rows[i]["AUDIT_STATUS"].ToString() == "1")
                                                    {
                                                        SecondChildAuditied = true;
                                                    }
                                                    #endregion
                                                }
                                            }
                                            #endregion

                                            if (ValidTransactionsProcess)
                                            {
                                                if (ParentBalanceAlreadyDeducated || FirstChildBalanceAlreadyDeducated || SecondChildBalanceAlreadyDeducated)
                                                {
                                                    #region Balance Already Deducated Only Need mearge
                                                    TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                                    objtransaction.TransactionId = ParentTransactionId;
                                                    objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                    TransactionBLL.UpdateAuditSection(objtransaction);
                                                    ModelStateList objModelState = new ModelStateList();
                                                    objModelState.ErrorMessage = "Reviewed Success! Balance already deducted, Transactions is merged.";
                                                    objResponseMessage.Add(objModelState);
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Deduct the balance
                                                    #region Check for voilation
                                                    if (ParentIKEEntryId > 0)
                                                    {
                                                        if (ParentIKEVehicleClassId != AuditedVehicleClassId)
                                                        {
                                                            ParentViolation = true;
                                                        }

                                                    }
                                                    if (ChildIKEEntryId > 0)
                                                    {
                                                        if (ChildIKEVehicleClassId != AuditedVehicleClassId)
                                                        {
                                                            ChildViolation = true;
                                                        }

                                                    }
                                                    #endregion
                                                    if (!ParentViolation && !ChildViolation)
                                                    {
                                                        if (CustomerVehicleDetails.VehicleClassId != AuditedVehicleClassId)
                                                        {
                                                            ParentViolation = true;
                                                        }
                                                    }
                                                    if (ParentViolation || ChildViolation)
                                                    {
                                                        #region Violation Occured
                                                        #region Status for Parent Only
                                                        objtransaction.TransactionId = ParentTransactionId;
                                                        if (ParentIKEVehicleClassId > ChildIKEVehicleClassId && ParentIKEVehicleClassId > AuditedVehicleClassId)
                                                        {
                                                            objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;
                                                        }
                                                        else if (ChildIKEVehicleClassId > ParentIKEVehicleClassId && ChildIKEVehicleClassId > AuditedVehicleClassId)
                                                        {
                                                            objtransaction.AuditedVehicleClassId = ChildIKEVehicleClassId;
                                                        }
                                                        else
                                                        {
                                                            objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                                        }


                                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                                        TransactionBLL.MarkAsViolation(objtransaction);
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        #endregion

                                                        #region Status for Child Only
                                                        objtransaction.TransactionId = FirstChildTranasactionId;
                                                        TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                                        //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                                        //TransactionBLL.UpdateAuditSection(objtransaction);

                                                        objtransaction.TransactionId = SecondChildTranasactionId;
                                                        TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                                        //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Mearged;
                                                        //TransactionBLL.UpdateAuditSection(objtransaction);
                                                        #endregion

                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "Reviewed Success! Balance deducted, Transactions is marked as violation.";
                                                        objResponseMessage.Add(objModelState);
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Only Mearged and Charged
                                                        #region Status for Parent Only
                                                        objtransaction.TransactionId = ParentTransactionId;
                                                        if (ParentIKEVehicleClassId > ChildIKEVehicleClassId && ParentIKEVehicleClassId > AuditedVehicleClassId)
                                                        {
                                                            objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;
                                                        }
                                                        else if (ChildIKEVehicleClassId > ParentIKEVehicleClassId && ChildIKEVehicleClassId > AuditedVehicleClassId)
                                                        {
                                                            objtransaction.AuditedVehicleClassId = ChildIKEVehicleClassId;
                                                        }
                                                        else
                                                        {
                                                            objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                                        }

                                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditedVehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Merged);
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Charged;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        #endregion

                                                        #region Status for Child Only
                                                        objtransaction.TransactionId = FirstChildTranasactionId;
                                                        TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                                        //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                        //TransactionBLL.UpdateAuditSection(objtransaction);

                                                        objtransaction.TransactionId = SecondChildTranasactionId;
                                                        TransactionBLL.MarkAsBalanceUpdated(objtransaction);
                                                        //objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                        //TransactionBLL.UpdateAuditSection(objtransaction);
                                                        #endregion

                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "Reviewed Success! Balance deducted.";
                                                        objResponseMessage.Add(objModelState);
                                                        #endregion
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                ModelStateList objModelState = new ModelStateList();
                                                objModelState.ErrorMessage = "Reviewed failed! Unable to process this Transactions.";
                                                objResponseMessage.Add(objModelState);

                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Reviewed failed! Unable to process this Transactions.";
                                            objResponseMessage.Add(objModelState);

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region No Associated Found

                                        if (!ParentBalanceAlreadyDeducated)
                                        {
                                            #region Balance Deduct
                                            if (ParentIKEVehicleClassId > 0)
                                            {
                                                if (ParentIKEVehicleClassId > AuditedVehicleClassId)
                                                {
                                                    ParentViolation = true;
                                                    objtransaction.AuditedVehicleClassId = ParentIKEVehicleClassId;

                                                }
                                                else
                                                    objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;

                                            }

                                            objtransaction.AuditedVehicleClassId = AuditedVehicleClassId;
                                            FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, objtransaction.AuditedVehicleClassId, objtransaction, ParentTransactionId);
                                            if (!ParentViolation)
                                            {
                                                if (CustomerVehicleDetails.VehicleClassId != AuditedVehicleClassId)
                                                {
                                                    ParentViolation = true;
                                                }
                                            }
                                            if (ParentViolation)
                                            {
                                                TransactionBLL.MarkAsViolation(objtransaction);
                                                objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                                ModelStateList objModelState = new ModelStateList();
                                                objModelState.ErrorMessage = "Reviewed Success! Balance deducted, Transactions is marked as violation.";
                                                objResponseMessage.Add(objModelState);
                                            }
                                            else
                                            {
                                                objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Charged;
                                                ModelStateList objModelState = new ModelStateList();
                                                objModelState.ErrorMessage = "Transactions ID " + TransactionId + " is successfully charged !!!";
                                                objResponseMessage.Add(objModelState);
                                            }
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            #region VRN Not Exists Mark as Unregistred Violance and Auditied
                            TransactionBLL.MarkAsUnregistred(objtransaction);
                            TransactionBLL.MarkAsViolation(objtransaction);
                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;


                            TransactionBLL.UpdateAuditSection(objtransaction);

                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Reviewed success! VRN not exists it marekd as violance.";
                            objResponseMessage.Add(objModelState);
                            //objModelState.ErrorMessage = "Balnce cannot deducated due to vrn not exists. It marked as violance.";
                            //objResponseMessage.Add(objModelState);
                            //objModelState.ErrorMessage = "VRN not exists it marekd as violance.";
                            //objResponseMessage.Add(objModelState);

                            #endregion
                        }

                    }
                    else
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Transaction not found please check for another.";
                        objResponseMessage.Add(objModelState);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    HelperClass.LogMessage("Failed to review transaction" + ex);
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Something went wrong";
                    objResponseMessage.Add(objModelState);
                }

            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Reviewed
        public ActionResult Reviewed()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Reviewed");
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

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

            vehicleClass.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
            {
                vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
            }

            ViewBag.VehicleClass = vehicleClass;

            #endregion

            #region Reviewer Dropdown
            List<SelectListItem> ReviewerList = new List<SelectListItem>();
            List<UserCBE> users = UserBLL.GetUserAll().Cast<UserCBE>().ToList();
            ReviewerList.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
            foreach (UserCBE u in users)
            {
                ReviewerList.Add(new SelectListItem() { Text = u.FirstName, Value = System.Convert.ToString(u.UserId) });
            }

            ViewBag.ReviewerList = ReviewerList;

            #endregion

            #region Reviewer Status
            ViewBag.ReviewerStatus = HelperClass.GetReviewerStatus();
            #endregion

            return View();
        }

        [HttpPost]
        public string ReviewedListScroll(int pageindex, int pagesize)
        {
            string Det = "";
            try
            {
                JsonResult result = new JsonResult();
                dt = TransactionBLL.GetReviewedDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Det;
        }

        [HttpPost]
        public string ReviewedFilter(ViewTransactionCBE transaction)
        {
            JsonResult result = new JsonResult();
            string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strQuery = " WHERE 1=1 ";
            if (strstarttime != null && strendtime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME BETWEEN TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strstarttime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME >= TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strendtime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME <= TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            if (!string.IsNullOrEmpty(transaction.ParentTranscationId))
            {
                strQuery += " AND (T.MEARGED_TRAN_ID LIKE '%" + transaction.ParentTranscationId + ")";
            }

            if (!string.IsNullOrEmpty(transaction.PlateNumber))
            {
                strQuery += " AND (LOWER(T.AUDITED_VRN) LIKE '%" + transaction.PlateNumber.ToLower() + "%' )";
            }
            if (transaction.VehicleClassId > 0)
            {
                strQuery += " AND (T.AUDITED_VEHICLE_CLASS_ID  = " + transaction.VehicleClassId + " )";
            }
            if (transaction.GantryId > 0)
            {
                strQuery += " AND (T.PLAZA_ID = " + transaction.GantryId + ")";
            }
            if (transaction.ReviewerId > 0)
            {
                strQuery += " AND (T.AUDITOR_ID = " + transaction.ReviewerId + ")";
            }
            if (transaction.ReviewerStatus > 0)
            {
                strQuery += " AND (T.TRANS_STATUS = " + transaction.ReviewerStatus + ")";
            }
            dt = TransactionBLL.GetReviewedDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion

        #region Charged
        public ActionResult Charged()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Charged");

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

            vehicleClass.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
            {
                vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
            }

            ViewBag.VehicleClass = vehicleClass;

            #endregion

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
            return View();
        }

        [HttpPost]
        public string ChargedListScroll(int pageindex, int pagesize)
        {
            string Det = "";
            try
            {
                JsonResult result = new JsonResult();
                dt = TransactionBLL.GetChargedDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Det;
        }

        [HttpPost]
        public string ChargedFilter(ViewTransactionCBE transaction)
        {
            JsonResult result = new JsonResult();
            string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strQuery = " WHERE 1=1 ";
            if (strstarttime != null && strendtime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME BETWEEN TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strstarttime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME >= TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strendtime != null)
            {
                strQuery += " AND  T.TRANSACTION_DATETIME <= TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            if (!string.IsNullOrEmpty(transaction.ResidentId))
            {
                strQuery += " AND (LOWER(CA_CT.RESIDENT_ID) LIKE '%" + transaction.ResidentId.ToLower() + "%'  OR LOWER(CA_NFPF.RESIDENT_ID) LIKE '%" + transaction.ResidentId.ToLower() + "%' OR LOWER(CA_NFPR.RESIDENT_ID) LIKE '%" + transaction.ResidentId.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.Name))
            {
                strQuery += " AND (LOWER(CA_CT.FIRST_NAME) LIKE '%" + transaction.Name.ToLower() + "%' OR LOWER(CA_NFPF.FIRST_NAME) LIKE '%" + transaction.Name.ToLower() + "%' OR LOWER(CA_NFPR.FIRST_NAME) LIKE '%" + transaction.Name.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.Email))
            {
                strQuery += " AND (LOWER(CA_CT.EMAIL_ID) LIKE '%" + transaction.Email.ToLower() + "%' OR LOWER(CA_NFPF.EMAIL_ID) LIKE '%" + transaction.Email.ToLower() + "%' OR LOWER(CA_NFPR.EMAIL_ID) LIKE '%" + transaction.Email.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.PlateNumber))
            {
                strQuery += " AND (LOWER(CTP.PLATE_NUMBER) LIKE '%" + transaction.PlateNumber.ToLower() + "%' OR LOWER(NFPF.PLATE_NUMBER) LIKE '%" + transaction.PlateNumber.ToLower() + "%' OR LOWER(NFPR.PLATE_NUMBER) LIKE '%" + transaction.PlateNumber.ToLower() + "%')";
            }
            if (transaction.VehicleClassId > 0)
            {
                strQuery += " AND (CTP.VEHICLE_CLASS_ID  = " + transaction.VehicleClassId + " OR NFPF.VEHICLE_CLASS_ID  = " + transaction.VehicleClassId + " OR NFPR.VEHICLE_CLASS_ID  = " + transaction.VehicleClassId + ")";
            }
            if (transaction.GantryId > 0)
            {
                strQuery += " AND (T.PLAZA_ID = " + transaction.GantryId + ")";
            }
            dt = TransactionBLL.GetChargedDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion

        #region Top Up
        public ActionResult TopUP()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "TopUP");
            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

            vehicleClass.Add(new SelectListItem() { Text = "--Select All--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
            {
                vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
            }

            ViewBag.VehicleClass = vehicleClass;

            #endregion
            return View();
        }

        [HttpPost]
        public string TopUpListScroll(int pageindex, int pagesize)
        {
            string Det = "";
            try
            {
                JsonResult result = new JsonResult();
                dt = AccountHistoryBLL.GetTopUpDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Det;
        }

        [HttpPost]
        public string TopUpFilter(ViewTransactionCBE transaction)
        {
            JsonResult result = new JsonResult();
            string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strQuery = " WHERE 1=1 ";
            if (strstarttime != null && strendtime != null)
            {
                strQuery += " AND  T.CREATION_DATE BETWEEN TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strstarttime != null)
            {
                strQuery += " AND  T.CREATION_DATE >= TO_DATE('" + strstarttime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            else if (strendtime != null)
            {
                strQuery += " AND  T.CREATION_DATE <= TO_DATE('" + strendtime + "','DD/MM/YYYY HH24:MI:SS')";
            }
            if (!string.IsNullOrEmpty(transaction.ResidentId))
            {
                strQuery += " AND (LOWER(CA.RESIDENT_ID) LIKE '%" + transaction.ResidentId.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.Name))
            {
                strQuery += " AND (LOWER(CA.FIRST_NAME) LIKE '%" + transaction.Name.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.Email))
            {
                strQuery += " AND (LOWER(CA.EMAIL_ID) LIKE '%" + transaction.Email.ToLower() + "%')";
            }
            if (!string.IsNullOrEmpty(transaction.PlateNumber))
            {
                strQuery += " AND (LOWER(CV.VEH_REG_NO) LIKE '%" + transaction.PlateNumber.ToLower() + "%')";
            }
            if (transaction.VehicleClassId > 0)
            {
                strQuery += " AND (CV.VEHICLE_CLASS_ID = " + transaction.VehicleClassId + ")";
            }
            dt = AccountHistoryBLL.GetTopUpDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion

        #region Helper Methord
        private static void MarkAsViolation(Int32 TranscationId, TransactionCBE transaction)
        {
            transaction.TransactionId = TranscationId;
            TransactionBLL.MarkAsViolation(transaction);
        }

        private void FinancialProcessing(CustomerVehicleCBE customerVehicleInfo, CustomerAccountCBE customerAccountInfo, Int32 ChargebleVehicleClassId, TransactionCBE transaction, int TranscationId)
        {

            transaction.TransactionId = TranscationId;
            #region LaneType and TollRate Section
            decimal tollToDeduct = -1; //if no toll rate found it will be returned as -1
            try
            {
                int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                tollToDeduct = GetTollRate(Constants.GetCurrentTMSId(), laneTypeId, transaction.TransactionDateTime, ChargebleVehicleClassId);

            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding out lane type and toll to deduct. " + ex.ToString());
                tollToDeduct = -1;
            }
            #endregion

            if (tollToDeduct > -1)
            {
                Decimal currentAccountBalance = customerVehicleInfo.AccountBalance;
                Decimal afterDeduction = currentAccountBalance - tollToDeduct;
                #region Account History Section
                try
                {
                    AccountHistoryCBE accountHistory = new AccountHistoryCBE();
                    accountHistory.TMSId = Constants.GetCurrentTMSId();
                    accountHistory.AccountId = customerAccountInfo.AccountId;
                    accountHistory.CustomerVehicleEntryId = customerVehicleInfo.EntryId; //<============================= 
                    accountHistory.TransactionTypeId = (int)Constants.TransactionType.LaneDebit;
                    accountHistory.TransactionId = TranscationId;
                    accountHistory.Amount = tollToDeduct;
                    accountHistory.IsSMSSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent; //will be updated later on
                    accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent;//will be updated later on//accountHistory.ModifierId = 1;//will be updated later on
                    accountHistory.CreationDate = DateTime.Now;
                    accountHistory.ModificationDate = DateTime.Now;
                    accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                    accountHistory.OpeningBalance = currentAccountBalance;
                    accountHistory.ClosingBalance = afterDeduction;
                    AccountHistoryBLL.Insert(accountHistory);
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in recording in the Account History table. " + ex.ToString());
                }
                #endregion

                #region Update Balance Section
                try
                {
                    customerVehicleInfo.AccountBalance = CustomerVehicleBLL.UpdateVehiclebalance(customerVehicleInfo, (-1 * tollToDeduct));
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in updating customer's account balance. " + ex.ToString());
                }
                #endregion

                #region Mark transaction as balance updated
                try
                {
                    TransactionBLL.MarkAsBalanceUpdated(transaction);
                    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, afterDeduction);
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in marking the transaction as balance updated. " + ex.ToString());
                }
                #endregion
            }

        }

        private void FinancialProcessingWithoutNotification(CustomerVehicleCBE customerVehicleInfo, CustomerAccountCBE customerAccountInfo, Int32 ChargebleVehicleClassId, TransactionCBE transaction, int TranscationId)
        {
            transaction.TransactionId = TranscationId;
            TransactionBLL.MarkAsBalanceUpdated(transaction);
        }

        private void NotificationProcessing(CustomerVehicleCBE customerVehicleInfo, CustomerAccountCBE customerAccountInfo, TransactionCBE transaction, Decimal tollToDeduct, Decimal AfterDeduction)
        {
            try
            {
                smsFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration.Deserialize();
                System.Messaging.Message smsMessage = new System.Messaging.Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                CultureInfo culture = new CultureInfo("id-ID");
                string RechareDate = transaction.TransactionDateTime.AddDays(4).ToString("dd-MMM-yyyy") + " 23:59:59";
                if (AfterDeduction > 0)
                {
                    string AFTERDEDUCTION = smsFileConfig.AFTERDEDUCTION;
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", ""));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[balance]", Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture).Replace("Rp", ""));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("tid", transaction.TransactionId.ToString());
                    if (AFTERDEDUCTION.Length > 160)
                    {
                        AFTERDEDUCTION = AFTERDEDUCTION.Substring(0, 159);
                    }
                    smsDetail.SMSMessage = AFTERDEDUCTION;
                }
                else
                {
                    string NOTIFICATION = smsFileConfig.NOTIFICATION;
                    NOTIFICATION = NOTIFICATION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", ""));
                    NOTIFICATION = NOTIFICATION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    NOTIFICATION = NOTIFICATION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    NOTIFICATION = NOTIFICATION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    NOTIFICATION = NOTIFICATION.Replace("[recharedate]", RechareDate);
                    NOTIFICATION = NOTIFICATION.Replace("[balance]", Decimal.Parse((AfterDeduction + tollToDeduct).ToString()).ToString("C", culture).Replace("Rp", ""));
                    NOTIFICATION = NOTIFICATION.Replace("tid", transaction.TransactionId.ToString());
                    if (NOTIFICATION.Length > 160)
                    {
                        NOTIFICATION = NOTIFICATION.Substring(0, 159);
                    }
                    smsDetail.SMSMessage = NOTIFICATION;
                }

                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;

                smsMessage.Body = smsDetail;

                smsMessageQueue.Send(smsMessage);
            }
            catch (Exception ex)
            {
                LogMessage("Exception in pushing SMS to SMS MSMQ. " + ex.ToString());
            }
        }

        private int GetLaneTypeByLaneId(int laneId)
        {
            int result = -1;

            try
            {
                lanes = LaneBLL.GetAll();
                foreach (LaneCBE lane in lanes)
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
        private decimal GetTollRate(int plazaId, int laneType, DateTime transactionTime, int vehicleClass)
        {
            decimal result = -1;

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection currentTimeTollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();
                tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
                currentTimeTollRates = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetTollRateCollection(transactionTime, tollRates);

                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in currentTimeTollRates)
                {
                    if (tr.PlazaId == plazaId && tr.LaneTypeId == laneType && tr.VehicleClassId == vehicleClass)
                    {
                        result = tr.Amount;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to get toll rate. " + ex.Message);
                result = -1;
            }

            return result;
        }
        private static bool CheckParentAsViolation(Int32 ParentPacketCount, Int32 ParentIKEEntryId, Int32 ParentIKEVehicleClassId, Int32 ParentANPRFrontEntryId, Int32 ParentANPRFrontVehicleClassId, int ParentANPRRearEntryId, Int32 ParentANPRRearVehicleClassId, Int32 AuditvehicleClassId, out Int32 VehicleClassForBalanceDecucation)
        {
            bool isViolation = false;
            VehicleClassForBalanceDecucation = AuditvehicleClassId;
            #region Check parent Transcation for volication
            if (ParentPacketCount == 3)
            {
                if (ParentIKEVehicleClassId != AuditvehicleClassId || ParentIKEVehicleClassId != ParentANPRFrontVehicleClassId || ParentIKEVehicleClassId != ParentANPRRearVehicleClassId)
                {
                    isViolation = true;
                    VehicleClassForBalanceDecucation = (ParentIKEVehicleClassId > AuditvehicleClassId) ? ParentIKEVehicleClassId : AuditvehicleClassId;
                }
            }
            else if (ParentPacketCount == 2)
            {
                #region check volication according to vehicle class 
                if (ParentIKEEntryId > 0)
                {
                    if (ParentANPRFrontEntryId > 0)
                    {
                        if (ParentIKEVehicleClassId != AuditvehicleClassId || ParentIKEVehicleClassId != ParentANPRFrontVehicleClassId)
                        {
                            isViolation = true;
                            VehicleClassForBalanceDecucation = (ParentIKEVehicleClassId > AuditvehicleClassId) ? ParentIKEVehicleClassId : AuditvehicleClassId;
                        }
                    }
                    else
                    {
                        if (ParentIKEVehicleClassId != AuditvehicleClassId || ParentIKEVehicleClassId != ParentANPRRearVehicleClassId)
                        {
                            isViolation = true;
                            VehicleClassForBalanceDecucation = (ParentIKEVehicleClassId > AuditvehicleClassId) ? ParentIKEVehicleClassId : AuditvehicleClassId;
                        }
                    }
                }
                else
                {
                    if (ParentANPRFrontVehicleClassId != AuditvehicleClassId || ParentANPRRearVehicleClassId != AuditvehicleClassId || ParentANPRFrontVehicleClassId != ParentANPRRearVehicleClassId)
                    {
                        isViolation = true;
                        VehicleClassForBalanceDecucation = AuditvehicleClassId;
                    }
                }
                #endregion
            }
            else
            {
                #region check volication according to vehicle class 
                if (ParentIKEEntryId > 0)
                {
                    if (ParentIKEVehicleClassId != AuditvehicleClassId)
                    {
                        isViolation = true;
                        VehicleClassForBalanceDecucation = (ParentIKEVehicleClassId > AuditvehicleClassId) ? ParentIKEVehicleClassId : AuditvehicleClassId;
                    }
                }
                else if (ParentANPRFrontEntryId > 0)
                {
                    if (ParentANPRFrontVehicleClassId != AuditvehicleClassId)
                    {
                        isViolation = true;
                        VehicleClassForBalanceDecucation = AuditvehicleClassId;
                    }
                }
                else if (ParentANPRRearEntryId > 0)
                {
                    if (ParentANPRRearVehicleClassId != AuditvehicleClassId)
                    {
                        isViolation = true;
                        VehicleClassForBalanceDecucation = AuditvehicleClassId;
                    }
                }
                #endregion
            }
            #endregion
            return isViolation;
        }

        #endregion

        #region OLD Display
        public ActionResult Violation()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Transaction", "Violation");
            return View();
        }

        [HttpPost]
        public string ViolationListScroll(int pageindex, int pagesize)
        {
            string Det = "";
            try
            {
                JsonResult result = new JsonResult();
                dt = TransactionBLL.GetVIOLATIONDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Det;
        }

        public ActionResult Unidentified()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Transaction", "Unidentified");
            return View();
        }

        [HttpPost]
        public string UnidentifiedListScroll(int pageindex, int pagesize)
        {
            string Det = "";
            try
            {
                JsonResult result = new JsonResult();
                dt = TransactionBLL.GetUnidentifiedDataTableFilteredRecordsLazyLoad(pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Det;
        }

        #endregion



    }
}