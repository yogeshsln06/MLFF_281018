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
using VaaaN.MLFF.Libraries.CommonLibrary.Common;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

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
        private MessageQueue smsMessageQueue;
        static MessageQueue failedQueue;


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

            gantryList.Add(new SelectListItem() { Text = "All Gantry", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
            {
                gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
            }

            ViewBag.Gantry = gantryList;

            #endregion

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

            vehicleClass.Add(new SelectListItem() { Text = "", Value = "0" });
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
            if (transaction.TranscationId > 0)
            {
                strQuery += " AND T.TRANSACTION_ID = " + transaction.TranscationId;
            }
            if (transaction.TransactionCategoryId == 1)// IKE Only
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) = 0  AND NVL(T.NF_ENTRY_ID_REAR,0) = 0";
            }
            //else if (transaction.TransactionCategoryId == 2)//IKE + Front/Rear ANPR
            //{
            //    strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  > 0 AND ((NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'unidentified')  OR (NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)<>'unidentified')) ";
            //}
            else if (transaction.TransactionCategoryId == 2)//Front & Rear ANPR Only
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)<>'unidentified'  AND NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)<>'unidentified' ";
            }
            else if (transaction.TransactionCategoryId == 3)//Front/Rear ANPR Only
            {
                strQuery += "  AND NVL (T.CT_ENTRY_ID, 0) = 0 AND((NVL(T.NF_ENTRY_ID_FRONT, 0) > 0 AND LOWER(NFPF.PLATE_NUMBER) <> 'unidentified' AND NVL(T.NF_ENTRY_ID_REAR, 0) = 0) OR (NVL(T.NF_ENTRY_ID_REAR, 0) > 0 AND LOWER(NFPF.PLATE_NUMBER) <> 'unidentified' AND NVL(T.NF_ENTRY_ID_FRONT, 0) = 0)) ";
            }
            else if (transaction.TransactionCategoryId == 4)//Unidentified Front/Rear ANPR
            {
                strQuery += " AND  NVL(T.CT_ENTRY_ID,0)  = 0 AND ((NVL(T.NF_ENTRY_ID_FRONT,0) > 0 AND LOWER(NFPF.PLATE_NUMBER)='unidentified')  OR (NVL(T.NF_ENTRY_ID_REAR,0) > 0 AND LOWER(NFPR.PLATE_NUMBER)='unidentified')) ";
            }

            dt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        #region Associated Transcation
        public ActionResult AssociatedTransaction(int TransactionId)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                dt = TransactionBLL.GetUnReviewedDataTableById(TransactionId);
                ViewData["dtCount"] = dt.Rows.Count;
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
            string strfilter = String.Empty;
            DataTable Assodt = new DataTable();
            dt = (DataTable)Session["CurrentTransaction"];
            //int IKEEntryId = 0;
            //int ANPRFrontEntryId = 0;
            //int ANPRRearEntryId = 0;
            DateTime TransactionDateTime;
            int TransactionId;
            int PlazaID;
            if (dt.Rows.Count > 0)
            {
                TransactionDateTime = Convert.ToDateTime(dt.Rows[0]["TRANSACTION_DATETIME"]);
                TransactionId = Convert.ToInt32(dt.Rows[0]["TRANSACTION_ID"]);
                PlazaID = Convert.ToInt32(dt.Rows[0]["PLAZA_ID"]);
                strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + TransactionDateTime.AddSeconds(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS')  AND T.PLAZA_ID = " + PlazaID + " AND T.TRANSACTION_ID <> " + TransactionId;
                Assodt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strfilter);
                Det = JsonConvert.SerializeObject(Assodt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");

                #region Old Code
                //if (!string.IsNullOrEmpty(dt.Rows[0]["CT_ENTRY_ID"].ToString()))
                //{
                //    IKEEntryId = Convert.ToInt32(dt.Rows[0]["CT_ENTRY_ID"]);
                //}
                //if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_FRONT"].ToString()))
                //{
                //    ANPRFrontEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_FRONT"]);
                //}
                //if (!string.IsNullOrEmpty(dt.Rows[0]["NF_ENTRY_ID_REAR"].ToString()))
                //{
                //    ANPRRearEntryId = Convert.ToInt32(dt.Rows[0]["NF_ENTRY_ID_REAR"]);
                //}
                //if (IKEEntryId > 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId > 0)
                //{
                //    Det = "No Record Found";

                //}
                //else
                //{
                //    strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + TransactionDateTime.AddSeconds(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID <> " + TransactionId;
                //    if (IKEEntryId > 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId == 0)
                //    {
                //        strfilter += " AND (NVL(CT_ENTRY_ID,0) = 0 AND (NVL(NF_ENTRY_ID_FRONT,0) > 0 OR NVL(NF_ENTRY_ID_REAR,0) > 0))";
                //    }
                //    else if (IKEEntryId == 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId == 0)
                //    {
                //        strfilter += " AND (NVL(NF_ENTRY_ID_FRONT,0) = 0 AND (NVL(CT_ENTRY_ID,0) > 0 OR NVL(NF_ENTRY_ID_REAR,0) > 0))";
                //    }
                //    else if (IKEEntryId == 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId > 0)
                //    {
                //        strfilter += " AND (NVL(NF_ENTRY_ID_REAR,0) = 0 AND (NVL(CT_ENTRY_ID,0) > 0 OR NVL(NF_ENTRY_ID_FRONT,0) > 0))";
                //    }
                //    else if (IKEEntryId > 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId == 0)
                //    {
                //        strfilter += " AND NVL(CT_ENTRY_ID,0) = 0 AND NVL(NF_ENTRY_ID_FRONT,0) = 0) AND (NVL(NF_ENTRY_ID_REAR,0) <> 0 ";
                //    }
                //    else if (IKEEntryId > 0 && ANPRFrontEntryId == 0 && ANPRRearEntryId > 0)
                //    {
                //        strfilter += " AND NVL(CT_ENTRY_ID,0) = 0 AND NVL(NF_ENTRY_ID_FRONT,0) <> 0 AND NVL(NF_ENTRY_ID_REAR,0) = 0 ";
                //    }
                //    else if (IKEEntryId == 0 && ANPRFrontEntryId > 0 && ANPRRearEntryId > 0)
                //    {
                //        strfilter += " AND NVL(CT_ENTRY_ID,0) <> 0 AND NVL(NF_ENTRY_ID_FRONT,0) = 0 AND NVL(NF_ENTRY_ID_REAR,0) = 0 ";
                //    }
                //    Assodt = TransactionBLL.GetUnReviewedDataTableFilteredRecords(strfilter);
                //    Det = JsonConvert.SerializeObject(Assodt, Formatting.Indented);
                //    Det = Det.Replace("\r", "").Replace("\n", "");
                //}
                #endregion

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
            #region Varaible 
            JsonResult result = new JsonResult();
            Int32 AuditedVehicleClassId = Convert.ToInt32(vehicleClassID);
            Int32 ParentTransactionId = TransactionId;
            Int32 ChildTranasactionId = 0;
            Int32 AlreadyChargedVRNId = 0;
            Int32 ParentIKEFrontVehicleClassId = 0;
            Int32 ParentIKERearVehicleClassId = 0;
            Int32 ChildIKEVehicleClassId = 0;

            ArrayList ChildClassIdArrayList = new ArrayList();

            int MaxParentClassId = AuditedVehicleClassId;

            bool Violation = false;
            bool IsAlreadyAuditedVRN = false;
            bool IsAlreadyChargedVRN = false;

            String Meassage = String.Empty;
            String strfilter = String.Empty;
            String ValidateChildsIds = String.Empty;
            String ChildsIds = String.Empty;
            String AuditedVRN = VehRegNo;
            #endregion

            if (String.IsNullOrEmpty(AuditedVRN))
            {
                #region VRN Validation
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "VRN is required for reviewing transaction.";
                objResponseMessage.Add(objModelState);
                #endregion
            }
            else
            {
                try
                {
                    #region Get transaction Data by id
                    dt = TransactionBLL.GetDataTableFilteredRecordById(TransactionId);
                    if (dt.Rows.Count > 0)
                    {
                        #region Transaction Exists
                        #region Fill mandatory parameter
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
                        #endregion

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
                        if (AssociatedTransactionIds != null)
                        {
                            for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                            {
                                ChildsIds = ChildsIds + "," + AssociatedTransactionIds[i];
                            }
                        }
                        #region Check audited VPN is already audited or not in one miniute
                        if (!string.IsNullOrEmpty(ChildsIds))
                        {
                            ValidateChildsIds = TransactionId.ToString() + ChildsIds;
                        }
                        else
                        {
                            ValidateChildsIds = TransactionId.ToString();
                        }
                        strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + objtransaction.TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + objtransaction.TransactionDateTime.AddMinutes(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.AUDITED_VRN='" + AuditedVRN + "' AND NVL(T.AUDIT_STATUS,0)=1 AND T.PLAZA_ID=" + objtransaction.PlazaId + "";
                        //strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + objtransaction.TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + objtransaction.TransactionDateTime.AddMinutes(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID NOT IN (" + ValidateChildsIds + ") AND T.AUDITED_VRN='" + AuditedVRN + "' AND NVL(T.AUDIT_STATUS,0)=1";
                        DataTable Assodt = TransactionBLL.GetDataTableFilteredRecords(strfilter);
                        if (Assodt.Rows.Count > 0)
                        {
                            IsAlreadyAuditedVRN = true;
                        }
                        #endregion

                        #region Check auditied VRN Exists or not
                        if (string.IsNullOrEmpty(CustomerVehicleDetails.VehRegNo))
                        {
                            #region Audited VRN not registered Mark as Unregistred Violance and Auditied
                            if (IsAlreadyAuditedVRN)
                            {
                                #region This VRN alreday Reviewed
                                int ReviewedTranId = Convert.ToInt32(Assodt.Rows[0]["TRANSACTION_ID"].ToString());
                                TransactionBLL.MarkAsUnregistred(objtransaction);
                                TransactionBLL.MeargedAuditTransaction(ReviewedTranId, TransactionId, (int)Constants.TranscationStatus.Merged);
                                objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                TransactionBLL.UpdateAuditSection(objtransaction);
                                Meassage = "yes!Transactions ID " + TransactionId + " successfully MERGED to Transactions ID " + ReviewedTranId + " that already reviewed and set as VIOLATION!!!";
                                if (AssociatedTransactionIds != null)
                                {

                                    for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                    {
                                        ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                        objtransaction.TransactionId = ChildTranasactionId;
                                        TransactionBLL.MarkAsUnregistred(objtransaction);
                                        TransactionBLL.MeargedAuditTransaction(ReviewedTranId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ReviewedTranId + "!!!";
                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region This VRN not Reviewed
                                objtransaction.TransactionId = ParentTransactionId;
                                TransactionBLL.MarkAsUnregistred(objtransaction);
                                objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                TransactionBLL.UpdateAuditSection(objtransaction);
                                Meassage = "yes!Transactions ID " + TransactionId + " set as VIOLATION!!!";
                                if (AssociatedTransactionIds != null)
                                {

                                    for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                    {
                                        ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                        objtransaction.TransactionId = ChildTranasactionId;
                                        TransactionBLL.MarkAsUnregistred(objtransaction);
                                        TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ParentTransactionId + "!!!";
                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                    }
                                }
                                #endregion
                            }

                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = Meassage;
                            objResponseMessage.Add(objModelState);
                            #endregion
                        }
                        else
                        {
                            #region Check Associated TransactionIds is avaliable or not 
                            if (AssociatedTransactionIds != null)
                            {
                                for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                {
                                    ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                    DataTable Childdt = TransactionBLL.GetDataTableFilteredRecordById(ChildTranasactionId);
                                    if (Childdt.Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Childdt.Rows[0]["CT_ENTRY_ID"].ToString()))
                                        {
                                            ChildIKEVehicleClassId = Convert.ToInt32(Childdt.Rows[0]["CTP_VEHICLE_CLASS_ID"]);
                                            ChildClassIdArrayList.Add(new child { TransId = ChildTranasactionId, ClassId = ChildIKEVehicleClassId, });

                                        }
                                        if (!string.IsNullOrEmpty(Childdt.Rows[0]["CT_ENTRY_ID_REAR"].ToString()))
                                        {
                                            ChildIKEVehicleClassId = Convert.ToInt32(Childdt.Rows[0]["CTP_VEHICLE_CLASS_IDR"]);
                                            ChildClassIdArrayList.Add(new child { TransId = ChildTranasactionId, ClassId = ChildIKEVehicleClassId, });
                                        }
                                    }
                                }


                            }
                            #endregion

                            #region Check max Class Id
                            int[] ParentClassIdArray = {
                                ParentIKEFrontVehicleClassId,
                                ParentIKERearVehicleClassId,
                                AuditedVehicleClassId,
                                CustomerVehicleDetails.VehicleClassId
                            };
                            if (ParentClassIdArray.Length > 0)
                                MaxParentClassId = ParentClassIdArray.Max();
                            #endregion

                            #region Check audited VRN is already balance deduct or not in one miniute
                            strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + objtransaction.TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + objtransaction.TransactionDateTime.AddMinutes(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND  (CTP.PLATE_NUMBER = '" + AuditedVRN + "' OR CTPR.PLATE_NUMBER = '" + AuditedVRN + "' OR NFPF.PLATE_NUMBER = '" + AuditedVRN + "' OR NFPR.PLATE_NUMBER = '" + AuditedVRN + "') AND NVL(T.IS_BALANCE_UPDATED,0)=1 AND T.PLAZA_ID=" + objtransaction.PlazaId + "";
                            //strfilter = " WHERE TRANSACTION_DATETIME BETWEEN TO_DATE('" + objtransaction.TransactionDateTime.AddSeconds(-Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + objtransaction.TransactionDateTime.AddMinutes(Seconds).ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS') AND T.TRANSACTION_ID NOT IN (" + ValidateChildsIds + ") AND (CTP.PLATE_NUMBER = '" + AuditedVRN + "' OR CTPR.PLATE_NUMBER = '" + AuditedVRN + "' OR NFPF.PLATE_NUMBER = '" + AuditedVRN + "' OR NFPR.PLATE_NUMBER = '" + AuditedVRN + "') AND NVL(T.IS_BALANCE_UPDATED,0)=1";
                            DataTable alreadyCharged = TransactionBLL.GetDataTableFilteredRecords(strfilter);
                            if (alreadyCharged.Rows.Count > 0)
                            {
                                IsAlreadyChargedVRN = true;
                                AlreadyChargedVRNId = Convert.ToInt32(alreadyCharged.Rows[0]["TRANSACTION_ID"].ToString());
                            }
                            #endregion

                            #region Process Transaction
                            if (IsAlreadyAuditedVRN)
                            {
                                #region This VRN alreday Reviewed
                                int ReviewedTranId = Convert.ToInt32(Assodt.Rows[0]["TRANSACTION_ID"].ToString());
                                int ReviewedViolation = 0;
                                if (Assodt.Rows[0]["IS_VIOLATION"] != DBNull.Value)
                                    ReviewedViolation = Convert.ToInt32(Assodt.Rows[0]["IS_VIOLATION"].ToString());
                                TransactionBLL.MeargedAuditTransaction(ReviewedTranId, TransactionId, (int)Constants.TranscationStatus.Merged);
                                objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                TransactionBLL.UpdateAuditSection(objtransaction);
                                Meassage = "yes!Transactions ID " + TransactionId + " successfully MERGED to Transactions ID " + ReviewedTranId + " that already reviewed!!!";
                                if (AssociatedTransactionIds != null)
                                {

                                    for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                    {
                                        ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                        objtransaction.TransactionId = ChildTranasactionId;
                                        TransactionBLL.MarkAsUnregistred(objtransaction);
                                        TransactionBLL.MeargedAuditTransaction(ReviewedTranId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ReviewedTranId + "!!!";
                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                    }
                                }
                                #endregion

                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = Meassage;
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                #region This VRN still not Reviewed
                                if (CustomerVehicleDetails.QueueStatus == 3)
                                {
                                    #region Customer Document already Verified
                                    if (IsAlreadyChargedVRN)
                                    {
                                        #region This VRN already Charged
                                        #region Check Parent Violation
                                        Violation = CheckViolation(ParentClassIdArray);
                                        TransactionBLL.MeargedAuditTransaction(AlreadyChargedVRNId, ParentTransactionId, (int)Constants.TranscationStatus.Merged);
                                        if (Violation)
                                        {
                                            #region Parent Violation found
                                            objtransaction.TransactionId = ParentTransactionId;
                                            TransactionBLL.MarkAsViolation(objtransaction);
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            Meassage = "yes!Transactions ID " + ParentTransactionId + " set as VIOLATION and successfully MERGED to Transactions ID " + AlreadyChargedVRNId + " that already reviewed and successfully CHARGED!!!";
                                            #endregion

                                            #region Process Child Data
                                            if (AssociatedTransactionIds != null)
                                            {
                                                for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                                {
                                                    ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                                    objtransaction.TransactionId = ChildTranasactionId;
                                                    TransactionBLL.MeargedAuditTransaction(AlreadyChargedVRNId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                                    Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + AlreadyChargedVRNId + "!!!";
                                                    objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                    TransactionBLL.UpdateAuditSection(objtransaction);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Parent Violation not found
                                            objtransaction.TransactionId = ParentTransactionId;
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            Meassage = "yes!Transactions ID " + ParentTransactionId + " successfully MERGED to Transactions ID " + AlreadyChargedVRNId + " that already reviewed and successfully CHARGED!!!";
                                            #endregion

                                            #region Process Child Data
                                            if (AssociatedTransactionIds != null)
                                            {
                                                bool Vresult = false;
                                                bool Presult = false;

                                                for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                                {
                                                    Presult = false;
                                                    ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                                    var query = from child classificationId in ChildClassIdArrayList
                                                                where classificationId.TransId == ChildTranasactionId
                                                                select classificationId;
                                                    #region Check for Child Violation
                                                    if (!Vresult && !Presult)
                                                    {
                                                        int PreVal = 0;
                                                        foreach (child c in query)
                                                        {
                                                            if (c.ClassId > 0)
                                                            {
                                                                if (PreVal == 0)
                                                                {
                                                                    PreVal = c.ClassId;
                                                                }
                                                                if (PreVal != c.ClassId)
                                                                {
                                                                    Vresult = true;
                                                                    Presult = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    objtransaction.TransactionId = ChildTranasactionId;
                                                    TransactionBLL.MeargedAuditTransaction(AlreadyChargedVRNId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                                    if (Vresult)
                                                    {
                                                        #region Child Violation found
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " set as VIOLATION and successfully MERGED to Transactions ID " + AlreadyChargedVRNId + "!!!";
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Child Violation not found
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + AlreadyChargedVRNId + "!!!";
                                                        #endregion
                                                    }

                                                }
                                            }
                                            #endregion



                                        }
                                        #endregion

                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = Meassage;
                                        objResponseMessage.Add(objModelState);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region This VRN is not Charged going for charging
                                        #region Check Parent Violation
                                        Violation = CheckViolation(ParentClassIdArray);

                                        objtransaction.TransactionId = ParentTransactionId;
                                        FinancialProcessing(CustomerVehicleDetails, customerAccountInfo, MaxParentClassId, objtransaction, ParentTransactionId);

                                        if (Violation)
                                        {
                                            #region Parent Violation found
                                            objtransaction.TransactionId = ParentTransactionId;
                                            TransactionBLL.MarkAsViolation(objtransaction);
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            Meassage = "yes!Transactions ID " + ParentTransactionId + " set as VIOLATION and successfully CHARGED!!!";
                                            #endregion

                                            #region Process Child Data
                                            if (AssociatedTransactionIds != null)
                                            {
                                                for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                                {
                                                    ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                                    objtransaction.TransactionId = ChildTranasactionId;
                                                    TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                                    Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ParentTransactionId + "!!!";
                                                    objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                    TransactionBLL.UpdateAuditSection(objtransaction);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Parent Violation not found
                                            objtransaction.TransactionId = ParentTransactionId;
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                            Meassage = "yes!Transactions ID " + ParentTransactionId + " successfully CHARGED!!!";
                                            #endregion

                                            #region Process Child Data
                                            if (AssociatedTransactionIds != null)
                                            {
                                                bool Vresult = false;
                                                bool Presult = false;
                                                for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                                {
                                                    Presult = false;
                                                    ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                                    var query = from child classificationId in ChildClassIdArrayList
                                                                where classificationId.TransId == ChildTranasactionId
                                                                select classificationId;
                                                    #region Check for Child Violation
                                                    if (!Vresult && !Presult)
                                                    {
                                                        int PreVal = 0;
                                                        foreach (child c in query)
                                                        {
                                                            if (c.ClassId > 0)
                                                            {
                                                                if (PreVal == 0)
                                                                {
                                                                    PreVal = c.ClassId;
                                                                }
                                                                if (PreVal != c.ClassId)
                                                                {
                                                                    Vresult = true;
                                                                    Presult = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    objtransaction.TransactionId = ChildTranasactionId;
                                                    TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                                    if (Vresult)
                                                    {
                                                        #region Child Violation found
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " set as VIOLATION and successfully MERGED to Transactions ID " + ParentTransactionId + "!!!";
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Child Violation not found
                                                        objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                                        TransactionBLL.UpdateAuditSection(objtransaction);
                                                        Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ParentTransactionId + "!!!";
                                                        #endregion
                                                    }

                                                }
                                            }
                                            #endregion



                                        }
                                        #endregion

                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = Meassage;
                                        objResponseMessage.Add(objModelState);
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Customer Document still Not Verified its marked AS Violation
                                    #region process for audit
                                    objtransaction.TransactionId = ParentTransactionId;
                                    Meassage = "yes!Transactions ID " + ParentTransactionId + "";

                                    #region violation Parent
                                    objtransaction.TransactionId = ParentTransactionId;
                                    TransactionBLL.MarkAsViolation(objtransaction);
                                    objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Violation;
                                    TransactionBLL.UpdateAuditSection(objtransaction);
                                    Meassage = Meassage + " set as VIOLATION!!!";
                                    #endregion

                                    #region No violation Child
                                    if (AssociatedTransactionIds != null)
                                    {
                                        for (int i = 0; i < AssociatedTransactionIds.Length; i++)
                                        {
                                            ChildTranasactionId = Convert.ToInt32(AssociatedTransactionIds[i]);
                                            objtransaction.TransactionId = ChildTranasactionId;
                                            TransactionBLL.MeargedAuditTransaction(ParentTransactionId, ChildTranasactionId, (int)Constants.TranscationStatus.Merged);
                                            Meassage = Meassage + "-999-Transactions ID " + ChildTranasactionId + " successfully MERGED to Transactions ID " + ParentTransactionId + "!!!";
                                            objtransaction.AuditedTranscationStatus = (int)Constants.TranscationStatus.Merged;
                                            TransactionBLL.UpdateAuditSection(objtransaction);
                                        }
                                    }
                                    #endregion

                                    #endregion

                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = Meassage;
                                    objResponseMessage.Add(objModelState);

                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Transaction not found please check for another or refresh your display.";
                        objResponseMessage.Add(objModelState);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    #region Exception Occured
                    HelperClass.LogMessage("Failed to review transaction" + ex);
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Something went wrong";
                    objResponseMessage.Add(objModelState);
                    #endregion
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

            gantryList.Add(new SelectListItem() { Text = "All Gantry", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
            {
                gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
            }

            ViewBag.Gantry = gantryList;

            #endregion

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleClass = new List<SelectListItem>();
            List<VehicleClassCBE> vehicle = VehicleClassBLL.GetAll();

            vehicleClass.Add(new SelectListItem() { Text = "All Reviewed Class", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
            {
                vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
            }

            ViewBag.VehicleClass = vehicleClass;

            #endregion

            #region Reviewer Dropdown
            List<SelectListItem> ReviewerList = new List<SelectListItem>();
            List<UserCBE> users = UserBLL.GetUserAll().Cast<UserCBE>().ToList();
            ReviewerList.Add(new SelectListItem() { Text = "All Reviewer", Value = "0" });
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
                strQuery += " AND (T.MEARGED_TRAN_ID LIKE '%" + transaction.ParentTranscationId + "%' )";
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
            if (transaction.TranscationId > 0)
            {
                strQuery += " AND T.TRANSACTION_ID = " + transaction.TranscationId;
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

            vehicleClass.Add(new SelectListItem() { Text = "All Class", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicle)
            {
                vehicleClass.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
            }

            ViewBag.VehicleClass = vehicleClass;

            #endregion

            #region Gantry Class Dropdown
            List<SelectListItem> gantryList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

            gantryList.Add(new SelectListItem() { Text = "All Gantry", Value = "0" });
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
            if (transaction.TranscationId > 0)
            {
                strQuery += " AND T.TRANSACTION_ID = " + transaction.TranscationId;
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

            vehicleClass.Add(new SelectListItem() { Text = "All Class", Value = "0" });
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
            if (transaction.TranscationId > 0)
            {
                strQuery += " AND T.ENTRY_ID = " + transaction.TranscationId;
            }
            dt = AccountHistoryBLL.GetTopUpDataTableFilteredRecords(strQuery);
            string Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion

        public ActionResult GetFile()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Transaction", "GetFile");
            #region Gantry Class Dropdown
            List<SelectListItem> gantryList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

            gantryList.Add(new SelectListItem() { Text = "All Gantry", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
            {
                gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
            }

            ViewBag.Gantry = gantryList;

            #endregion

            #region Gantry Class Dropdown
            List<SelectListItem> DeviceType = new List<SelectListItem>();

            DeviceType.Add(new SelectListItem() { Text = "", Value = "0" });
            DeviceType.Add(new SelectListItem() { Text = "RFID", Value = "1" });
            DeviceType.Add(new SelectListItem() { Text = "ANPR", Value = "1" });
            ViewBag.DeviceType = DeviceType;

            #endregion

            return View();
        }


        [HttpPost]
        public string DownloadTransactionData(ViewTransactionCBE transaction)
        {
            string result = "";
            CSVController ExportCSV = new CSVController();
            if (transaction.TranscationId == 1)
            {
                result = ExportCSV.ExportCSVRFID();
            }
            else if (transaction.TranscationId == 2)
            {
                result = ExportCSV.ExportCSVANPR();
            }
            else
            {
                string Det = JsonConvert.SerializeObject("No Data to Export.", Formatting.Indented);
                result = Det.Replace("\r", "").Replace("\n", "");
            }
            return result;
        }


        public ActionResult ReportTransactionData()
        {
            return View();
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
                if (customerVehicleInfo.ExceptionFlag == 2)
                {
                    tollToDeduct = 0;
                }
                else
                {
                    int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                    tollToDeduct = GetTollRate(Constants.GetCurrentTMSId(), laneTypeId, transaction.TransactionDateTime, ChargebleVehicleClassId);
                }
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
                int entryId = 0;
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
                    entryId = AccountHistoryBLL.Insert(accountHistory);
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
                    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, afterDeduction, entryId);
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

        private void NotificationProcessing(CustomerVehicleCBE customerVehicleInfo, CustomerAccountCBE customerAccountInfo, TransactionCBE transaction, Decimal tollToDeduct, Decimal AfterDeduction, int entryId)
        {
            try
            {


                System.Messaging.Message smsMessage = new System.Messaging.Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                CultureInfo culture = new CultureInfo("id-ID");
                string RechareDate = transaction.TransactionDateTime.AddDays(4).ToString("dd-MMM-yyyy") + " 23:59:59";
                if (AfterDeduction > 0)
                {
                    string AFTERDEDUCTION = Constants.AfterDeduction;
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[laneid]", transaction.LaneId.ToString());
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[balance]", Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tid]", transaction.TransactionId.ToString());
                    smsDetail.SMSMessage = AFTERDEDUCTION;
                }
                else
                {
                    string NOTIFICATION = Constants.AfterDeductionInsufficientBalance;
                    NOTIFICATION = NOTIFICATION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture));
                    NOTIFICATION = NOTIFICATION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    NOTIFICATION = NOTIFICATION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    NOTIFICATION = NOTIFICATION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    NOTIFICATION = NOTIFICATION.Replace("[laneid]", transaction.LaneId.ToString());
                    NOTIFICATION = NOTIFICATION.Replace("[recharedate]", RechareDate);
                    NOTIFICATION = NOTIFICATION.Replace("[liability]", Decimal.Parse(Math.Abs(AfterDeduction).ToString()).ToString("C", culture));
                    NOTIFICATION = NOTIFICATION.Replace("[tid]", transaction.TransactionId.ToString());
                    smsDetail.SMSMessage = NOTIFICATION;
                }

                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;
                smsDetail.AccountHistoryId = entryId;
                smsMessage.Body = smsDetail;
                smsMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.smsMessageQueue);
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

        private static bool CheckViolation(int[] classArray)
        {
            bool result = false;
            int PreVal = 0;
            for (int i = 0; i < classArray.Length; i++)
            {
                if (classArray[i] > 0)
                {
                    if (PreVal == 0)
                    {
                        PreVal = classArray[i];
                    }
                    if (PreVal != classArray[i])
                    {
                        result = true;
                    }
                }

            }

            return result;
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

        public ActionResult FailedQueueData()
        {
            string filepath = "";
            string rootpath = HttpContext.Server.MapPath("~/events/failed/" + DateTime.Now.ToString("dd-MMM-yyyy") + "/");
            JsonResult result = new JsonResult();
            try
            {
                //failedQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.failedQueueName);
                //failedQueue.PeekCompleted += new PeekCompletedEventHandler(failedQueue_PeekCompleted);
                //failedQueue.BeginPeek();
                //MessageQueue[] myQueueArray = MessageQueue.GetPrivateQueuesByMachine("10.213.0.33");
                System.Messaging.Message[] msgs = failedQueue.GetAllMessages();
                //failedQueue.Purge();

                if (msgs.Length > 0)
                {

                    foreach (System.Messaging.Message msg in msgs)
                    {

                        System.Messaging.Message m = msg;
                        m.Formatter = new BinaryMessageFormatter();

                        if (m != null)
                        {
                            HelperClass.LogMessage("M is not null ");
                            if (m.Body != null)
                            {
                                HelperClass.LogMessage("M is not null ");
                                #region Processing packets
                                if (m.Body is CrossTalkEvent)
                                {
                                    #region CrossTalk packet
                                    CrossTalkPacket ctp = (CrossTalkPacket)m.Body;
                                    string jsonString = JsonConvert.SerializeObject(ctp);

                                    #region Create Physical Path to save CrossTalk XML Data as file
                                    if (!Directory.Exists(rootpath))
                                    {
                                        Directory.CreateDirectory(rootpath);
                                    }
                                    filepath = rootpath + "CrossTalk/";
                                    if (!Directory.Exists(filepath))
                                    {
                                        Directory.CreateDirectory(filepath);
                                    }
                                    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".xml";
                                    if (!System.IO.File.Exists(filepath))
                                    {
                                        System.IO.File.Create(filepath).Dispose();
                                        System.IO.File.WriteAllText(filepath, jsonString);
                                    }
                                    else
                                    {
                                        var guid = Guid.NewGuid().ToString();
                                        filepath = rootpath + "CrossTalk/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".xml";
                                        System.IO.File.Create(filepath).Dispose();
                                        System.IO.File.WriteAllText(filepath, jsonString);
                                    }
                                    #endregion
                                    #endregion
                                }
                                else if (m.Body is NodeFluxEvent)
                                {
                                    #region NodeFlux packet
                                    NodeFluxPacket ntp = (NodeFluxPacket)m.Body;
                                    string jsonString = JsonConvert.SerializeObject(ntp);
                                    #endregion

                                    #region Create Physical Path to save nodeflux JSON Data as file
                                    if (!Directory.Exists(rootpath))
                                    {
                                        Directory.CreateDirectory(rootpath);
                                    }
                                    filepath = rootpath + "Hikvision/";
                                    if (!Directory.Exists(filepath))
                                    {
                                        Directory.CreateDirectory(filepath);
                                    }
                                    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".json";
                                    if (!System.IO.File.Exists(filepath))
                                    {
                                        System.IO.File.Create(filepath).Dispose();
                                        System.IO.File.WriteAllText(filepath, jsonString);
                                    }
                                    else {
                                        var guid = Guid.NewGuid().ToString();
                                        filepath = rootpath + "Hikvision/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                                        System.IO.File.Create(filepath).Dispose();
                                        System.IO.File.WriteAllText(filepath, jsonString);
                                    }



                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                HelperClass.LogMessage("Body null");
                            }
                        }
                        else
                        {
                            HelperClass.LogMessage("Failed Formatter null");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to read MSMQ failed  data. " + ex.Message);
            }

            return View();
        }

        void failedQueue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {

        }


        public class child
        {
            public int ClassId { get; set; }
            public int TransId { get; set; }
        }
    }
}