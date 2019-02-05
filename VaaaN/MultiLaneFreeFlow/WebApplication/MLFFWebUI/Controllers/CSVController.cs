using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

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
        public string ExportCSVCustomer()
        {
            var filename = "Customer_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            try
            {
                FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
                Int16 IsDataFound = CSVUtility.CreateCsvWithTitle(file.FullName, CustomerAccountBLL.GetAllAsCSV(), "Customer");
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

        [HttpPost]
        public string ExportCustomerAccountFilter(CustomerVehicleModel objCustomerVehicleModel)
        {
            var filename = "Customer_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
            Int16 IsDataFound = 0;
            try
            {
                string strQuery = " WHERE 1=1";
                if (objCustomerVehicleModel.SearchEnable)
                {
                    #region Filter Query
                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        strQuery += " AND CA.ACCOUNT_ID LIKE '%" + objCustomerVehicleModel.AccountId + "%'";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.ResidentId))
                    {
                        strQuery += " AND CA.RESIDENT_ID LIKE '%" + objCustomerVehicleModel.ResidentId + "%'";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        strQuery += " AND CA.MOB_NUMBER LIKE '%" + objCustomerVehicleModel.MobileNo + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.EmailId))
                    {
                        strQuery += " AND LOWER(CA.EMAIL_ID) LIKE '%" + objCustomerVehicleModel.EmailId.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.FirstName))
                    {
                        strQuery += " AND LOWER(CA.FIRST_NAME) LIKE '%" + objCustomerVehicleModel.FirstName.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehRegNo))
                    {
                        strQuery += " AND LOWER(CV.VEH_REG_NO) LIKE '%" + objCustomerVehicleModel.VehRegNo.ToLower() + "%'";
                    }
                    IsDataFound = CSVUtility.CreateCsvWithTitleFilter(file.FullName, CustomerAccountBLL.GetFilterCSV(strQuery), "Customer", objCustomerVehicleModel);
                    #endregion
                }
                else
                {
                    IsDataFound = CSVUtility.CreateCsvWithTitle(file.FullName, CustomerAccountBLL.GetFilterCSV(strQuery), "Customer");
                }
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

        #region Customer Vehicle
        public string ExportCSVCustomerVehicle(string ViewId)
        {
            var filename = "CustomerVehicle_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            try
            {
                FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
                Int16 IsDataFound = CSVUtility.CreateCsv(file.FullName, CustomerVehicleBLL.GetAllAsCSV());
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

        [HttpPost]
        public string ExportCustomerVehicleFilter(CustomerVehicleModel objCustomerVehicleModel)
        {
            var filename = "Vehicle_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
            Int16 IsDataFound = 0;
            try
            {
                string strQuery = " WHERE 1=1";
                if (objCustomerVehicleModel.SearchEnable)
                {
                    #region Filter Query
                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        strQuery += " AND CA.ACCOUNT_ID LIKE '%" + objCustomerVehicleModel.AccountId + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.ResidentId))
                    {
                        strQuery += " AND CA.RESIDENT_ID LIKE '%" + objCustomerVehicleModel.ResidentId.ToLower() + "%'";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        strQuery += " AND CA.MOB_NUMBER LIKE '%" + objCustomerVehicleModel.MobileNo.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.EmailId))
                    {
                        strQuery += " AND LOWER(CA.EMAIL_ID) LIKE '%" + objCustomerVehicleModel.EmailId.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.FirstName))
                    {
                        strQuery += " AND LOWER(CA.FIRST_NAME) LIKE '%" + objCustomerVehicleModel.FirstName.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehRegNo))
                    {
                        strQuery += " AND LOWER(CV.VEH_REG_NO) LIKE '%" + objCustomerVehicleModel.VehRegNo.ToLower() + "%' ";
                    }
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehicleRCNumber))
                    {
                        strQuery += " AND LOWER(CV.VEHICLE_RC_NO) LIKE '%" + objCustomerVehicleModel.VehicleRCNumber.ToLower() + "%'";
                    }
                    if (objCustomerVehicleModel.VehicleClassId > 0)
                    {
                        strQuery += " AND CV.VEHICLE_CLASS_ID = " + objCustomerVehicleModel.VehicleClassId + "";
                    }
                    if (objCustomerVehicleModel.QueueStatus > 0)
                    {
                        strQuery += " AND CV.QUEUE_STATUS = " + objCustomerVehicleModel.QueueStatus + "";
                    }
                    if (objCustomerVehicleModel.ExceptionFlag > 0)
                    {
                        strQuery += " AND CV.EXCEPTION_FLAG = " + objCustomerVehicleModel.ExceptionFlag + "";
                    }
                    IsDataFound = CSVUtility.CreateCsvWithTitleFilter(file.FullName, CustomerVehicleBLL.GetFilterCSV(strQuery), "Vehicle", objCustomerVehicleModel);
                    #endregion
                }
                else
                {
                    IsDataFound = CSVUtility.CreateCsvWithTitle(file.FullName, CustomerVehicleBLL.GetFilterCSV(strQuery), "Vehicle");
                }
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

        #region Transaction Export
        public string ExportCSVRFID()
        {
            var filename = "Transaction_RFID_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            try
            {
                FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
                Int16 IsDataFound = CSVUtility.CreateCsv(file.FullName, CustomerVehicleBLL.GetAllAsCSV());
                if (IsDataFound == 0)
                    filename = "No Data to Export.";
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed Export RFID CSV " + ex);
            }
            string Det = JsonConvert.SerializeObject(filename, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }

        [HttpPost]
        public string ExportCSVANPR()
        {
            var filename = "Transaction_ANPR_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            try
            {
                FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
                Int16 IsDataFound = CSVUtility.CreateCsv(file.FullName, CustomerVehicleBLL.GetAllAsCSV());
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

        public string ExportCSVTranscations(ViewTransactionCBE transaction)
        {
            Int16 IsDataFound = 0;
            var filename = "TransactionDetails_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".csv";
            FileInfo file = new FileInfo(Server.MapPath("~/Attachment/ExportFiles/" + filename));
            try
            {
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
                IsDataFound = CSVUtility.CreateCsv(file.FullName, TransactionBLL.TransDeatils(strQuery));
                if (IsDataFound == 0)
                    filename = "No Data to Export.";
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed Export Transaction Details_ CSV " + ex);
            }
            string Det = JsonConvert.SerializeObject(filename, Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
        }
        #endregion

    }
}