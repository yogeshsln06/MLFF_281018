using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication.Controllers
{
    public class POSController : Controller
    {
        int tmsId = 1;
        // GET: POS
        public ActionResult Index()
        {
            return View();
        }

        //Get
        public ActionResult CustomerRegistrationList()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Customer Registration List in POS Controller" + ex);
            }
            return View();
        }

        //Get
        [HttpGet]
        public ActionResult RegisterCustomer()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                #region Bind Province DropDowm
                List<SelectListItem> provincelist = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE> province = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ProvinceBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE>().ToList();

                provincelist.Add(new SelectListItem() { Text = "--Select Province--", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
                {
                    provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
                }

                ViewBag.Provinces = provincelist;

                #endregion
                #region Queue Status
                List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
                Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));

                for (int i = 0; i < arStatus.Length; i++)
                {
                    customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
                }
                ViewBag.queueStatus = customerQueueStatus;

                #endregion
                #region Gender
                List<SelectListItem> genderList = new List<SelectListItem>();
                Array argender = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Gender));

                for (int i = 0; i < argender.Length; i++)
                {
                    genderList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GenderName[i], Value = System.Convert.ToString((int)argender.GetValue(i)) });
                }
                ViewBag.genderName = genderList;

                #endregion
                #region MaritalStatus
                List<SelectListItem> maritalstatusList = new List<SelectListItem>();
                Array armaritalstatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatus));

                for (int i = 0; i < armaritalstatus.Length; i++)
                {
                    maritalstatusList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatusName[i], Value = System.Convert.ToString((int)armaritalstatus.GetValue(i)) });
                }
                ViewBag.maritalstatusName = maritalstatusList;

                #endregion
                #region Nationality
                List<SelectListItem> nationalityList = new List<SelectListItem>();
                Array arnationality = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Nationality));

                for (int i = 0; i < arnationality.Length; i++)
                {
                    nationalityList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.NationalityName[i], Value = System.Convert.ToString((int)arnationality.GetValue(i)) });
                }
                ViewBag.nationalityName = nationalityList;

                #endregion

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Register Customer in POS Controller" + ex);
            }
            return View();
        }

        public ActionResult GetCityList(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount)
        {
            Libraries.CommonLibrary.CBE.CityCBE city = new CityCBE();
            city.TmsId = 1;
            city.ProvinceId = customerAccount.ProvinceId;
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE> citys = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CityBLL.GetByProvinceId(city).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(citys);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDistrictList(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount)
        {
            Libraries.CommonLibrary.CBE.DistrictCBE district = new DistrictCBE();
            district.TmsId = 1;
            district.CityId = customerAccount.CityId;
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE> districts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.DistrictBLL.GetByCityId(district).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(districts);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubDistrictList(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount)
        {
            Libraries.CommonLibrary.CBE.SubDistrictCBE subdistrict = new SubDistrictCBE();
            subdistrict.TmsId = 1;
            subdistrict.DistrictId = customerAccount.DistrictId;
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE> subdistricts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubDistrictBLL.GetByDistrictId(subdistrict).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(subdistricts);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult RegisterCustomer(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount, FormCollection form)
        {

            try
            {

                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"

                };
                //-------------------Validating image-----------------------------

                if (customerAccount.CustomerImage != null && customerAccount.CustomerImage.ContentLength > 0)
                {
                    if (!imageTypes.Contains(customerAccount.CustomerImage.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return RedirectToAction("RegisterCustomer");
                    }

                    if (customerAccount.CustomerImage.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return RedirectToAction("RegisterCustomer");
                    }
                }
                if (customerAccount.CustomerImage != null && customerAccount.CustomerImage.ContentLength > 0)
                {
                    string customerImageName = customerAccount.FirstName.ToString() + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(customerAccount.CustomerImage.FileName).ToLower();

                    String uploadFilePath = "\\Attachment\\";
                    // create a folder for distinct user -
                    string FolderName = "Customer";
                    string pathWithFolderName = Server.MapPath(uploadFilePath + FolderName);

                    bool folderExists = Directory.Exists(pathWithFolderName);
                    if (!folderExists)
                        Directory.CreateDirectory(pathWithFolderName);

                    if (extension.ToLower() == ".pdf")
                    {
                        //string renamedFile = System.Guid.NewGuid().ToString("N");
                        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);
                        customerAccount.CustomerImage.SaveAs(filePath);
                    }
                    else
                    {
                        using (var img = System.Drawing.Image.FromStream(customerAccount.CustomerImage.InputStream))
                        {
                            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);

                            // Save large size image, 600 x 600
                            VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new System.Drawing.Size(600, 600), filePath);
                        }
                    }
                    customerAccount.CustomerImagePath = customerImageName + extension;
                }
                //-------------------Validating image-----------------------------

                if (customerAccount.ScannedDocument != null && customerAccount.ScannedDocument.ContentLength > 0)
                {
                    if (!imageTypes.Contains(customerAccount.ScannedDocument.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return RedirectToAction("RegisterCustomer");
                    }

                    if (customerAccount.ScannedDocument.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return RedirectToAction("RegisterCustomer");
                    }
                }
                if (customerAccount.ScannedDocument != null && customerAccount.ScannedDocument.ContentLength > 0)
                {
                    string customerImageName = customerAccount.FirstName.ToString() + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(customerAccount.ScannedDocument.FileName).ToLower();

                    String uploadFilePath = "\\Attachment\\";
                    // create a folder for distinct user -
                    string FolderName = "Customer";
                    string pathWithFolderName = Server.MapPath(uploadFilePath + FolderName);

                    bool folderExists = Directory.Exists(pathWithFolderName);
                    if (!folderExists)
                        Directory.CreateDirectory(pathWithFolderName);

                    if (extension.ToLower() == ".pdf")
                    {
                        //string renamedFile = System.Guid.NewGuid().ToString("N");
                        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);
                        customerAccount.ScannedDocument.SaveAs(filePath);
                    }
                    else
                    {
                        using (var img = System.Drawing.Image.FromStream(customerAccount.ScannedDocument.InputStream))
                        {
                            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);

                            // Save large size image, 600 x 600
                            VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new System.Drawing.Size(600, 600), filePath);
                        }
                    }
                    customerAccount.ScannedDocsPath1 = customerImageName + extension;
                }
                //Insert Detail In customer Account
                customerAccount.TmsId = tmsId;
                customerAccount.RegistartionThrough = Convert.ToInt32(Session["LoggedUserId"].ToString());
                customerAccount.BirthDate = Convert.ToDateTime(form["BirthDate"].ToString());
                customerAccount.ValidUntil = Convert.ToDateTime(form["ValidUntil"].ToString());
                customerAccount.ProvinceId = Convert.ToInt32(form["ProvinceId"].ToString());
                customerAccount.CityId = Convert.ToInt32(form["ddlCityId"].ToString());
                customerAccount.DistrictId = Convert.ToInt32(form["DistrictId"].ToString());
                customerAccount.SubDistrictId = Convert.ToInt32(form["SubDistrictId"].ToString());
                int customerEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Insert(customerAccount);
                TempData["lblerror"] = "Sucessfully created Customer.";

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to Insert Customer Registration List in POS Controller" + ex);
            }

            //else
            //{
            //    TempData["lblerror"] = "Fill Mandatory Fields.";
            //}
            return RedirectToAction("RegisterCustomer");
        }


        public ActionResult CustomerQueueList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                #region Queue Status
                List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
                Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));

                for (int i = 0; i < arStatus.Length; i++)
                {
                    customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
                }
                ViewBag.queueStatus = customerQueueStatus;

                #endregion

                //Call Bll To get all Plaza List
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList().Where(x => x.QueueStatus != 3).ToList();
                //customerDataList =customerDataList.Select(x => x.QueueStatus != 3).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>().ToList();
                return View(customerDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Customer Queue List in POS Controller" + ex);
            }
            return View();
        }
        public ActionResult TagSaleList()
        {

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();

                return View(customerDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load TagSaleList in POS Controller" + ex);
            }
            return View(customerDataList);
        }

        [HttpGet]
        public ActionResult CustomerTagList(int id)//here id is account id
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle = new Libraries.CommonLibrary.CBE.CustomerVehicleCBE();
                customerVehicle.AccountId = id;
                customerVehicle.TMSId = tmsId;
                customerVehicleList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetUserByAccountId(customerVehicle).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>().ToList();
                ViewBag.AccountId = id;

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load CustomerTagList  in POS Controller" + ex);
            }
            return View(customerVehicleList);
        }

        [HttpGet]
        public ActionResult AddCustomerTag(int id)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                //Bind DropDown
                #region Vehicle Class Dropdown
                List<SelectListItem> vehicleClassList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleClass = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                vehicleClassList.Add(new SelectListItem() { Text = "Select Plaza", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleClass)
                {
                    vehicleClassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
                }

                ViewBag.VehicleClass = vehicleClassList;
                ViewBag.AccountId = id;

                #endregion

            }
            catch (Exception ex)
            {
                TempData["lblerror"] = "Not Saved Something Went Wrong";

                HelperClass.LogMessage("Failed to load AddCustomerTag  in POS Controller" + ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult RenderCustomerVehicle()
        {
            try
            {
                #region Vehicle Class filter
                List<SelectListItem> vehicleClassList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleClass = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                vehicleClassList.Add(new SelectListItem() { Text = "Select Control Room", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClass)
                {
                    vehicleClassList.Add(new SelectListItem() { Text = vc.Name, Value = System.Convert.ToString(vc.Id) });
                }

                ViewBag.VehicleClass = vehicleClassList;
                #endregion
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Render Customer vehicle in POS Controller" + ex);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddCustomerTag(int id, Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle) //Here id is account Id
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Insert Detail in customer vehicle with customer Entry Id
                    customerVehicle.AccountId = id;
                    customerVehicle.TMSId = tmsId;
                    int customerVehicleId = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Insert(customerVehicle);

                    //Call Account_History Class
                    Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory = new Libraries.CommonLibrary.CBE.AccountHistoryCBE();
                    accountHistory.TransactionId = 0;// Set 0 for Credit From POS Form
                    accountHistory.Amount = customerVehicle.Amount;//From Form 
                    accountHistory.TMSId = tmsId;
                    accountHistory.CustomerVehicleEntryId = customerVehicleId;
                    accountHistory.AccountId = id;//Set Plaza Id 1
                    accountHistory.TransactionTypeId = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransactionType.Sale);

                    //Insert Detail in Account_History
                    Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);

                    Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount = new Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                    customerAccount.TmsId = tmsId;
                    customerAccount.AccountId = id;
                    ViewBag.ID = id;

                    //Update Account Balance in Customer Account
                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateBalance(customerAccount, accountHistory.Amount);
                    TempData["lblerror"] = "SucessFully Saved";
                }
                catch (Exception ex)
                {

                    HelperClass.LogMessage("Failed to  AddCustomerTag  in POS Controller in POST" + ex);
                }
            }
            else
            {
                TempData["lblerror"] = "Mandatory Field Required";
            }

            return RedirectToAction("AddCustomerTag");
        }

        //Get
        public ActionResult AddCustomerRecharge(int id)
        {
            List<Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerAccountList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                //Get Customer Detail By ID
                customerAccountList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetById(id, Libraries.CommonLibrary.Constants.GetCurrentTMSId()).Cast<Libraries.CommonLibrary.CBE.CustomerAccountCBE>().ToList();

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to  Add CustomerRecharge  in POS Controller in POST" + ex);
            }
            return View(customerAccountList[0]);
        }
        [HttpPost]
        public ActionResult AddCustomerRecharge(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount)
        {
            try
            {
                //Call Account_History Class
                Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory = new Libraries.CommonLibrary.CBE.AccountHistoryCBE();
                accountHistory.TransactionId = 0;// Set 0 for Credit From POS Form
                accountHistory.Amount = customerAccount.Balance;//From Form 
                accountHistory.TMSId = tmsId;
                accountHistory.CustomerVehicleEntryId = 1;//Customer Vehicle Entry Decided

                accountHistory.AccountId = customerAccount.AccountId;//Set Plaza Id 1
                accountHistory.TransactionTypeId = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransactionType.Sale);

                //Insert Detail in Account_History
                Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
                customerAccount.TmsId = tmsId;

                //Update Account Balance in Customer Account
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateBalance(customerAccount, customerAccount.Balance);
                TempData["lblerror"] = "Recharge Done Sucessfully";
            }
            catch (Exception ex)
            {
                TempData["lblerror"] = "Recharge Not Done Sucessfully";
                HelperClass.LogMessage("Failed to  Add CustomerRecharge  in POS Controller in POST" + ex);
            }
            return RedirectToAction("AddCustomerRecharge");
        }
        //Get
        public ActionResult RechargeList()
        {
            try
            {

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load RechargeList in POS Controller" + ex);
            }
            return View();
        }
        //Get
        public ActionResult BalanceList()
        {
            try
            {

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Balance List in POS Controller" + ex);
            }
            return View();
        }

        #region CustomerDetails and Transcation
        public ActionResult CustomerDetails()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewData["apiPath"] = HelperClass.GetAPIUrl();
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load TagSaleList in POS Controller" + ex);
            }
            return View();
        }

        public ActionResult FilterCustomer(Libraries.CommonLibrary.CBE.ViewTransactionCBE transaction)
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetByMobileNumber(transaction.MobileNo).Cast<CustomerAccountCBE>().ToList();
               

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load TagSaleList in POS Controller" + ex);
            }
            return PartialView("_CustomerDetails", customerDataList);
        }

        public ActionResult CustomerDetails1()
        {

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();

                return View(customerDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load TagSaleList in POS Controller" + ex);
            }
            return View(customerDataList);
        }
        public ActionResult CustomerVehicleListbyCustomer(int id)
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
            try
            {

                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.AccountId = id;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                CustomerVehicleCBE customer = new CustomerVehicleCBE();
                customer.AccountId = id;
                customer.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();

                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetUserByAccountId(customer).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>().ToList();
                return PartialView("_CustomerVehicleDetails", customerDataList);



            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
                return PartialView("_CustomerVehicleDetails", customerDataList);
            }

        }

        public ActionResult RechargeHistorybyCustomer(int id)
        {
            DataTable dt = new DataTable();
            try
            {

                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.AccountId = id;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                CustomerVehicleCBE customer = new CustomerVehicleCBE();
                customer.AccountId = id;
                customer.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();

                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.AccountHistoryBYAccountId(id,2);
                return PartialView("_CustomerRechargeHistory", dt);



            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
                return PartialView("_CustomerRechargeHistory", dt);
            }

        }

        public ActionResult TranscationHistorybyCustomer(int id)
        {
            DataTable dt = new DataTable();
            try
            {

                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.AccountId = id;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                CustomerVehicleCBE customer = new CustomerVehicleCBE();
                customer.AccountId = id;
                customer.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();

                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.AccountHistoryBYAccountId(id, 4);
                return PartialView("_CustomerTranscationHistory", dt);



            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
                return PartialView("_CustomerTranscationHistory", dt);
            }

        }
        #endregion


    }
}