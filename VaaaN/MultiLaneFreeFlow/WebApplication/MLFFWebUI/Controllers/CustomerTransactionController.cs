using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using static MLFFWebUI.Models.HelperClass;

namespace MLFFWebUI.Controllers
{

    public class CustomerTransactionController : Controller
    {
        DataTable dt = new DataTable();
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();

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
            return View("Unreviewed", dt);
        }
    }
}