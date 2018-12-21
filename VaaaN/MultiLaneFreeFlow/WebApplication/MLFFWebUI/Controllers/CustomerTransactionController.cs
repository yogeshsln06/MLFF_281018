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

namespace MLFFWebUI.Controllers
{

    public class CustomerTransactionController : Controller
    {
        LaneCollection lanes;
        TollRateCollection tollRates;
        PlazaCollection plazas;
        DataTable dt = new DataTable();
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration smsFileConfig;
        private MessageQueue smsMessageQueue;

        // GET: CustomerTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unreviewed()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Unreviewed");
            //AND TRANSACTION_DATETIME BETWEEN TO_DATE('" + DateTime.Now.AddMinutes(-30) + "', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + DateTime.Now + "', 'DD/MM/YYYY HH24:MI:SS')
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strQuery);
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

            return View("Unreviewed", dt);
        }

        public ActionResult AssociatedTransaction(int TranscationId)
        {
            DataSet ds = new DataSet();

            try
            {
                int IKEEntryId = 0;
                int ANPRFrontEntryId = 0;
                int ANPRRearEntryId = 0;
                DateTime TranscationDateTime;
                dt = TransactionBLL.GetUnReviewedDataTableById(TranscationId);
                DataTable dtCurrentTranscation = new DataTable();
                dtCurrentTranscation = dt.Copy();
                ds.Tables.Add(dtCurrentTranscation);
                ds.Tables[0].TableName = "CurrentTranscation";
                if (dtCurrentTranscation.Rows.Count > 0)
                {
                    TranscationDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"]);
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
                        DataTable Assodt = new DataTable();
                        DataTable dtAssociatedTranscation = new DataTable();
                        dtAssociatedTranscation = Assodt.Copy();
                        ds.Tables.Add(dtAssociatedTranscation);
                    }
                    else {
                        string strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + TranscationDateTime.AddMinutes(-1).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + TranscationDateTime.AddMinutes(1).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID <> " + TranscationId;

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
                        DataTable dtAssociatedTranscation = new DataTable();
                        dtAssociatedTranscation = Assodt.Clone();
                        ds.Tables.Add(dtAssociatedTranscation);
                    }

                }
                else {
                    DataTable Assodt = new DataTable();
                    DataTable dtAssociatedTranscation = new DataTable();
                    dtAssociatedTranscation = Assodt.Copy();
                    ds.Tables.Add(dtAssociatedTranscation);
                }

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Associated Transaction finding Controller" + ex);
            }

            return PartialView("AssociatedTransaction", ds);
        }

        [HttpPost]
        public JsonResult SaveUnidentified(int TranscationId)
        {
            JsonResult result = new JsonResult();
            try
            {
                dt = TransactionBLL.GetUnReviewedDataTableById(TranscationId);
                if (dt.Rows.Count > 0)
                {
                    TransactionBLL.JoinAuditTransaction(TranscationId, 0, 0, "", 0, Convert.ToInt32(Session["LoggedUserId"].ToString()));
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "success";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "This transcation already reviewed.";
                    objResponseMessage.Add(objModelState);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Save Unidentified Transcation in Customer Transaction Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }

            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompleteReviewed(string[] AssociatedTransactionIds, int TransactionId, string VehRegNo, int vehicleClassID)
        {
            JsonResult result = new JsonResult();

            Int32 AuditvehicleClassId = Convert.ToInt32(vehicleClassID);
            Int32 ParentTransactionId = TransactionId;
            Int32 ParentIKEVehicleClassId = 0;
            Int32 ParentANPRFrontVehicleClassId = 0;
            Int32 ParentANPRRearVehicleClassId = 0;
            Int32 ParentIKEEntryId = 0;
            Int32 ParentANPRFrontEntryId = 0;
            Int32 ParentANPRRearEntryId = 0;
            Int32 ParentPacketCount = 0;
            bool ParentBalanceAlreadyUpdated = false;
            bool ParentAuditied = false;

            Int32 childOneTranasactionId = 0;
            Int32 childTwoTranasactionId = 0;

            Int32 ChildIKEEntryId = 0;
            Int32 ChildAnprFrontEntryId = 0;
            Int32 ChildAnprRearEntryId = 0;
            Int32 ChildIKEVehicleClassId = 0;
            Int32 ChildANPRFrontVehicleClassId = 0;
            Int32 ChildANPRRearVehicleClassId = 0;
            Int32 ChildPacketCount = 0;
            bool isChildViolation = false;

            bool VRNisExists = false;
            bool isChildBalanceUpdated = false;
            bool isViolation = false;

            bool Process = true;
            Int32 VehicleClassForBalanceDecucation = vehicleClassID;
            Int32 ChildVehicleClassForBalanceDecucation = vehicleClassID;

            Int32 AssociatedTransactionCount = 0;
            TransactionCBE objtransaction = new TransactionCBE();
            objtransaction.TMSId = Constants.GetCurrentTMSId();
            objtransaction.PlazaId = Constants.GetCurrentPlazaId();
            objtransaction.ModifierId = Convert.ToInt32(Session["LoggedUserId"].ToString());
            objtransaction.ModificationDate = DateTime.Now;
            try
            {
                #region Check Vrn is blank or not
                if (string.IsNullOrEmpty(VehRegNo))
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "VRN is required.";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    #region Customer Vehicle Info by VRN
                    CustomerVehicleCBE CustomerVehicleDetails = new CustomerVehicleCBE();
                    CustomerVehicleDetails.VehRegNo = VehRegNo;
                    CustomerVehicleDetails = CustomerVehicleBLL.GetCustomerVehicleByVehRegNo(CustomerVehicleDetails);
                    #endregion

                    #region Customer Inforamtion by VRN
                    CustomerAccountCBE customerAccountInfo = new CustomerAccountCBE();
                    customerAccountInfo.AccountId = CustomerVehicleDetails.AccountId;
                    customerAccountInfo.TmsId = 1;
                    customerAccountInfo = CustomerAccountBLL.GetCustomerById(customerAccountInfo);
                    #endregion

                    dt = TransactionBLL.GetDataTableFilteredRecordById(TransactionId);

                    #region Check auditied VRN Exists or not
                    if (!string.IsNullOrEmpty(CustomerVehicleDetails.VehRegNo))
                    {
                        VRNisExists = true;
                    }
                    #endregion

                    #region Check Associated TransactionIds is avaliable or not 
                    if (AssociatedTransactionIds != null)
                    {
                        if (AssociatedTransactionIds.Length > 0)
                        {
                            AssociatedTransactionCount++;
                            childOneTranasactionId = Convert.ToInt32(AssociatedTransactionIds[0]);
                        }
                        if (AssociatedTransactionIds.Length > 1)
                        {
                            AssociatedTransactionCount++;
                            childTwoTranasactionId = Convert.ToInt32(AssociatedTransactionIds[1]);
                        }
                    }
                    #endregion

                    if (VRNisExists)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            objtransaction.TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"].ToString());
                            objtransaction.LaneId = Convert.ToInt32(dt.Rows[0]["LANE_ID"].ToString());

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
                                ParentANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["NFP_VEHICLE_CLASS_NAME_REAR"]);
                            }
                            #endregion

                            #region check Balance already deduct or not for parent
                            if (dt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                            {
                                ParentBalanceAlreadyUpdated = true;
                            }
                            if (dt.Rows[0]["AUDIT_STATUS"].ToString() == "1")
                            {
                                ParentAuditied = true;
                            }
                            #endregion

                            #region Procees for already auditied or not
                            if (!ParentAuditied)
                            {
                                if (ParentPacketCount == 3) // Parent Paket is full
                                {
                                    #region Transcation Process for Audit
                                    if (!ParentBalanceAlreadyUpdated)
                                    {
                                        isViolation = CheckParentAsViolation(ParentPacketCount, ParentIKEEntryId, ParentIKEVehicleClassId, ParentANPRFrontEntryId, ParentANPRFrontVehicleClassId, ParentANPRRearEntryId, ParentANPRRearVehicleClassId, AuditvehicleClassId, out VehicleClassForBalanceDecucation);

                                        if (isViolation)
                                        {
                                            MarkAsViolation(ParentTransactionId, objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationCharged);
                                        }
                                        else
                                        {
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Charged);
                                        }
                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "success-Yes Mearge-No";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                    }
                                    else
                                    {
                                        isViolation = CheckParentAsViolation(ParentPacketCount, ParentIKEEntryId, ParentIKEVehicleClassId, ParentANPRFrontEntryId, ParentANPRFrontVehicleClassId, ParentANPRRearEntryId, ParentANPRRearVehicleClassId, AuditvehicleClassId, out VehicleClassForBalanceDecucation);
                                        if (isViolation)
                                        {
                                            MarkAsViolation(ParentTransactionId, objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Violation);
                                        }
                                        else
                                        {
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Unknown);
                                        }
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "success-Yes Mearge-No";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region validate Associated Records 
                                    if (AssociatedTransactionCount > 0)//Associated Transcation Found
                                    {
                                        #region Find 1st Associated Transcation releated pakcet id and is balance already deducated or not
                                        if (childOneTranasactionId > 0)
                                        {
                                            DataTable childOnedt = TransactionBLL.GetDataTableFilteredRecordById(childOneTranasactionId);
                                            if (childOnedt.Rows.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(childOnedt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                                {
                                                    if (ParentIKEEntryId != 0)
                                                    {
                                                        ChildIKEEntryId = Convert.ToInt32(childOnedt.Rows[0]["CT_ENTRY_ID"]);
                                                        ChildIKEVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                        ChildPacketCount++;
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(childOnedt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                                {
                                                    if (ParentANPRFrontEntryId != 0)
                                                    {
                                                        ChildAnprFrontEntryId = Convert.ToInt32(childOnedt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                        ChildANPRFrontVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                        ChildPacketCount++;
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(childOnedt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                                {
                                                    if (ParentANPRRearEntryId != 0)
                                                    {
                                                        ChildAnprRearEntryId = Convert.ToInt32(childOnedt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                        ChildANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                        ChildPacketCount++;
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }

                                                }
                                                if (!string.IsNullOrEmpty(childOnedt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                                {
                                                    if (childOnedt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                                    {
                                                        isChildBalanceUpdated = true;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (Process)
                                        {
                                            #region Find 2nd Associated Transcation reated pakcet id and is balance already deducated or not
                                            if (childTwoTranasactionId > 0)
                                            {
                                                DataTable childTwodt = TransactionBLL.GetDataTableFilteredRecordById(childTwoTranasactionId);
                                                if (childTwodt.Rows.Count > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(childTwodt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                                    {
                                                        if (ParentIKEEntryId != 0)
                                                        {
                                                            if (ChildIKEEntryId == 0)
                                                            {
                                                                ChildIKEEntryId = Convert.ToInt32(childTwodt.Rows[0]["CT_ENTRY_ID"]);
                                                                ChildIKEVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                            }
                                                            else
                                                            {
                                                                Process = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Process = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(childTwodt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                                    {
                                                        if (ParentANPRRearEntryId != 0)
                                                        {
                                                            if (ChildAnprFrontEntryId == 0)
                                                            {
                                                                ChildAnprFrontEntryId = Convert.ToInt32(childTwodt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                                ChildANPRFrontVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                            }
                                                            else
                                                            {
                                                                Process = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Process = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(childTwodt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                                    {
                                                        if (ParentANPRRearEntryId != 0)
                                                        {
                                                            if (ChildAnprRearEntryId == 0)
                                                            {
                                                                ChildAnprRearEntryId = Convert.ToInt32(childTwodt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                                ChildANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                            }
                                                            else
                                                            {
                                                                Process = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Process = false;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(childTwodt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                                    {
                                                        if (childTwodt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                                        {
                                                            isChildBalanceUpdated = true;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            if (Process)
                                            {
                                                isViolation = CheckParentAsViolation(ParentPacketCount, ParentIKEEntryId, ParentIKEVehicleClassId, ParentANPRFrontEntryId, ParentANPRFrontVehicleClassId, ParentANPRRearEntryId, ParentANPRRearVehicleClassId, AuditvehicleClassId, out VehicleClassForBalanceDecucation);
                                                #region check volication for associated records
                                                if (ChildPacketCount == 2)
                                                {
                                                    #region check child data for volication according to vehicle class 
                                                    if (ChildIKEEntryId > 0)
                                                    {
                                                        if (ChildAnprFrontEntryId > 0)
                                                        {
                                                            if (ChildIKEVehicleClassId != AuditvehicleClassId || ChildIKEVehicleClassId != ChildANPRFrontVehicleClassId)
                                                            {
                                                                isChildViolation = true;
                                                                ChildVehicleClassForBalanceDecucation = (ChildIKEVehicleClassId > AuditvehicleClassId) ? ChildIKEVehicleClassId : AuditvehicleClassId;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ChildIKEVehicleClassId != AuditvehicleClassId || ChildIKEVehicleClassId != ChildANPRRearVehicleClassId)
                                                            {
                                                                isChildViolation = true;
                                                                ChildVehicleClassForBalanceDecucation = (ChildIKEVehicleClassId > AuditvehicleClassId) ? ChildIKEVehicleClassId : AuditvehicleClassId;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ChildANPRFrontVehicleClassId != AuditvehicleClassId || ChildANPRRearVehicleClassId != AuditvehicleClassId || ChildANPRFrontVehicleClassId != ChildANPRRearVehicleClassId)
                                                        {
                                                            isChildViolation = true;
                                                            ChildVehicleClassForBalanceDecucation = AuditvehicleClassId;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region check volication according to vehicle class 
                                                    if (ChildIKEEntryId > 0)
                                                    {
                                                        if (ChildIKEVehicleClassId != AuditvehicleClassId)
                                                        {
                                                            isChildViolation = true;
                                                            ChildVehicleClassForBalanceDecucation = (ChildIKEVehicleClassId > AuditvehicleClassId) ? ChildIKEVehicleClassId : AuditvehicleClassId;
                                                        }
                                                    }
                                                    else if (ChildAnprFrontEntryId > 0)
                                                    {
                                                        if (ChildANPRFrontVehicleClassId != AuditvehicleClassId)
                                                        {
                                                            isChildViolation = true;
                                                            ChildVehicleClassForBalanceDecucation = AuditvehicleClassId;
                                                        }
                                                    }
                                                    else if (ChildAnprRearEntryId > 0)
                                                    {
                                                        if (ChildANPRRearVehicleClassId != AuditvehicleClassId)
                                                        {
                                                            isChildViolation = true;
                                                            ChildVehicleClassForBalanceDecucation = AuditvehicleClassId;
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                #endregion

                                                if (isViolation)
                                                {
                                                    MarkAsViolation(ParentTransactionId, objtransaction);
                                                    #region Process for Balance Deduct of not for parent
                                                    if (ParentBalanceAlreadyUpdated && isChildBalanceUpdated)
                                                    {
                                                        #region Balance already deducated 
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        if (isChildViolation)
                                                        {
                                                            MarkAsViolation(childOneTranasactionId, objtransaction);
                                                            MarkAsViolation(childTwoTranasactionId, objtransaction);
                                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        }
                                                        else
                                                        {
                                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                                        }



                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childOneTranasactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childTwoTranasactionId);

                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "success-Yes Mearge-Yes";
                                                        objResponseMessage.Add(objModelState);
                                                        Process = false;
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Deduct balance and send notification 
                                                        if (ChildVehicleClassForBalanceDecucation > VehicleClassForBalanceDecucation)
                                                            VehicleClassForBalanceDecucation = ChildVehicleClassForBalanceDecucation;
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "success-Yes Mearge-Yes";
                                                        objResponseMessage.Add(objModelState);
                                                        #endregion

                                                    }
                                                    Process = false;
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Process for Balance Deduct of not for parent
                                                    if (ParentBalanceAlreadyUpdated && isChildBalanceUpdated)
                                                    {
                                                        #region balance already deducated 
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        if (isChildViolation)
                                                        {
                                                            MarkAsViolation(childOneTranasactionId, objtransaction);
                                                            MarkAsViolation(childTwoTranasactionId, objtransaction);
                                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        }
                                                        else
                                                        {
                                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                                        }
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childOneTranasactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childTwoTranasactionId);

                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "success-Yes Mearge-Yes";
                                                        objResponseMessage.Add(objModelState);
                                                        Process = false;
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Deduct balance and marked and send notification
                                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                                        TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);

                                                        if (ChildVehicleClassForBalanceDecucation > VehicleClassForBalanceDecucation)
                                                            VehicleClassForBalanceDecucation = ChildVehicleClassForBalanceDecucation;
                                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childOneTranasactionId);
                                                        FinancialProcessingWithoutNotification(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, childTwoTranasactionId);

                                                        ModelStateList objModelState = new ModelStateList();
                                                        objModelState.ErrorMessage = "success-Yes Mearge-Yes";
                                                        objResponseMessage.Add(objModelState);
                                                        #endregion
                                                    }


                                                    Process = false;
                                                    #endregion
                                                }

                                            }
                                            else
                                            {
                                                #region Invalid Associtated Selected
                                                ModelStateList objModelState = new ModelStateList();
                                                objModelState.ErrorMessage = "Please Select valid associated transcation.";
                                                objResponseMessage.Add(objModelState);
                                                Process = false;
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            #region Invalid Associtated Selected
                                            ModelStateList objModelState = new ModelStateList();
                                            objModelState.ErrorMessage = "Please Select valid associated transcation.";
                                            objResponseMessage.Add(objModelState);
                                            Process = false;
                                            #endregion
                                        }

                                    }
                                    else {
                                        // need to work
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Transcation already Audited but looking for charged and mearged
                                if (ParentPacketCount == 3) // Parent Paket is full
                                {
                                    if (!ParentBalanceAlreadyUpdated)
                                    {
                                        #region Balance deduct and send notification
                                        isViolation = CheckParentAsViolation(ParentPacketCount, ParentIKEEntryId, ParentIKEVehicleClassId, ParentANPRFrontEntryId, ParentANPRFrontVehicleClassId, ParentANPRRearEntryId, ParentANPRRearVehicleClassId, AuditvehicleClassId, out VehicleClassForBalanceDecucation);
                                        if (isViolation)
                                        {
                                            MarkAsViolation(ParentTransactionId, objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationCharged);
                                        }
                                        else
                                        {
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Charged);
                                        }
                                        if (ChildVehicleClassForBalanceDecucation > VehicleClassForBalanceDecucation)
                                            VehicleClassForBalanceDecucation = ChildVehicleClassForBalanceDecucation;
                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, VehicleClassForBalanceDecucation, objtransaction, ParentTransactionId);
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "success-Yes Mearge-No";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Balance already deducated 
                                        isViolation = CheckParentAsViolation(ParentPacketCount, ParentIKEEntryId, ParentIKEVehicleClassId, ParentANPRFrontEntryId, ParentANPRFrontVehicleClassId, ParentANPRRearEntryId, ParentANPRRearVehicleClassId, AuditvehicleClassId, out VehicleClassForBalanceDecucation);
                                        if (isViolation)
                                        {
                                            MarkAsViolation(ParentTransactionId, objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.Violation);
                                        }
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "success-Yes Mearge-No";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    // need to work
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Customer account not exists balance cannot deduct
                        if (dt.Rows.Count > 0)
                        {
                            objtransaction.TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"].ToString());
                            objtransaction.LaneId = Convert.ToInt32(dt.Rows[0]["LANE_ID"].ToString());

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
                                ParentANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["NFP_VEHICLE_CLASS_NAME_REAR"]);
                            }
                            #endregion
                            if (AssociatedTransactionCount > 0)//Associated Transcation Found
                            {
                                #region Find 1st Associated Transcation releated pakcet id and is balance already deducated or not
                                if (childOneTranasactionId > 0)
                                {
                                    DataTable childOnedt = TransactionBLL.GetDataTableFilteredRecordById(childOneTranasactionId);
                                    if (childOnedt.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(childOnedt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                        {
                                            if (ParentIKEEntryId != 0)
                                            {
                                                ChildIKEEntryId = Convert.ToInt32(childOnedt.Rows[0]["CT_ENTRY_ID"]);
                                                ChildIKEVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                Process = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(childOnedt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                        {
                                            if (ParentANPRFrontEntryId != 0)
                                            {
                                                ChildAnprFrontEntryId = Convert.ToInt32(childOnedt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                ChildANPRFrontVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                Process = false;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(childOnedt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                        {
                                            if (ParentANPRRearEntryId != 0)
                                            {
                                                ChildAnprRearEntryId = Convert.ToInt32(childOnedt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                ChildANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                ChildPacketCount++;
                                            }
                                            else
                                            {
                                                Process = false;
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(childOnedt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                        {
                                            if (childOnedt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                            {
                                                isChildBalanceUpdated = true;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (Process)
                                {
                                    #region Find 2nd Associated Transcation reated pakcet id and is balance already deducated or not
                                    if (childTwoTranasactionId > 0)
                                    {
                                        DataTable childTwodt = TransactionBLL.GetDataTableFilteredRecordById(childTwoTranasactionId);
                                        if (childTwodt.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(childTwodt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                            {
                                                if (ParentIKEEntryId != 0)
                                                {
                                                    if (ChildIKEEntryId == 0)
                                                    {
                                                        ChildIKEEntryId = Convert.ToInt32(childTwodt.Rows[0]["CT_ENTRY_ID"]);
                                                        ChildIKEVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Process = false;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(childTwodt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                                            {
                                                if (ParentANPRRearEntryId != 0)
                                                {
                                                    if (ChildAnprFrontEntryId == 0)
                                                    {
                                                        ChildAnprFrontEntryId = Convert.ToInt32(childTwodt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                                                        ChildANPRFrontVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Process = false;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(childTwodt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                                            {
                                                if (ParentANPRRearEntryId != 0)
                                                {
                                                    if (ChildAnprRearEntryId == 0)
                                                    {
                                                        ChildAnprRearEntryId = Convert.ToInt32(childTwodt.Rows[0]["NF_ENTRY_ID_REAR"]);
                                                        ChildANPRRearVehicleClassId = Convert.ToInt32(dt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                                    }
                                                    else
                                                    {
                                                        Process = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Process = false;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(childTwodt.Rows[0]["IS_BALANCE_UPDATED"].ToString()))
                                            {
                                                if (childTwodt.Rows[0]["IS_BALANCE_UPDATED"].ToString() == "1")
                                                {
                                                    isChildBalanceUpdated = true;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    if (Process)
                                    {
                                        MarkAsViolation(ParentTransactionId, objtransaction);
                                        #region Process for mearged
                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildIKEEntryId, ChildAnprFrontEntryId, ChildAnprRearEntryId, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                        if (isChildViolation)
                                        {
                                            MarkAsViolation(childOneTranasactionId, objtransaction);
                                            MarkAsViolation(childTwoTranasactionId, objtransaction);
                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                        }
                                        else
                                        {
                                            TransactionBLL.MeargedAuditTransaction(childOneTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                            TransactionBLL.MeargedAuditTransaction(childTwoTranasactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.MeargedCharged);
                                        }

                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "success-Yes Mearge-Yes";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Invalid Associtated Selected
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "Please Select valid associated transcation.";
                                        objResponseMessage.Add(objModelState);
                                        Process = false;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region Invalid Associtated Selected
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Please Select valid associated transcation.";
                                    objResponseMessage.Add(objModelState);
                                    Process = false;
                                    #endregion
                                }
                            }
                            else
                            {
                                MarkAsViolation(ParentTransactionId, objtransaction);
                                TransactionBLL.MeargedAuditTransaction(ParentTransactionId, 0, 0, 0, VehRegNo, AuditvehicleClassId, Convert.ToInt32(Session["LoggedUserId"].ToString()), (int)Constants.TranscationStatus.ViolationMeargedCharged);
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "success-Yes Mearge-No";
                                objResponseMessage.Add(objModelState);
                                Process = false;
                            }
                        }
                        else
                        {
                            HelperClass.LogMessage("no transcation found to reviewed");
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong";
                            objResponseMessage.Add(objModelState);
                            Process = false;
                        }
                        #endregion
                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to reviewedtTranscation" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }

            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reviewed()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Reviewed");
            //AND TRANSACTION_DATETIME BETWEEN TO_DATE('" + DateTime.Now.AddMinutes(-30) + "', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + DateTime.Now + "', 'DD/MM/YYYY HH24:MI:SS')
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetReviewedDataTableFilteredRecords(strQuery);
            return View("Reviewed", dt);
        }

        public ActionResult Charged()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Charged");
            //AND TRANSACTION_DATETIME BETWEEN TO_DATE('" + DateTime.Now.AddMinutes(-30) + "', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + DateTime.Now + "', 'DD/MM/YYYY HH24:MI:SS')
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetChargedDataTableFilteredRecords(strQuery);
            return View("Charged", dt);
        }

        public ActionResult TopUP()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "TopUP");
            //AND TRANSACTION_DATETIME BETWEEN TO_DATE('" + DateTime.Now.AddMinutes(-30) + "', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + DateTime.Now + "', 'DD/MM/YYYY HH24:MI:SS')
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strQuery);
            return View("TopUP", dt);
        }

        public ActionResult Violation()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Violation");
            //AND TRANSACTION_DATETIME BETWEEN TO_DATE('" + DateTime.Now.AddMinutes(-30) + "', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + DateTime.Now + "', 'DD/MM/YYYY HH24:MI:SS')
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetViolationDataTableFilteredRecords(strQuery);
            return View("Violation", dt);
        }

        public ActionResult Unidentified()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "CustomerTransaction", "Unidentified");
            string strQuery = "WHERE 1=1";
            dt = TransactionBLL.GetUnIdentifiedDataTableFilteredRecords(strQuery);
            return View("Unidentified", dt);
        }


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
                        AFTERDEDUCTION = AFTERDEDUCTION.Substring(0, 149);
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
                        NOTIFICATION = NOTIFICATION.Substring(0, 149);
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

                lock (tollRates)
                {
                    currentTimeTollRates = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetTollRateCollection(transactionTime, tollRates);
                }

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
    }
}