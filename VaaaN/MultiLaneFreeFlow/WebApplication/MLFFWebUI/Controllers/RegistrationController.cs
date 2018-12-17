using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using static MLFFWebUI.Models.HelperClass;

namespace MLFFWebUI.Controllers
{
    public class RegistrationController : Controller
    {
        int tmsId = 1;
        ResponseMessage objResponse = new ResponseMessage();
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();

        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Customer()
        {
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("Logout", "Login");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Registration", "Customer");
                //customerDataList = CustomerAccountBLL.CustomerAccountLazyLoad(1, 10);
                //customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.CustomerAccountLazyLoad(1, 10);
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                return View(customerDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return View();
            //return View(customerDataList);
        }
        [HttpGet]
        public JsonResult CustomerList()
        {
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            JsonResult result = new JsonResult();
            try
            {

                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Registration", "Customer");
                customerDataList = CustomerAccountBLL.GetAllAsList(); ;
                //customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.CustomerAccountLazyLoad(1, 10);
                //customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                result.Data = customerDataList;

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer in Registration Controller" + ex);
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CustomerData(int pageindex, int pagesize)
        {
            JsonResult result = new JsonResult();
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            customerDataList = CustomerAccountBLL.CustomerAccountLazyLoad(pageindex, pagesize);
            result.Data = customerDataList;
            return Json(result.Data);
        }

        [HttpPost]
        public JsonResult CustomerDataonScroll(int pageindex, int pagesize)
        {
            JsonResult result = new JsonResult();
            System.Threading.Thread.Sleep(1000);
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            customerDataList = CustomerAccountBLL.CustomerAccountLazyLoad(pageindex, pagesize);
            result.Data = customerDataList;
            return Json(result.Data);
        }

        [ChildActionOnly]
        public ActionResult CustomerChildList(List<CustomerAccountCBE> Model)
        {
            return PartialView(Model);
        }

        protected string renderPartialViewtostring(string Viewname, object model)
        {
            if (string.IsNullOrEmpty(Viewname))

                Viewname = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewresult = ViewEngines.Engines.FindPartialView(ControllerContext, Viewname);
                ViewContext viewcontext = new ViewContext(ControllerContext, viewresult.View, ViewData, TempData, sw);
                viewresult.View.Render(viewcontext, sw);
                return sw.GetStringBuilder().ToString();
            }

        }

        public class JsonModel
        {
            public string HTMLString { get; set; }
            public bool NoMoredata { get; set; }
        }

        [HttpPost]
        public ActionResult InfiniteScroll(int pageindex, int RowNo, int pagesize)
        {
            ViewBag.RowNo = RowNo;
            System.Threading.Thread.Sleep(1000);
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.CustomerAccountLazyLoad(pageindex, pagesize);
            JsonModel jsonmodel = new JsonModel();
            jsonmodel.NoMoredata = customerDataList.Count < pagesize;
            jsonmodel.HTMLString = renderPartialViewtostring("CustomerChildList", customerDataList);
            return Json(jsonmodel);
        }

        [HttpGet]
        public ActionResult NewCustomer()
        {
            #region Bind Province DropDowm
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
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

            return View("_CustomerPopUp");
        }

        [HttpPost]
        public ActionResult GetCustomer(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.AccountId = id;
            #region Bind Province DropDowm
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE> province = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ProvinceBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }
            ViewBag.Provinces = provincelist;
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
                        objModelState.ErrorMessage = "Identity Id already exists";
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
                        objModelState.ErrorMessage = "Identity Card Image is required";
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
                                    string customerImageName = customerAccount.ResidentId.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
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
                        if (customerAccount.BirthPlace.Contains("--"))
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
                        if (string.IsNullOrEmpty(customerAccount.RT) && string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RT_RW = string.Empty;
                        else
                            customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;
                        int customerEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Insert(customerAccount);
                        if (customerEntryId > 0)
                        {
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
                        objModelState.ErrorMessage = "Identity Id already exists";
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
                            objModelState.ErrorMessage = "Identity Card Image is required";
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
                                        string customerImageName = customerAccount.ResidentId.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
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
                        customerAccount.BirthPlace = customerAccount.BirthPlace.Trim();
                        if (customerAccount.BirthPlace.Contains("--"))
                        {
                            customerAccount.BirthPlace = string.Empty;
                        }
                        if (string.IsNullOrEmpty(customerAccount.RT))
                            customerAccount.RT = customer.RT.Trim();


                        if (string.IsNullOrEmpty(customerAccount.RW))
                            customerAccount.RW = customer.RW.Trim();

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

        public ActionResult CustomerVehicle()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Registration", "CustomerVehicle");
            return View();

        }


        #region Customer Address Dropdown Lists

        public ActionResult GetCityList(CustomerAccountCBE customerAccount)
        {
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
            SubDistrictCBE subdistrict = new SubDistrictCBE();
            subdistrict.TmsId = 1;
            subdistrict.DistrictId = customerAccount.DistrictId;
            List<SubDistrictCBE> subdistricts = SubDistrictBLL.GetByDistrictId(subdistrict).Cast<SubDistrictCBE>().ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(subdistricts);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

    }
}