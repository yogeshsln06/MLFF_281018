using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.Classes.MobileBroadCast;
using static MLFFWebUI.Models.HelperClass;

namespace MLFFWebUI.Controllers
{
    public class RegistrationController : Controller
    {
        int tmsId = 1;
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();
        DataTable dt = new DataTable();

        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        #region Customer Account
        [HttpGet]
        public ActionResult Customer()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("Logout", "Login");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Registration", "Customer");


                return View();

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return View();
        }

        [HttpPost]
        public JsonResult CustomerAccountListScroll(int pageindex, int pagesize)
        {
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            JsonResult result = new JsonResult();
            customerDataList = CustomerAccountBLL.CustomerAccountLazyLoad(pageindex, pagesize);
            result.Data = customerDataList;
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CustomerAccountListAll()
        {
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            JsonResult result = new JsonResult();
            customerDataList = CustomerAccountBLL.GetAllAsList();
            result.Data = customerDataList;
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewCustomer()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            #region Bind Province DropDowm
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }

            ViewBag.Provinces = provincelist;
            ViewBag.hfProvinceId = 0;
            ViewBag.hfCityId = 0;
            ViewBag.hfDistrictId = 0;
            ViewBag.hfSubDistrictId = 0;
            ViewBag.PostalCode = "";
            ViewBag.CustomerDocumentPath = "";
            #endregion

            #region Gender
            List<SelectListItem> genderList = new List<SelectListItem>();
            Array argender = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Gender));
            genderList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < argender.Length; i++)
            {
                genderList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GenderName[i], Value = System.Convert.ToString((int)argender.GetValue(i)) });
            }
            ViewBag.genderName = genderList;

            #endregion

            #region MaritalStatus
            List<SelectListItem> maritalstatusList = new List<SelectListItem>();
            Array armaritalstatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatus));
            maritalstatusList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < armaritalstatus.Length; i++)
            {
                maritalstatusList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatusName[i], Value = System.Convert.ToString((int)armaritalstatus.GetValue(i)) });
            }
            ViewBag.maritalstatusName = maritalstatusList;

            #endregion

            #region Nationality
            List<SelectListItem> nationalityList = new List<SelectListItem>();
            Array arnationality = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Nationality));
            nationalityList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arnationality.Length; i++)
            {
                nationalityList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.NationalityName[i], Value = System.Convert.ToString((int)arnationality.GetValue(i)) });
            }
            ViewBag.nationalityName = nationalityList;

            #endregion

            return View("_CustomerPopUp");
        }

        [HttpPost]
        public ActionResult GetCustomer(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.AccountId = id;
            #region Bind Province DropDowm
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE> province = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ProvinceBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }
            ViewBag.Provinces = provincelist;
            #endregion

            #region Gender
            List<SelectListItem> genderList = new List<SelectListItem>();
            Array argender = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Gender));
            genderList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < argender.Length; i++)
            {
                genderList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GenderName[i], Value = System.Convert.ToString((int)argender.GetValue(i)) });
            }
            ViewBag.genderName = genderList;

            #endregion

            #region MaritalStatus
            List<SelectListItem> maritalstatusList = new List<SelectListItem>();
            Array armaritalstatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatus));
            maritalstatusList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < armaritalstatus.Length; i++)
            {
                maritalstatusList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MaritalStatusName[i], Value = System.Convert.ToString((int)armaritalstatus.GetValue(i)) });
            }
            ViewBag.maritalstatusName = maritalstatusList;

            #endregion

            #region Nationality
            List<SelectListItem> nationalityList = new List<SelectListItem>();
            Array arnationality = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.Nationality));
            nationalityList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arnationality.Length; i++)
            {
                nationalityList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.NationalityName[i], Value = System.Convert.ToString((int)arnationality.GetValue(i)) });
            }
            ViewBag.nationalityName = nationalityList;

            #endregion

            CustomerAccountCBE customer = new CustomerAccountCBE();
            customer.AccountId = id;
            customer.TmsId = 1;
            customer = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customer);
            #region ViewBag for DDL Values
            ViewBag.hfProvinceId = customer.ProvinceId;
            ViewBag.hfCityId = customer.CityId;
            ViewBag.hfDistrictId = customer.DistrictId;
            ViewBag.hfSubDistrictId = customer.SubDistrictId;
            ViewBag.PostalCode = customer.PostalCode;
            ViewBag.CustomerDocumentPath = customer.ResidentidcardImagePath;
            ViewBag.Nationality = customer.Nationality;
            ViewBag.Gender = customer.Gender;
            ViewBag.MaritalStatus = customer.MaritalStatus;
            ViewBag.BirthPlace = customer.BirthPlace;
            #endregion
            return View("_CustomerPopUp", customer);
        }

        [HttpPost]
        public JsonResult CustomerAdd(CustomerAccountCBE customerAccount)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {
                    List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();

                    if (!string.IsNullOrEmpty(customerAccount.MobileNo))
                    {
                        customerAccount.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(customerAccount.MobileNo.Trim());
                    }
                    customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == customerAccount.MobileNo.ToString());
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == customerAccount.EmailId.ToString());
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Registrationfiltered = customerDataList.FindAll(x => x.ResidentId == customerAccount.ResidentId);
                    if (Registrationfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Resident Id already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Mobilefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Mobile Number already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Emailfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Email Id already exists";
                        objResponseMessage.Add(objModelState);
                    }

                    #region Document validate
                    var DocumentTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png",
                    "application/pdf",
                    "application/doc",
                    "application/docx",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                    if (string.IsNullOrEmpty(customerAccount.ResidentidcardImagePath))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Resident Card Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else
                    {
                        try
                        {
                            string[] block = customerAccount.ResidentidcardImagePath.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose either GIF, JPG or PNG  image.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"Customer\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = customerAccount.ResidentId.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    customerAccount.ResidentidcardImagePath = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Something went wrong";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong";
                            objResponseMessage.Add(objModelState);
                        }
                    }

                    #endregion

                    if (objResponseMessage.Count == 0)
                    {
                        #region Insert Into Customer Queue
                        customerAccount.CreationDate = DateTime.Now;
                        customerAccount.TmsId = tmsId;
                        customerAccount.RegistartionThrough = 1;
                        customerAccount.EmailId = customerAccount.EmailId.Trim();
                        customerAccount.ResidentId = customerAccount.ResidentId.Trim();
                        if (string.IsNullOrEmpty(customerAccount.BirthPlace))
                        {
                            customerAccount.BirthPlace = string.Empty;
                        }
                        if (string.IsNullOrEmpty(customerAccount.RT))
                            customerAccount.RT = string.Empty;
                        else
                            customerAccount.RT = customerAccount.RT.Trim();
                        if (string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RW = string.Empty;
                        else
                            customerAccount.RW = customerAccount.RW.Trim();
                        customerAccount.Address = customerAccount.Address.Trim();
                        customerAccount.AccountStatus = 1;
                        customerAccount.TransferStatus = 1;
                        customerAccount.IsDocVerified = 1;
                        if (string.IsNullOrEmpty(customerAccount.Occupation))
                            customerAccount.Occupation = string.Empty;

                        if (string.IsNullOrEmpty(customerAccount.RT) && string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RT_RW = string.Empty;
                        else
                            customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;
                        int customerEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Insert(customerAccount);
                        if (customerEntryId > 0)
                        {
                            customerAccount.AccountId = customerEntryId;
                            string responseString = BrodcastDataMobile.SignUp(customerAccount);
                            MobileResponce objMobileResponce = JsonConvert.DeserializeObject<MobileResponce>(responseString);
                            try
                            {
                                if (objMobileResponce.status == "success")
                                {
                                    CustomerAccountBLL.UpdateMobileResponce(objMobileResponce.trans_id, 1, objMobileResponce.message);
                                }
                                else
                                {
                                    CustomerAccountBLL.UpdateMobileResponce(objMobileResponce.trans_id, 2, objMobileResponce.message);
                                }
                            }
                            catch (Exception ex)
                            {
                                HelperClass.LogMessage("Failed to get responce from mobile API" + ex);
                            }

                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "success";
                            objResponseMessage.Add(objModelState);
                        }
                        else
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong";
                            objResponseMessage.Add(objModelState);
                        }

                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerUpdate(CustomerAccountCBE customerAccount, bool ImageChnage)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {

                    int customerId = customerAccount.AccountId;

                    List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
                    #region Mobile No validate by using country code
                    if (!string.IsNullOrEmpty(customerAccount.MobileNo))
                    {
                        customerAccount.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(customerAccount.MobileNo);

                    }
                    #endregion

                    #region Validate duplicate and Image format
                    customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> ResidentIdfiltered = customerDataList.FindAll(x => x.ResidentId == customerAccount.ResidentId && x.AccountId != customerId);
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == customerAccount.MobileNo.ToString() && x.AccountId != customerId);
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == customerAccount.EmailId.ToString() && x.AccountId != customerId);


                    if (ResidentIdfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Resident Id already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Mobilefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Mobile Number already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Emailfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Email Id already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    #region Document validate
                    if (ImageChnage)
                    {

                        var DocumentTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png",
                    "application/pdf",
                    "application/doc",
                    "application/docx",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                        if (string.IsNullOrEmpty(customerAccount.ResidentidcardImagePath))
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Resident Card Image is required";
                            objResponseMessage.Add(objModelState);
                        }
                        else
                        {
                            try
                            {
                                string[] block = customerAccount.ResidentidcardImagePath.Split(';');
                                var contentType = block[0].Split(':')[1];
                                var realData = block[1].Split(',')[1];
                                if (!DocumentTypes.Contains(contentType))
                                {
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Please choose either GIF, JPG or PNG  image.";
                                    objResponseMessage.Add(objModelState);
                                }
                                else
                                {
                                    try
                                    {
                                        string CustomerFilepath = Constants.CustomerImagePath + @"Customer\";
                                        if (!Directory.Exists(CustomerFilepath))
                                        {
                                            Directory.CreateDirectory(CustomerFilepath);
                                        }
                                        string customerImageName = customerAccount.ResidentId.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                        customerAccount.ResidentidcardImagePath = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                    }
                                    catch (Exception ex)
                                    {
                                        HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                                        ModelStateList objModelState = new ModelStateList();
                                        objModelState.ErrorMessage = "Something went wrong";
                                        objResponseMessage.Add(objModelState);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Something went wrong";
                                objResponseMessage.Add(objModelState);
                            }
                        }


                    }
                    #endregion
                    #endregion

                    if (objResponseMessage.Count == 0)
                    {
                        #region Customer Details
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                        customer.AccountId = customerId;
                        customer.TmsId = 1;
                        customer = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customer);
                        #endregion
                        #region Customer Data Process
                        customerAccount.ModificationDate = DateTime.Now;
                        customerAccount.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        customerAccount.TmsId = tmsId;
                        customerAccount.EmailId = customerAccount.EmailId.Trim();
                        customerAccount.ResidentId = customerAccount.ResidentId.Trim();

                        if (string.IsNullOrEmpty(customerAccount.BirthPlace))
                        {
                            customerAccount.BirthPlace = string.Empty;
                        }
                        else
                        {
                            customerAccount.BirthPlace = customerAccount.BirthPlace.Trim();
                        }
                        if (string.IsNullOrEmpty(customerAccount.RT))
                            customerAccount.RT = customer.RT.Trim();


                        if (string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RW = customer.RW.Trim();

                        if (string.IsNullOrEmpty(customerAccount.Occupation))
                            customerAccount.Occupation = string.Empty;

                        customerAccount.Address = customerAccount.Address.Trim();
                        customerAccount.AccountStatus = 1;
                        customerAccount.TransferStatus = 1;
                        customerAccount.IsDocVerified = 1;
                        customerAccount.AccountBalance = customer.AccountBalance;
                        customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;
                        if (string.IsNullOrEmpty(customerAccount.RT) && string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RT_RW = customer.RW.Trim();
                        else
                            customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;
                        CustomerAccountBLL.Update(customerAccount);

                        string responseString = BrodcastDataMobile.UpdateUser(customerAccount);
                        MobileResponce objMobileResponce = JsonConvert.DeserializeObject<MobileResponce>(responseString);
                        try
                        {
                            if (objMobileResponce.status == "success")
                            {
                                CustomerAccountBLL.UpdateMobileResponce(objMobileResponce.trans_id, 1, objMobileResponce.message);
                            }
                            else
                            {
                                CustomerAccountBLL.UpdateMobileResponce(objMobileResponce.trans_id, 2, objMobileResponce.message);
                            }
                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to get responce from mobile API" + ex);
                        }
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetTranscationHistoryByCustomer(int AccountId, int pageindex, int pagesize)
        {
            string Det = "";
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                dt = AccountHistoryBLL.AccountHistoryBYAccountIdLazyLoad(AccountId, pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }

            return Det;
        }

        [HttpPost]
        public JsonResult GetVehicleListByAccount(int AccountId)
        {
            List<CustomerVehicleCBE> customerDataList = new List<CustomerVehicleCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                customerDataList = CustomerVehicleBLL.GetCustomerVehicleByAccountId(AccountId);
                result.Data = customerDataList;

            }

            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehicleListData()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            return PartialView("VihicleList");
        }

        [HttpPost]
        public JsonResult CustomerAccountFilter(CustomerVehicleModel objCustomerVehicleModel)
        {
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                try
                {
                    string strQuery = " WHERE 1=1 ";
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
                    customerDataList = CustomerAccountBLL.GetCustomerAccountFiltered(strQuery);
                    result.Data = customerDataList;
                }
                catch (Exception)
                {

                    result.Data = "failed";
                }

            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Customer Vehicle
        public ActionResult CustomerVehicle()
        {
            List<CustomerVehicleCBE> customerVehicleDataList = new List<CustomerVehicleCBE>();
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Registration", "Vehicle");

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleclassList = new List<SelectListItem>();
            List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
            vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

            vehicleclassList.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleclassDataList)
            {
                vehicleclassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
            }
            ViewBag.VehicleClassList = vehicleclassList;

            #endregion

            #region Queue Status
            List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
            Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));
            customerQueueStatus.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arStatus.Length; i++)
            {
                customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
            }
            ViewBag.QueueStatusList = customerQueueStatus;

            #endregion

            #region Exception Flag
            List<SelectListItem> ExceptionFlagList = new List<SelectListItem>();
            Array ExceptionFlagListart = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlag));
            ExceptionFlagList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < ExceptionFlagListart.Length; i++)
            {
                ExceptionFlagList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlagName[i], Value = System.Convert.ToString((int)ExceptionFlagListart.GetValue(i)) });
            }
            ViewBag.ExceptionFlagList = ExceptionFlagList;

            #endregion

            List<CustomerAccountCBE> SortedList = CustomerAccountBLL.GetAllAsList().OrderBy(o => o.ResidentId).ToList();
            ViewData["CustomerAccount"] = SortedList;//CustomerAccountBLL.GetAllAsList();
            return View();

        }

        [HttpPost]
        public JsonResult CustomerVehicleListScroll(int pageindex, int pagesize)
        {

            List<CustomerVehicleCBE> customerDataList = new List<CustomerVehicleCBE>();
            JsonResult result = new JsonResult();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.CustomerVehicleAccountLazyLoad(pageindex, pagesize);
            result.Data = customerDataList;
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult CustomerVehicleChildList(List<CustomerVehicleCBE> Model)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            return PartialView(Model);
        }

        [HttpGet]
        public ActionResult NewCustomerVehicle()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleclassList = new List<SelectListItem>();
            List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
            vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

            vehicleclassList.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleclassDataList)
            {
                vehicleclassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
            }
            ViewBag.VehicleClassList = vehicleclassList;

            #endregion

            #region Queue Status
            List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
            Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));
            customerQueueStatus.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arStatus.Length; i++)
            {
                customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
            }
            ViewBag.QueueStatusList = customerQueueStatus;

            #endregion

            #region Exception Flag
            List<SelectListItem> ExceptionFlagList = new List<SelectListItem>();
            Array ExceptionFlagListart = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlag));
            ExceptionFlagList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < ExceptionFlagListart.Length; i++)
            {
                ExceptionFlagList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlagName[i], Value = System.Convert.ToString((int)ExceptionFlagListart.GetValue(i)) });
            }
            ViewBag.ExceptionFlagList = ExceptionFlagList;

            #endregion

            #region Fuel Type
            List<SelectListItem> fuelTypeList = new List<SelectListItem>();
            Array arcfuelType = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelType));
            fuelTypeList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arcfuelType.Length; i++)
            {
                fuelTypeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelTypeName[i], Value = System.Convert.ToString((int)arcfuelType.GetValue(i)) });
            }
            ViewBag.FuelTypeList = fuelTypeList;

            #endregion

            #region Licence Plate Color
            List<SelectListItem> licencePlateColorList = new List<SelectListItem>();
            Array arlicencePlateColor = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColor));
            licencePlateColorList.Add(new SelectListItem() { Text = "", Value = "0" });
            for (int i = 0; i < arlicencePlateColor.Length; i++)
            {
                licencePlateColorList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColorName[i], Value = System.Convert.ToString((int)arlicencePlateColor.GetValue(i)) });
            }
            ViewBag.LicencePlateColorList = licencePlateColorList;

            #endregion

            return View("_CustomerVehiclePopUp");
        }

        [HttpPost]
        public ActionResult GetCustomerVehicle(int id)
        {
            CustomerVehicleModel objCustomerVehicleModel = new CustomerVehicleModel();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("Logout", "Login");
                }

                #region Get vehicle Information
                CustomerVehicleCBE vehicle = new CustomerVehicleCBE();
                vehicle.EntryId = id;
                vehicle.TMSId = 1;
                dt = CustomerVehicleBLL.GetCustomerVehicleById_DT(vehicle);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Convert Dt to CBE
                        if (row["TMS_ID"] != DBNull.Value)
                            objCustomerVehicleModel.TMSId = Convert.ToInt32(row["TMS_ID"]);

                        if (row["ENTRY_ID"] != DBNull.Value)
                            objCustomerVehicleModel.EntryId = Convert.ToInt32(row["ENTRY_ID"]);

                        if (row["ACCOUNT_ID"] != DBNull.Value)
                            objCustomerVehicleModel.AccountId = Convert.ToInt32(row["ACCOUNT_ID"]);

                        if (row["VEH_REG_NO"] != DBNull.Value)
                            objCustomerVehicleModel.VehRegNo = Convert.ToString(row["VEH_REG_NO"]);

                        if (row["TAG_ID"] != DBNull.Value)
                            objCustomerVehicleModel.TagId = Convert.ToString(row["TAG_ID"]);

                        if (row["VEHICLE_CLASS_ID"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleClassId = Convert.ToInt32(row["VEHICLE_CLASS_ID"]);

                        if (row["VEHICLE_CLASS_NAME"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleClassName = Convert.ToString(row["VEHICLE_CLASS_NAME"]);

                        if (row["CREATION_DATE"] != DBNull.Value)
                            objCustomerVehicleModel.CreationDate = Convert.ToDateTime(row["CREATION_DATE"]);

                        if (row["MODIFICATION_DATE"] != DBNull.Value)
                            objCustomerVehicleModel.ModificationDate = Convert.ToDateTime(row["MODIFICATION_DATE"]);

                        if (row["MODIFIED_BY"] != DBNull.Value)
                            objCustomerVehicleModel.ModifiedBy = Convert.ToInt32(row["MODIFIED_BY"]);

                        if (row["TRANSFER_STATUS"] != DBNull.Value)
                            objCustomerVehicleModel.TransferStatus = Convert.ToInt32(row["TRANSFER_STATUS"]);

                        if (row["VEHICLE_RC_NO"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleRCNumber = Convert.ToString(row["VEHICLE_RC_NO"]);

                        if (row["OWNER_NAME"] != DBNull.Value)
                            objCustomerVehicleModel.OwnerName = Convert.ToString(row["OWNER_NAME"]);

                        if (row["OWNER_ADDRESS"] != DBNull.Value)
                            objCustomerVehicleModel.OwnerAddress = Convert.ToString(row["OWNER_ADDRESS"]);

                        if (row["BRAND"] != DBNull.Value)
                            objCustomerVehicleModel.Brand = Convert.ToString(row["BRAND"]);

                        if (row["VEHICLE_TYPE"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleType = Convert.ToString(row["VEHICLE_TYPE"]);

                        if (row["VEHICLE_CATEGORY"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleCategory = Convert.ToString(row["VEHICLE_CATEGORY"]);

                        if (row["MODEL_NO"] != DBNull.Value)
                            objCustomerVehicleModel.Model = Convert.ToString(row["MODEL_NO"]);

                        if (row["MANUFACTURING_YEAR"] != DBNull.Value)
                            objCustomerVehicleModel.ManufacturingYear = Convert.ToInt32(row["MANUFACTURING_YEAR"]);

                        if (row["CYCLINDER_CAPACITY"] != DBNull.Value)
                            objCustomerVehicleModel.CyclinderCapacity = Convert.ToString(row["CYCLINDER_CAPACITY"]);

                        if (row["FRAME_NUMBER"] != DBNull.Value)
                            objCustomerVehicleModel.FrameNumber = Convert.ToString(row["FRAME_NUMBER"]);

                        if (row["ENGINE_NUMBER"] != DBNull.Value)
                            objCustomerVehicleModel.EngineNumber = Convert.ToString(row["ENGINE_NUMBER"]);

                        if (row["VEHICLE_COLOR"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleColor = Convert.ToString(row["VEHICLE_COLOR"]);

                        if (row["FUEL_TYPE"] != DBNull.Value)
                        {
                            objCustomerVehicleModel.FuelType = Convert.ToInt32(row["FUEL_TYPE"]);
                            if (Convert.ToInt32(row["FUEL_TYPE"]) > 0)
                                objCustomerVehicleModel.FuelTypeName = Constants.FuelTypeName[Convert.ToInt32(row["FUEL_TYPE"]) - 1];
                        }

                        if (row["LICENCE_PLATE_COLOR"] != DBNull.Value)
                        {
                            objCustomerVehicleModel.LicencePlateColor = Convert.ToInt32(row["LICENCE_PLATE_COLOR"]);
                            if (Convert.ToInt32(row["LICENCE_PLATE_COLOR"]) > 0)
                                objCustomerVehicleModel.LicencePlateColorName = Constants.LicencePlateColorName[Convert.ToInt32(row["LICENCE_PLATE_COLOR"]) - 1];
                        }

                        if (row["REGISTRATION_YEAR"] != DBNull.Value)
                            objCustomerVehicleModel.RegistrationYear = Convert.ToInt32(row["REGISTRATION_YEAR"]);

                        if (row["VEHICLE_OWNERSHIP_NO"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleOwnershipDocumentNumber = Convert.ToString(row["VEHICLE_OWNERSHIP_NO"]);

                        if (row["LOCATION_CODE"] != DBNull.Value)
                            objCustomerVehicleModel.LocationCode = Convert.ToString(row["LOCATION_CODE"]);

                        if (row["REG_QUEUE_NO"] != DBNull.Value)
                            objCustomerVehicleModel.RegistrationQueueNumber = Convert.ToString(row["REG_QUEUE_NO"]);

                        if (row["VEHICLEIMAGE_FRONT"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleImageFront = Convert.ToString(row["VEHICLEIMAGE_FRONT"]);

                        if (row["VEHICLEIMAGE_REAR"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleImageRear = Convert.ToString(row["VEHICLEIMAGE_REAR"]);

                        if (row["VEHICLEIMAGE_RIGHT"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleImageRight = Convert.ToString(row["VEHICLEIMAGE_RIGHT"]);

                        if (row["VEHICLEIMAGE_LEFT"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleImageLeft = Convert.ToString(row["VEHICLEIMAGE_LEFT"]);

                        if (row["VEHICLE_RC_NO_PATH"] != DBNull.Value)
                            objCustomerVehicleModel.VehicleRCNumberImagePath = Convert.ToString(row["VEHICLE_RC_NO_PATH"]);

                        if (row["EXCEPTION_FLAG"] != DBNull.Value)
                        {
                            objCustomerVehicleModel.ExceptionFlag = Convert.ToInt16(row["EXCEPTION_FLAG"]);
                            if (Convert.ToInt32(row["EXCEPTION_FLAG"]) > 0)
                                objCustomerVehicleModel.ExceptionFlagName = Constants.ExceptionFlagName[Convert.ToInt32(row["EXCEPTION_FLAG"]) - 1];
                        }

                        if (row["STATUS"] != DBNull.Value)
                            objCustomerVehicleModel.Status = Convert.ToInt16(row["STATUS"]);

                        if (row["VALID_UNTIL"] != DBNull.Value)
                            objCustomerVehicleModel.ValidUntil = Convert.ToDateTime(row["VALID_UNTIL"]);

                        if (row["TID_FRONT"] != DBNull.Value)
                            objCustomerVehicleModel.TidFront = Convert.ToString(row["TID_FRONT"]);

                        if (row["TID_REAR"] != DBNull.Value)
                            objCustomerVehicleModel.TidRear = Convert.ToString(row["TID_REAR"]);

                        if (row["ACCOUNT_BALANCE"] != DBNull.Value)
                            objCustomerVehicleModel.AccountBalance = Convert.ToDecimal(row["ACCOUNT_BALANCE"]);

                        if (row["REGISTRATION_THROUGH"] != DBNull.Value)
                            objCustomerVehicleModel.RegistartionThrough = Convert.ToInt16(row["REGISTRATION_THROUGH"]);

                        if (row["IS_DOC_VERIFIED"] != DBNull.Value)
                            objCustomerVehicleModel.IsDocVerified = Convert.ToInt16(row["IS_DOC_VERIFIED"]);

                        if (row["QUEUE_STATUS"] != DBNull.Value)
                        {
                            objCustomerVehicleModel.QueueStatus = Convert.ToInt16(row["QUEUE_STATUS"]);
                            if (Convert.ToInt32(row["QUEUE_STATUS"]) > 0)
                                objCustomerVehicleModel.CustomerQueueStatusName = Constants.CustomerQueueStatusName[Convert.ToInt32(row["QUEUE_STATUS"]) - 1];

                        }
                        if (row["CUSTOMER_NAME"] != DBNull.Value)
                            objCustomerVehicleModel.FirstName = Convert.ToString(row["CUSTOMER_NAME"]);

                        if (row["RESIDENT_ID"] != DBNull.Value)
                            objCustomerVehicleModel.ResidentId = Convert.ToString(row["RESIDENT_ID"]);

                        if (row["EMAIL_ID"] != DBNull.Value)
                            objCustomerVehicleModel.EmailId = Convert.ToString(row["EMAIL_ID"]);

                        if (row["MOB_NUMBER"] != DBNull.Value)
                            objCustomerVehicleModel.MobileNo = Convert.ToString(row["MOB_NUMBER"]);

                        if (row["ADDRESS"] != DBNull.Value)
                            objCustomerVehicleModel.Address = Convert.ToString(row["ADDRESS"]);
                        #endregion
                    }
                    #region ViewBag for DDL Values
                    ViewBag.VehicleImageFront = objCustomerVehicleModel.VehicleImageFront;
                    ViewBag.VehicleImageRear = objCustomerVehicleModel.VehicleImageRear;
                    ViewBag.VehicleImageLeft = objCustomerVehicleModel.VehicleImageLeft;
                    ViewBag.VehicleImageRight = objCustomerVehicleModel.VehicleImageRight;
                    ViewBag.VehicleRCNumberImagePath = objCustomerVehicleModel.VehicleRCNumberImagePath;
                    #endregion
                }
                #endregion

                #region Vehicle Class Dropdown
                List<SelectListItem> vehicleclassList = new List<SelectListItem>();
                List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
                vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                vehicleclassList.Add(new SelectListItem() { Text = "", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleclassDataList)
                {
                    vehicleclassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
                }
                ViewBag.VehicleClassList = vehicleclassList;

                #endregion

                #region Queue Status
                List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
                Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));
                customerQueueStatus.Add(new SelectListItem() { Text = "", Value = "0" });
                for (int i = 0; i < arStatus.Length; i++)
                {
                    customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
                }
                ViewBag.QueueStatusList = customerQueueStatus;

                #endregion

                #region Exception Flag
                List<SelectListItem> ExceptionFlagList = new List<SelectListItem>();
                Array ExceptionFlagListart = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlag));
                ExceptionFlagList.Add(new SelectListItem() { Text = "", Value = "0" });
                for (int i = 0; i < ExceptionFlagListart.Length; i++)
                {
                    ExceptionFlagList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlagName[i], Value = System.Convert.ToString((int)ExceptionFlagListart.GetValue(i)) });
                }
                ViewBag.ExceptionFlagList = ExceptionFlagList;

                #endregion

                #region Fuel Type
                List<SelectListItem> fuelTypeList = new List<SelectListItem>();
                Array arcfuelType = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelType));
                fuelTypeList.Add(new SelectListItem() { Text = "", Value = "0" });
                for (int i = 0; i < arcfuelType.Length; i++)
                {
                    fuelTypeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelTypeName[i], Value = System.Convert.ToString((int)arcfuelType.GetValue(i)) });
                }
                ViewBag.FuelTypeList = fuelTypeList;

                #endregion

                #region Licence Plate Color
                List<SelectListItem> licencePlateColorList = new List<SelectListItem>();
                Array arlicencePlateColor = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColor));
                licencePlateColorList.Add(new SelectListItem() { Text = "", Value = "0" });
                for (int i = 0; i < arlicencePlateColor.Length; i++)
                {
                    licencePlateColorList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColorName[i], Value = System.Convert.ToString((int)arlicencePlateColor.GetValue(i)) });
                }
                ViewBag.LicencePlateColorList = licencePlateColorList;

                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("_CustomerVehiclePopUp", objCustomerVehicleModel);
        }

        [HttpPost]
        public JsonResult CustomerVehicleAdd(CustomerVehicleModel objCustomerVehicleModel)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {
                    #region Process Custmer Vehicle Data
                    List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        objCustomerVehicleModel.MobileNo = Constants.MobileNoPrefix(objCustomerVehicleModel.MobileNo.Trim());
                    }

                    objCustomerVehicleModel.AccountId = ValidateCustomerAccount(objCustomerVehicleModel);

                    CustomerVehicleCBE objCustomerVehicleCBE = new CustomerVehicleCBE();
                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        objResponseMessage = ValidateVehicleData(objCustomerVehicleModel, objResponseMessage);
                        if (objResponseMessage.Count == 0)
                        {
                            #region Insert Into Customer Vehicle Data
                            objCustomerVehicleCBE = UpdateVehicleCBE(objCustomerVehicleCBE, objCustomerVehicleModel);
                            objCustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                            objCustomerVehicleCBE.AccountId = objCustomerVehicleModel.AccountId;
                            if (string.IsNullOrEmpty(objCustomerVehicleCBE.TagId))
                                objCustomerVehicleCBE.TagId = string.Empty;
                            int customerVehicleEntryId = CustomerVehicleBLL.Insert(objCustomerVehicleCBE);
                            if (customerVehicleEntryId > 0)
                            {
                                if (objCustomerVehicleModel.SendEmail)
                                {
                                    if (objCustomerVehicleModel.SendEmail)
                                    {
                                        objCustomerVehicleModel.EntryId = customerVehicleEntryId;
                                        BrodcastDataMobile.SendEmail(EmailBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString(), objCustomerVehicleModel.EmailId, "Registrasi Kendaraan Sukses");
                                        BrodcastDataMobile.BroadCastNotification(objCustomerVehicleModel.ResidentId, objCustomerVehicleModel.EntryId.ToString(), "Registrasi Kendaraan Sukses", NotificationBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString());
                                    }
                                }
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "success";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Something went wrong";
                                objResponseMessage.Add(objModelState);
                            }

                            #endregion
                        }
                    }
                    else {
                        objResponseMessage = ValidateCustomerAccountData(objCustomerVehicleModel, objResponseMessage);
                        objResponseMessage = ValidateVehicleData(objCustomerVehicleModel, objResponseMessage);
                        if (objResponseMessage.Count == 0)
                        {
                            #region Data set for Customer Account
                            CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
                            objCustomerAccountCBE.ResidentId = objCustomerVehicleModel.ResidentId.Trim();
                            objCustomerAccountCBE.EmailId = objCustomerVehicleModel.EmailId.Trim();
                            objCustomerAccountCBE.MobileNo = objCustomerVehicleModel.MobileNo.Trim();
                            objCustomerAccountCBE.FirstName = objCustomerVehicleModel.FirstName.Trim();
                            objCustomerAccountCBE.Address = objCustomerVehicleModel.Address.Trim();
                            objCustomerAccountCBE.TmsId = tmsId;
                            objCustomerAccountCBE.TransferStatus = 1;
                            objCustomerAccountCBE.AccountId = 0;
                            objCustomerAccountCBE.AccountStatus = 1;
                            objCustomerAccountCBE.CreationDate = DateTime.Now;
                            objCustomerAccountCBE.IsDocVerified = 1;
                            objCustomerAccountCBE.RegistartionThrough = 1;
                            #endregion

                            #region Insert Customer Data
                            objCustomerVehicleModel.AccountId = CustomerAccountBLL.Insert(objCustomerAccountCBE);
                            #endregion

                            if (objCustomerVehicleModel.AccountId > 0)
                            {
                                #region Insert Into Customer Vehicle Data
                                objCustomerVehicleCBE = UpdateVehicleCBE(objCustomerVehicleCBE, objCustomerVehicleModel);
                                objCustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                                objCustomerVehicleCBE.AccountId = objCustomerVehicleModel.AccountId;
                                if (string.IsNullOrEmpty(objCustomerVehicleCBE.TagId))
                                    objCustomerVehicleCBE.TagId = string.Empty;

                                int customerVehicleEntryId = CustomerVehicleBLL.Insert(objCustomerVehicleCBE);
                                if (customerVehicleEntryId > 0)
                                {

                                    if (objCustomerVehicleModel.SendEmail)
                                    {
                                        objCustomerVehicleModel.EntryId = customerVehicleEntryId;
                                        BrodcastDataMobile.SendEmail(EmailBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString(), objCustomerVehicleModel.EmailId, "Registrasi Kendaraan Sukses");
                                        BrodcastDataMobile.BroadCastNotification(objCustomerVehicleModel.ResidentId, objCustomerVehicleModel.EntryId.ToString(), "Registrasi Kendaraan Sukses", NotificationBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString());
                                    }
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "success";
                                    objResponseMessage.Add(objModelState);
                                }
                                else
                                {
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Something went wrong";
                                    objResponseMessage.Add(objModelState);
                                }

                                #endregion
                            }
                            else {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Something went wrong";
                                objResponseMessage.Add(objModelState);
                            }


                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Customer Vehicle Add Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerVehicleUpdate(CustomerVehicleModel objCustomerVehicleModel)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    CustomerVehicleCBE objCustomerVehicleCBE = new CustomerVehicleCBE();

                    #region Update Customer Mobile No
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        objCustomerVehicleModel.MobileNo = Constants.MobileNoPrefix(objCustomerVehicleModel.MobileNo.Trim());
                    }
                    #endregion

                    #region Check Customer Account exists or not
                    objCustomerVehicleModel.AccountId = ValidateCustomerAccount(objCustomerVehicleModel);

                    #endregion

                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        objResponseMessage = ValidateVehicleData(objCustomerVehicleModel, objResponseMessage);

                        if (objResponseMessage.Count == 0)
                        {
                            #region Update Customer Vehicle Data
                            objCustomerVehicleCBE = UpdateVehicleCBE(objCustomerVehicleCBE, objCustomerVehicleModel);
                            objCustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                            objCustomerVehicleCBE.EntryId = objCustomerVehicleModel.EntryId;
                            objCustomerVehicleCBE.AccountId = objCustomerVehicleModel.AccountId;
                            if (string.IsNullOrEmpty(objCustomerVehicleCBE.TagId))
                                objCustomerVehicleCBE.TagId = string.Empty;
                            CustomerVehicleBLL.Update(objCustomerVehicleCBE);
                            if (objCustomerVehicleModel.SendEmail)
                            {
                                BrodcastDataMobile.SendEmail(EmailBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString(), objCustomerVehicleModel.EmailId, "Registration Vehicle Success");
                                BrodcastDataMobile.BroadCastNotification(objCustomerVehicleModel.ResidentId, objCustomerVehicleModel.EntryId.ToString(), "Registrasi Kendaraan Sukses", NotificationBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString());
                            }

                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "success";
                            objResponseMessage.Add(objModelState);
                            #endregion
                        }
                        else
                        {
                            #region Data set for Customer Account
                            CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
                            objCustomerAccountCBE.ResidentId = objCustomerVehicleModel.ResidentId.Trim();
                            objCustomerAccountCBE.EmailId = objCustomerVehicleModel.EmailId.Trim();
                            objCustomerAccountCBE.MobileNo = objCustomerVehicleModel.MobileNo.Trim();
                            objCustomerAccountCBE.FirstName = objCustomerVehicleModel.FirstName.Trim();
                            objCustomerAccountCBE.Address = objCustomerVehicleModel.Address.Trim();
                            objCustomerAccountCBE.TmsId = tmsId;
                            objCustomerAccountCBE.TransferStatus = 1;
                            objCustomerAccountCBE.AccountId = 0;
                            objCustomerAccountCBE.AccountStatus = 1;
                            objCustomerAccountCBE.CreationDate = DateTime.Now;
                            objCustomerAccountCBE.IsDocVerified = 1;
                            objCustomerAccountCBE.RegistartionThrough = 1;
                            #endregion

                            #region Insert Customer Data
                            objCustomerVehicleModel.AccountId = CustomerAccountBLL.Insert(objCustomerAccountCBE);
                            #endregion

                            if (objCustomerVehicleModel.AccountId > 0)
                            {
                                #region Update Customer Vehicle Data

                                objCustomerVehicleCBE = UpdateVehicleCBE(objCustomerVehicleCBE, objCustomerVehicleModel);
                                objCustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                                objCustomerVehicleCBE.EntryId = objCustomerVehicleModel.EntryId;
                                objCustomerVehicleCBE.AccountId = objCustomerVehicleModel.AccountId;
                                if (string.IsNullOrEmpty(objCustomerVehicleCBE.TagId))
                                    objCustomerVehicleCBE.TagId = string.Empty;
                                CustomerVehicleBLL.Update(objCustomerVehicleCBE);
                                if (objCustomerVehicleModel.SendEmail)
                                {
                                    BrodcastDataMobile.SendEmail(EmailBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString(), objCustomerVehicleModel.EmailId, "Registrasi Kendaraan Sukses");
                                    BrodcastDataMobile.BroadCastNotification(objCustomerVehicleModel.ResidentId, objCustomerVehicleModel.EntryId.ToString(), "Registrasi Kendaraan Sukses", NotificationBody(objCustomerVehicleModel.FirstName, objCustomerVehicleModel.VehRegNo).ToString());
                                }

                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "success";
                                objResponseMessage.Add(objModelState);

                                #endregion
                            }
                            else
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Something went wrong";
                                objResponseMessage.Add(objModelState);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Customer Registration List in Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        private static Int32 ValidateCustomerAccount(CustomerVehicleModel objCustomerVehicleModel)
        {
            Int32 CustomerAccountId = 0;
            CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
            objCustomerAccountCBE.ResidentId = objCustomerVehicleModel.ResidentId.Trim();
            objCustomerAccountCBE.MobileNo = objCustomerVehicleModel.MobileNo.Trim();
            objCustomerAccountCBE.EmailId = objCustomerVehicleModel.EmailId.Trim();
            List<CustomerAccountCBE> CustomerAccountByResident = CustomerAccountBLL.ValidateCustomerAccount(objCustomerAccountCBE);
            if (CustomerAccountByResident.Count > 0)
            {
                CustomerAccountId = CustomerAccountByResident[0].AccountId;
            }
            return CustomerAccountId;


        }

        private static List<ModelStateList> ValidateCustomerAccountData(CustomerVehicleModel objCustomerVehicleModel, List<ModelStateList> objResponseMessage)
        {
            try
            {
                #region Validate Customer Account
                List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
                customerDataList = CustomerAccountBLL.GetAllAsList();
                List<CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == objCustomerVehicleModel.MobileNo.ToString() && x.AccountId != objCustomerVehicleModel.AccountId);
                List<CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == objCustomerVehicleModel.EmailId.ToString().Trim() && x.AccountId != objCustomerVehicleModel.AccountId);
                List<CustomerAccountCBE> Registrationfiltered = customerDataList.FindAll(x => x.ResidentId == objCustomerVehicleModel.ResidentId.Trim() && x.AccountId != objCustomerVehicleModel.AccountId);
                #region Validate Mobile Resident Id and Email Id
                if (Registrationfiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Resident Card already exists.";
                    objResponseMessage.Add(objModelState);
                }
                if (Mobilefiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Mobile phone number already exists.";
                    objResponseMessage.Add(objModelState);
                }
                if (Emailfiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Email address already exists.";
                    objResponseMessage.Add(objModelState);
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objResponseMessage;

        }

        private static List<ModelStateList> ValidateVehicleData(CustomerVehicleModel objCustomerVehicleModel, List<ModelStateList> objResponseMessage)
        {
            try
            {
                #region Vehicle Validation
                List<CustomerVehicleCBE> customerVehicleDataList = new List<CustomerVehicleCBE>();
                customerVehicleDataList = CustomerVehicleBLL.GetAllAsList();
                List<CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == objCustomerVehicleModel.VehRegNo.ToString().ToLower() && x.EntryId != objCustomerVehicleModel.EntryId);
                List<CustomerVehicleCBE> TagIdfiltered = customerVehicleDataList.FindAll(x => x.TagId == objCustomerVehicleModel.TagId.ToString() && x.EntryId != objCustomerVehicleModel.EntryId);
                List<CustomerVehicleCBE> TIDFrontfiltered = customerVehicleDataList.FindAll(x => (x.TidFront == objCustomerVehicleModel.TidFront.ToString() || x.TidRear == objCustomerVehicleModel.TidFront.ToString()) && x.EntryId != objCustomerVehicleModel.EntryId);
                List<CustomerVehicleCBE> TIDRearfiltered = customerVehicleDataList.FindAll(x => (x.TidFront == objCustomerVehicleModel.TidRear.ToString() || x.TidRear == objCustomerVehicleModel.TidRear.ToString()) && x.EntryId != objCustomerVehicleModel.EntryId);
                if (VehRegNofiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Registration Number already exists.";
                    objResponseMessage.Add(objModelState);

                }
                else if (TagIdfiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "EPC already exists.";
                    objResponseMessage.Add(objModelState);

                }
                else if (TIDFrontfiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Front TID already exists.";
                    objResponseMessage.Add(objModelState);

                }
                else if (TIDRearfiltered.Count > 0)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Rear TID already exists.";
                    objResponseMessage.Add(objModelState);

                }
                #region Validate and proces Image
                var DocumentTypes = new string[] { "image/gif", "image/jpg", "image/jpeg", "image/png" };
                #region Front Image
                if (objCustomerVehicleModel.ImageFrontChnage)
                {
                    if (string.IsNullOrEmpty(objCustomerVehicleModel.VehicleImageFront))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Front Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else {
                        try
                        {
                            string[] block = objCustomerVehicleModel.VehicleImageFront.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose valid front vehicle image either GIF, JPG or PNG.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = objCustomerVehicleModel.VehRegNo.ToString().Trim() + "_Document_Front_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    objCustomerVehicleModel.VehicleImageFront = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Convert Front Image" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Invalid Front Image";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Convert Front Image" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Front Image";
                            objResponseMessage.Add(objModelState);
                        }
                    }
                }
                #endregion
                #region Rear Image
                if (objCustomerVehicleModel.ImageRearChnage)
                {
                    if (string.IsNullOrEmpty(objCustomerVehicleModel.VehicleImageRear))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Rear Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else {
                        try
                        {
                            string[] block = objCustomerVehicleModel.VehicleImageRear.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose valid rear vehicle image either GIF, JPG or PNG.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = objCustomerVehicleModel.VehRegNo.ToString().Trim() + "_Document_Rear_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    objCustomerVehicleModel.VehicleImageRear = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Convert Rear Image" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Invalid Rear Image";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Convert Rear Image" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Rear Image";
                            objResponseMessage.Add(objModelState);
                        }
                    }
                }
                #endregion
                #region Left Image
                if (objCustomerVehicleModel.ImageLeftChnage)
                {
                    if (string.IsNullOrEmpty(objCustomerVehicleModel.VehicleImageLeft))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Left Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else {
                        try
                        {
                            string[] block = objCustomerVehicleModel.VehicleImageLeft.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose valid left vehicle image either GIF, JPG or PNG.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = objCustomerVehicleModel.VehRegNo.ToString().Trim() + "_Document_Left_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    objCustomerVehicleModel.VehicleImageLeft = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Convert Left Image" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Invalid Left Image.";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Convert Left Image" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Left Image.";
                            objResponseMessage.Add(objModelState);
                        }
                    }
                }
                #endregion
                #region Right Image
                if (objCustomerVehicleModel.ImageRightChnage)
                {
                    if (string.IsNullOrEmpty(objCustomerVehicleModel.VehicleImageRight))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Right Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else {
                        try
                        {
                            string[] block = objCustomerVehicleModel.VehicleImageRight.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose valid right vehicle image either GIF, JPG or PNG.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = objCustomerVehicleModel.VehRegNo.ToString().Trim() + "_Document_Right_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    objCustomerVehicleModel.VehicleImageRight = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Convert Right Image" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Invalid Right Image.";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Convert Right Image" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Right Image.";
                            objResponseMessage.Add(objModelState);
                        }
                    }
                }
                #endregion
                #region RC Image
                if (objCustomerVehicleModel.RCNumberImageChnage)
                {
                    if (string.IsNullOrEmpty(objCustomerVehicleModel.VehicleRCNumberImagePath))
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Registration Certificate Image Image is required";
                        objResponseMessage.Add(objModelState);
                    }
                    else {
                        try
                        {
                            string[] block = objCustomerVehicleModel.VehicleRCNumberImagePath.Split(';');
                            var contentType = block[0].Split(':')[1];
                            var realData = block[1].Split(',')[1];
                            if (!DocumentTypes.Contains(contentType))
                            {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "Please choose valid registration certificate image vehicle image either GIF, JPG or PNG.";
                                objResponseMessage.Add(objModelState);
                            }
                            else
                            {
                                try
                                {
                                    string CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                                    if (!Directory.Exists(CustomerFilepath))
                                    {
                                        Directory.CreateDirectory(CustomerFilepath);
                                    }
                                    string customerImageName = objCustomerVehicleModel.VehRegNo.ToString().Trim() + "_Document_RC_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpeg";
                                    objCustomerVehicleModel.VehicleRCNumberImagePath = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, realData, customerImageName);
                                }
                                catch (Exception ex)
                                {
                                    HelperClass.LogMessage("Failed to Convert Registration Certificate Image Image" + ex);
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Invalid Registration Certificate Image Image.";
                                    objResponseMessage.Add(objModelState);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            HelperClass.LogMessage("Failed to Convert Registration Certificate Image Image" + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Registration Certificate Image Image.";
                            objResponseMessage.Add(objModelState);
                        }
                    }
                }
                #endregion
                #endregion
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objResponseMessage;
        }

        private static CustomerVehicleCBE UpdateVehicleCBE(CustomerVehicleCBE objCustomerVehicleCBE, CustomerVehicleModel objCustomerVehicleModel)
        {

            objCustomerVehicleCBE.CreationDate = DateTime.Now;
            objCustomerVehicleCBE.ModificationDate = DateTime.Now;

            objCustomerVehicleCBE.TMSId = 1;
            objCustomerVehicleCBE.RegistartionThrough = 1;
            objCustomerVehicleCBE.TransferStatus = 1;
            objCustomerVehicleCBE.AccountId = objCustomerVehicleModel.AccountId;
            objCustomerVehicleCBE.VehicleClassId = objCustomerVehicleModel.VehicleClassId;
            objCustomerVehicleCBE.ExceptionFlag = objCustomerVehicleModel.ExceptionFlag;
            objCustomerVehicleCBE.AccountBalance = objCustomerVehicleModel.AccountBalance;
            objCustomerVehicleCBE.FuelType = objCustomerVehicleModel.FuelType;
            objCustomerVehicleCBE.LicencePlateColor = objCustomerVehicleModel.LicencePlateColor;
            objCustomerVehicleCBE.ManufacturingYear = objCustomerVehicleModel.ManufacturingYear;
            objCustomerVehicleCBE.QueueStatus = objCustomerVehicleModel.QueueStatus;
            objCustomerVehicleCBE.RegistrationYear = objCustomerVehicleModel.RegistrationYear;
            objCustomerVehicleCBE.ValidUntil = objCustomerVehicleModel.ValidUntil;
            objCustomerVehicleCBE.EntryId = objCustomerVehicleModel.EntryId;

            if (objCustomerVehicleModel.Brand != null)
                objCustomerVehicleCBE.Brand = objCustomerVehicleModel.Brand;
            if (objCustomerVehicleModel.CyclinderCapacity != null)
                objCustomerVehicleCBE.CyclinderCapacity = objCustomerVehicleModel.CyclinderCapacity;
            if (objCustomerVehicleModel.EngineNumber != null)
                objCustomerVehicleCBE.EngineNumber = objCustomerVehicleModel.EngineNumber;
            if (objCustomerVehicleModel.FrameNumber != null)
                objCustomerVehicleCBE.FrameNumber = objCustomerVehicleModel.FrameNumber;
            if (objCustomerVehicleModel.LocationCode != null)
                objCustomerVehicleCBE.LocationCode = objCustomerVehicleModel.LocationCode;
            if (objCustomerVehicleModel.Model != null)
                objCustomerVehicleCBE.Model = objCustomerVehicleModel.Model;
            if (objCustomerVehicleModel.OwnerAddress != null)
                objCustomerVehicleCBE.OwnerAddress = objCustomerVehicleModel.OwnerAddress.Trim();
            if (objCustomerVehicleModel.OwnerName != null)
                objCustomerVehicleCBE.OwnerName = objCustomerVehicleModel.OwnerName.Trim();
            if (objCustomerVehicleModel.RegistrationQueueNumber != null)
                objCustomerVehicleCBE.RegistrationQueueNumber = objCustomerVehicleModel.RegistrationQueueNumber;
            if (objCustomerVehicleModel.TagId != null)
                objCustomerVehicleCBE.TagId = objCustomerVehicleModel.TagId.Trim();
            if (objCustomerVehicleModel.TidFront != null)
                objCustomerVehicleCBE.TidFront = objCustomerVehicleModel.TidFront.Trim();
            if (objCustomerVehicleModel.TidRear != null)
                objCustomerVehicleCBE.TidRear = objCustomerVehicleModel.TidRear.Trim();
            if (objCustomerVehicleModel.VehicleCategory != null)
                objCustomerVehicleCBE.VehicleCategory = objCustomerVehicleModel.VehicleCategory;
            if (objCustomerVehicleModel.VehicleColor != null)
                objCustomerVehicleCBE.VehicleColor = objCustomerVehicleModel.VehicleColor;
            if (objCustomerVehicleModel.VehicleImageFront != null)
                objCustomerVehicleCBE.VehicleImageFront = objCustomerVehicleModel.VehicleImageFront;
            if (objCustomerVehicleModel.VehicleImageLeft != null)
                objCustomerVehicleCBE.VehicleImageLeft = objCustomerVehicleModel.VehicleImageLeft;
            if (objCustomerVehicleModel.VehicleImageRear != null)
                objCustomerVehicleCBE.VehicleImageRear = objCustomerVehicleModel.VehicleImageRear;
            if (objCustomerVehicleModel.VehicleImageRight != null)
                objCustomerVehicleCBE.VehicleImageRight = objCustomerVehicleModel.VehicleImageRight;
            if (objCustomerVehicleModel.VehicleRCNumberImagePath != null)
                objCustomerVehicleCBE.VehicleRCNumberImagePath = objCustomerVehicleModel.VehicleRCNumberImagePath;
            if (objCustomerVehicleModel.VehicleType != null)
                objCustomerVehicleCBE.VehicleType = objCustomerVehicleModel.VehicleType;
            if (objCustomerVehicleModel.VehRegNo != null)
                objCustomerVehicleCBE.VehRegNo = objCustomerVehicleModel.VehRegNo.Trim();
            if (objCustomerVehicleModel.VehicleRCNumber != null)
                objCustomerVehicleCBE.VehicleRCNumber = objCustomerVehicleModel.VehicleRCNumber.Trim();
            if (objCustomerVehicleModel.VehicleOwnershipDocumentNumber != null)
                objCustomerVehicleCBE.VehicleOwnershipDocumentNumber = objCustomerVehicleModel.VehicleOwnershipDocumentNumber;
            return objCustomerVehicleCBE;
        }

        [HttpPost]
        public JsonResult GetTagId(CustomerVehicleCBE customerVehicle)
        {
            JsonResult result = new JsonResult();
            result.Data = Constants.VRNToByte(customerVehicle.VehicleClassId, customerVehicle.VehRegNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetTranscationHistoryByCustomereVehicle(int VehicleId, int AccountId, int pageindex, int pagesize)
        {
            string Det = "";
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                dt = AccountHistoryBLL.AccountHistoryBYVehicleIdLazyLoad(AccountId, VehicleId, pageindex, pagesize);
                Det = JsonConvert.SerializeObject(dt, Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
            }

            return Det;
        }

        [HttpPost]
        public JsonResult CustomerVehicleFilter(CustomerVehicleModel objCustomerVehicleModel)
        {
            List<CustomerVehicleCBE> customerDataList = new List<CustomerVehicleCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
            }
            else
            {
                try
                {
                    string strQuery = " WHERE 1=1 ";
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
                    customerDataList = CustomerVehicleBLL.GetCustomerVehicleFiltered(strQuery);
                    result.Data = customerDataList;
                }
                catch (Exception)
                {

                    result.Data = "failed";
                }

            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Customer Address Dropdown Lists

        public ActionResult GetCityList(CustomerAccountCBE customerAccount)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            CityCBE city = new CityCBE();
            city.TmsId = 1;
            city.ProvinceId = customerAccount.ProvinceId;
            List<CityCBE> citys = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CityBLL.GetByProvinceId(city).Cast<CityCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(citys);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDistrictList(CustomerAccountCBE customerAccount)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            DistrictCBE district = new DistrictCBE();
            district.TmsId = 1;
            district.CityId = customerAccount.CityId;
            List<DistrictCBE> districts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.DistrictBLL.GetByCityId(district).Cast<DistrictCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(districts);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSubDistrictList(CustomerAccountCBE customerAccount)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            SubDistrictCBE subdistrict = new SubDistrictCBE();
            subdistrict.TmsId = 1;
            subdistrict.DistrictId = customerAccount.DistrictId;
            List<SubDistrictCBE> subdistricts = SubDistrictBLL.GetByDistrictId(subdistrict).Cast<SubDistrictCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(subdistricts);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion


        public ActionResult TransactionHistory()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            return PartialView("TransactionHistory");
        }


        public class MobileResponce
        {
            public string Apifor { get; set; }
            public Int32 trans_id { get; set; }
            public string status { get; set; }
            public string message { get; set; }
        }

        public string EmailBody(string Name, string VRN)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/assets/custom/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[name]", Name);
            body = body.Replace("[VRN]", VRN);
            return body;

            //StringBuilder sb = new StringBuilder();
            //sb.Append("Hai " + Name + ",");
            //sb.Append("");
            //sb.Append("");
            //sb.Append("");
            //sb.Append("Selamat kendaraan " + VRN + " anda telah kami verifikasi.Kendaraan tersebut sudah bisa digunakan untuk melewati SJBE (Sistem Jalan Berbayar Elektronik).");
            //sb.Append("");
            //sb.Append("");
            //sb.Append("Salam,");
            //sb.Append("SmartERP");
        }

        public string NotificationBody(string Name, string VRN)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Selamat kendaraan " + VRN + " anda telah kami verifikasi.Kendaraan tersebut sudah bisa digunakan untuk melewati SJBE (Sistem Jalan Berbayar Elektronik).");
            return sb.ToString();
        }
    }
}