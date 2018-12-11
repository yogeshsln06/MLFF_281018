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
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to load Customer Registration List in POS Controller" + ex);
            }
            return View();
        }

        #region Register Customer
        public ActionResult RegisterCustomerList()
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
        //Get
        [HttpGet]
        public ActionResult RegisterCustomer()
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
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

        [HttpPost]
        public ActionResult RegisterCustomer(Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount, FormCollection form)
        {

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
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

                #region Image validate
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png"
                };
                if (customerAccount.CustomerImage != null && customerAccount.CustomerImage.ContentLength > 0)
                {
                    if (!imageTypes.Contains(customerAccount.CustomerImage.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return View("RegisterCustomer");
                    }

                    if (customerAccount.CustomerImage.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return View("RegisterCustomer");
                    }

                    #region ImageProcess
                    string customerImageName = customerAccount.ResidentId.ToString() + "_Profile_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(customerAccount.CustomerImage.FileName).ToLower();


                    #region save image outsside the drive
                    //string filePath = Libraries.CommonLibrary.Constants.CutomerDocuments;
                    //string filename = customerImageName + extension;
                    //customerAccount.CustomerImage.SaveAs(filePath + filename);

                    //filestream fs = new filestream(filepath + filename, filemode.open, fileaccess.read);
                    //binaryreader br = new binaryreader(fs);
                    //byte[] bytes = br.readbytes((int32)fs.length);
                    //br.close();
                    //fs.close();

                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = contenttype;
                    //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    //Response.BinaryWrite(bytes);
                    //Response.Flush();
                    //Response.End();
                    #endregion



                    String uploadFilePath = "\\Attachment\\";

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
                    #endregion
                }
                #endregion

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
                if (customerAccount.ResidentidImage != null && customerAccount.ResidentidImage.ContentLength > 0)
                {
                    if (!DocumentTypes.Contains(customerAccount.ResidentidImage.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return View("RegisterCustomer");
                    }

                    if (customerAccount.ResidentidImage.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return View("RegisterCustomer");
                    }

                    #region Document Process
                    string customerImageName = customerAccount.ResidentId.ToString() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(customerAccount.ResidentidImage.FileName).ToLower();

                    String uploadFilePath = "\\Attachment\\";
                    // create a folder for distinct user -
                    string FolderName = "Customer";
                    string pathWithFolderName = Server.MapPath(uploadFilePath + FolderName);

                    bool folderExists = Directory.Exists(pathWithFolderName);
                    if (!folderExists)
                        Directory.CreateDirectory(pathWithFolderName);

                    if (extension.ToLower() == ".pdf" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == "vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        //string renamedFile = System.Guid.NewGuid().ToString("N");
                        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);
                        customerAccount.ResidentidImage.SaveAs(filePath);
                    }
                    else
                    {
                        using (var img = System.Drawing.Image.FromStream(customerAccount.ResidentidImage.InputStream))
                        {
                            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);

                            // Save large size image, 600 x 600
                            VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new System.Drawing.Size(600, 600), filePath);
                        }
                    }
                    customerAccount.ResidentidcardImagePath = customerImageName + extension;
                    #endregion
                }
                #endregion

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

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
                    TempData["Message"] = "eKTP already exists";
                    return View("RegisterCustomer");
                }
                else if (Mobilefiltered.Count > 0)
                {
                    TempData["Message"] = "Mobile Number already exists";
                    return View("RegisterCustomer");
                }
                else if (Emailfiltered.Count > 0)
                {
                    TempData["Message"] = "Email Id already exists";
                    return View("RegisterCustomer");
                }

                #region Insert Into Customer Queue
                customerAccount.CreationDate = DateTime.Now;
                customerAccount.TmsId = tmsId;
                customerAccount.RegistartionThrough = 1;
                customerAccount.BirthDate = Convert.ToDateTime(form["BirthDate"].ToString());
                customerAccount.ValidUntil = Convert.ToDateTime(form["ValidUntil"].ToString());
                customerAccount.ProvinceId = Convert.ToInt32(form["ProvinceId"].ToString());
                customerAccount.CityId = Convert.ToInt32(form["ddlCityId"].ToString());
                customerAccount.DistrictId = Convert.ToInt32(form["DistrictId"].ToString());
                customerAccount.SubDistrictId = Convert.ToInt32(form["SubDistrictId"].ToString());
                customerAccount.EmailId = customerAccount.EmailId.Trim();
                customerAccount.ResidentId = customerAccount.ResidentId.Trim();
                customerAccount.BirthPlace = customerAccount.BirthPlace.Trim();
                customerAccount.RT = customerAccount.RT.Trim();
                customerAccount.RW = customerAccount.RW.Trim();
                customerAccount.Address = customerAccount.Address.Trim();
                customerAccount.AccountStatus = 1;
                customerAccount.TransferStatus = 1;
                customerAccount.IsDocVerified = 1;
                customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;

                int customerEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Insert(customerAccount);
                if (customerEntryId > 0)
                    TempData["lblerror"] = "Sucessfully created Customer.";
                else
                    TempData["lblerror"] = "Unable to process customer.";
                #endregion
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed to Insert Customer Registration List in POS Controller" + ex);
                TempData["lblerror"] = ex.Message.ToString();
                return View("RegisterCustomer");
            }
            return RedirectToAction("RegisterCustomer");
        }

        [HttpGet]
        public ActionResult RegisterCustomerEdit(int id, string urltoken, string RefPage)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.AccountId = id;
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
            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("POS", "RegisterCustomerEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                customer.AccountId = id;
                customer.TmsId = 1;
                customer = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customer);
                #region ViewBag for DDL Values
                ViewBag.hfGender = customer.Gender;
                ViewBag.hfProvinceId = customer.ProvinceId;
                ViewBag.hfCityId = customer.CityId;
                ViewBag.hfDistrictId = customer.DistrictId;
                ViewBag.hfSubDistrictId = customer.SubDistrictId;
                ViewBag.hfMaritalStatus = customer.MaritalStatus;
                ViewBag.hfNationality = customer.Nationality;
                ViewBag.PostalCode = customer.PostalCode;
                ViewBag.CustomerImagePath = customer.CustomerImagePath;
                ViewBag.CustomerDocumentPath = customer.ResidentidcardImagePath;
                ViewBag.ReferencePage = RefPage;
                #endregion
                return View(customer);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }



        }

        [HttpPost]
        public ActionResult RegisterCustomerEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount, FormCollection form)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            ViewBag.AccountId = customerAccount.AccountId;
            string RedirecttoPage = "tagsalelist";
            RedirecttoPage = Request.Form["ReferencePage"];

            try
            {


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

                int customerId = customerAccount.AccountId;
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
                #region Mobile No validate by using country code
                if (!string.IsNullOrEmpty(customerAccount.MobileNo))
                {
                    customerAccount.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(customerAccount.MobileNo);

                }
                #endregion

                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> ResidentIdfiltered = customerDataList.FindAll(x => x.ResidentId == customerAccount.ResidentId && x.AccountId != customerId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == customerAccount.MobileNo.ToString() && x.AccountId != customerId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == customerAccount.EmailId.ToString() && x.AccountId != customerId);
                if (ResidentIdfiltered.Count > 0)
                {
                    TempData["Message"] = "eKTP already exists";
                    return View("RegisterCustomerEdit");
                }
                if (Mobilefiltered.Count > 0)
                {
                    TempData["Message"] = "Mobile Number already exists";
                    return View("RegisterCustomerEdit");
                }
                else if (Emailfiltered.Count > 0)
                {
                    TempData["Message"] = "Email Id already exists";
                    return View("RegisterCustomerEdit");
                }

                #region Customer Details
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                customer.AccountId = customerId;
                customer.TmsId = 1;
                customer = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customer);
                #endregion

                #region Image validate
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png"
                };
                if (customerAccount.CustomerImage != null && customerAccount.CustomerImage.ContentLength > 0)
                {
                    if (!imageTypes.Contains(customerAccount.CustomerImage.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return View("RegisterCustomer");
                    }

                    if (customerAccount.CustomerImage.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return View("RegisterCustomer");
                    }

                    #region ImageProcess
                    string customerImageName = customerAccount.ResidentId.ToString() + "_Profile_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
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
                    #endregion
                }
                else {
                    customerAccount.CustomerImagePath = customer.CustomerImagePath;
                }
                #endregion

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
                if (customerAccount.ResidentidImage != null && customerAccount.ResidentidImage.ContentLength > 0)
                {
                    if (!DocumentTypes.Contains(customerAccount.ResidentidImage.ContentType))
                    {
                        TempData["lblerror"] = "Please choose either GIF, JPG or PNG  image.";
                        return View("RegisterCustomer");
                    }

                    if (customerAccount.ResidentidImage.ContentLength > 2097152) // about 2 MB
                    {
                        // Notify the user why their file was not uploaded.
                        TempData["lblerror"] = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                        return View("RegisterCustomer");
                    }

                    #region Document Process
                    string customerImageName = customerAccount.ResidentId.ToString() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(customerAccount.ResidentidImage.FileName).ToLower();

                    String uploadFilePath = "\\Attachment\\";
                    // create a folder for distinct user -
                    string FolderName = "Customer";
                    string pathWithFolderName = Server.MapPath(uploadFilePath + FolderName);

                    bool folderExists = Directory.Exists(pathWithFolderName);
                    if (!folderExists)
                        Directory.CreateDirectory(pathWithFolderName);

                    if (extension.ToLower() == ".pdf" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == "vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        //string renamedFile = System.Guid.NewGuid().ToString("N");
                        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);
                        customerAccount.ResidentidImage.SaveAs(filePath);
                    }
                    else
                    {
                        using (var img = System.Drawing.Image.FromStream(customerAccount.ResidentidImage.InputStream))
                        {
                            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", customerImageName, extension);

                            // Save large size image, 600 x 600
                            VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new System.Drawing.Size(600, 600), filePath);
                        }
                    }
                    customerAccount.ResidentidcardImagePath = customerImageName + extension;
                    #endregion
                }
                else {
                    customerAccount.ResidentidcardImagePath = customer.ResidentidcardImagePath;
                }
                #endregion

                #region Customer Data Process
                customerAccount.ModificationDate = DateTime.Now;
                customerAccount.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                customerAccount.TmsId = tmsId;
                customerAccount.BirthDate = Convert.ToDateTime(form["BirthDate"].ToString());
                customerAccount.ValidUntil = Convert.ToDateTime(form["ValidUntil"].ToString());
                customerAccount.ProvinceId = Convert.ToInt32(form["ProvinceId"].ToString());
                customerAccount.CityId = Convert.ToInt32(form["ddlCityId"].ToString());
                customerAccount.DistrictId = Convert.ToInt32(form["DistrictId"].ToString());
                customerAccount.SubDistrictId = Convert.ToInt32(form["SubDistrictId"].ToString());
                customerAccount.EmailId = customerAccount.EmailId.Trim();
                customerAccount.ResidentId = customerAccount.ResidentId.Trim();
                customerAccount.BirthPlace = customerAccount.BirthPlace.Trim();
                customerAccount.RT = customerAccount.RT.Trim();
                customerAccount.RW = customerAccount.RW.Trim();
                customerAccount.Address = customerAccount.Address.Trim();
                customerAccount.AccountStatus = 1;
                customerAccount.TransferStatus = 1;
                customerAccount.IsDocVerified = 1;
                customerAccount.AccountBalance = customer.AccountBalance;
                customerAccount.RT_RW = customerAccount.RT + "/" + customerAccount.RW;

                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Update(customerAccount);

                return Redirect("~/POS/registercustomerlist");
                #endregion
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update user" + ex);
                TempData["lblerror"] = "Failed to update user" + ex.Message.ToString();
                return View("RegisterCustomerEdit");
            }
        }
        #endregion

        #region Customer Address Dropdown Lists

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
        #endregion

        #region Customer Queue
        public ActionResult CustomerQueueList()
        {
            //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

            //try
            //{
            //    if (Session["LoggedUserId"] == null)
            //    {
            //        return RedirectToAction("SessionPage", "Home");
            //    }
            //    ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            //    #region Queue Status
            //    List<SelectListItem> customerQueueStatus = new List<SelectListItem>();
            //    Array arStatus = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatus));

            //    for (int i = 0; i < arStatus.Length; i++)
            //    {
            //        customerQueueStatus.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.CustomerQueueStatusName[i], Value = System.Convert.ToString((int)arStatus.GetValue(i)) });
            //    }
            //    ViewBag.queueStatus = customerQueueStatus;

            //    #endregion

            //    //Call Bll To get all Plaza List
            //    customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList().Where(x => x.QueueStatus != 3).ToList();
            //    //customerDataList =customerDataList.Select(x => x.QueueStatus != 3).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE>().ToList();
            //    return View(customerDataList);

            //}
            //catch (Exception ex)
            //{

            //    HelperClass.LogMessage("Failed to load Customer Queue List in POS Controller" + ex);
            //}
            return View();
        }

        [HttpPost]
        public JsonResult CreateCustomerAppointment(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE objCustomerAppointmentCBE)
        {
            JsonResult result = new JsonResult();
            result.Data = "Failure";
            //try
            //{
            //    objCustomerAppointmentCBE.AppointedById = Convert.ToInt32(Session["LoggedUserId"]);
            //    Int32 entryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAppointmentBLL.Insert(objCustomerAppointmentCBE);
            //    if (entryId > 0)
            //    {
            //        CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
            //        objCustomerAccountCBE.AccountId = objCustomerAppointmentCBE.AccountId;

            //        objCustomerAccountCBE.TmsId = 1;
            //        VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateQueueStatus(objCustomerAccountCBE);
            //        result.Data = "Sucess";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result.Data = "Failure";
            //    HelperClass.LogMessage("Create Customer Appointment :" + ex);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCustomerAppointment(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE objCustomerAppointmentCBE)
        {
            JsonResult result = new JsonResult();
            result.Data = "Failure";
            try
            {
                objCustomerAppointmentCBE.AppointedById = Convert.ToInt32(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAppointmentBLL.Update(objCustomerAppointmentCBE);
                result.Data = "Sucess";
            }
            catch (Exception ex)
            {
                result.Data = "Failure";
                HelperClass.LogMessage("Update Customer Appointment :" + ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerAppointmentList(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE objCustomerAppointmentCBE)
        {
            JsonResult result = new JsonResult();
            result.Data = "Failure";
            try
            {
                List<CustomerAppointmentCBE> CustomerAppointmentList = new List<CustomerAppointmentCBE>();
                CustomerAppointmentList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAppointmentBLL.GetCustomerAppointmentByAccountId(objCustomerAppointmentCBE).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE>().ToList();
                CustomerAppointmentList = CustomerAppointmentList.OrderBy(o => Convert.ToDateTime(o.AppointmentDate)).ToList();
                result.Data = CustomerAppointmentList;
            }
            catch (Exception ex)
            {
                result.Data = "Failure";
                HelperClass.LogMessage("Update Customer Appointment :" + ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Customer Vehicle Section

        public ActionResult CustomerVehicleListbyCustomer(int id, string urltoken)
        {
            try
            {

                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.AccountId = id;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("POS", "CustomerVehicleListbyCustomer", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
                if (token == urltoken)
                {

                    CustomerVehicleCBE customer = new CustomerVehicleCBE();
                    customer.AccountId = id;
                    customer.TMSId = Libraries.CommonLibrary.Constants.GetCurrentTMSId();
                    List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
                    customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetUserByAccountId(customer).Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>().ToList();
                    return View(customerDataList);
                }
                else
                {
                    ViewBag.urlForged = "URL has been modified or forged.";
                    return View();
                }



            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
                return View();
            }

        }
        public ActionResult CustomerVehicleList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
            DataTable dt = new DataTable();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsList();
                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllVehicleinDataTable();
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
            }
            return View(dt);
        }

        private ActionResult CustomerVehicleAdditions()
        {
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleclassDataList = new List<Libraries.CommonLibrary.CBE.VehicleClassCBE>();
            vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

            vehicleclassList.Add(new SelectListItem() { Text = "--Select Vehicle Class--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleclassDataList)
            {
                vehicleclassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
            }

            ViewBag.vehicleclassList = vehicleclassList;

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

            #region Customer Data Dropdown
            List<SelectListItem> customerclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
            customerclassList.Add(new SelectListItem() { Text = "--Select Mobile No--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE cr in customerDataList)
            {
                customerclassList.Add(new SelectListItem() { Text = cr.MobileNo, Value = System.Convert.ToString(cr.AccountId) });
            }
            ViewBag.customerclassList = customerclassList;
            #endregion

            #region Vehicle Type
            List<SelectListItem> typeList = new List<SelectListItem>();
            Array artype = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.VehicleType));

            for (int i = 0; i < artype.Length; i++)
            {
                typeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.VehicleTypeName[i], Value = System.Convert.ToString((int)artype.GetValue(i)) });
            }
            ViewBag.typeName = typeList;

            #endregion

            #region Exception Flag
            List<SelectListItem> ExceptionFlagList = new List<SelectListItem>();
            Array ExceptionFlagListart = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlag));

            for (int i = 0; i < ExceptionFlagListart.Length; i++)
            {
                ExceptionFlagList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlagName[i], Value = System.Convert.ToString((int)ExceptionFlagListart.GetValue(i)) });
            }
            ViewBag.ExceptionFlagType = ExceptionFlagList;

            #endregion

            #region Fuel Type
            List<SelectListItem> fuelTypeList = new List<SelectListItem>();
            Array arcfuelType = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelType));

            for (int i = 0; i < arcfuelType.Length; i++)
            {
                fuelTypeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelTypeName[i], Value = System.Convert.ToString((int)arcfuelType.GetValue(i)) });
            }
            ViewBag.fuelTypeName = fuelTypeList;

            #endregion

            #region Licence Plate Color
            List<SelectListItem> licencePlateColorList = new List<SelectListItem>();
            Array arlicencePlateColor = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColor));

            for (int i = 0; i < arlicencePlateColor.Length; i++)
            {
                licencePlateColorList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColorName[i], Value = System.Convert.ToString((int)arlicencePlateColor.GetValue(i)) });
            }
            ViewBag.licencePlateColorName = licencePlateColorList;

            #endregion
            return View("CustomerVehicleAdd");
        }

        [HttpGet]
        public ActionResult CustomerVehicleAdd()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }

            return CustomerVehicleAdditions();
        }

        [HttpPost]
        public ActionResult CustomerVehicleAdd(CustomerVehicleCBE customerVehicle)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
                }


                #region Save VehicleImages
                bool Value = false;
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png" };
                if (customerVehicle.FrontImage != null)
                {
                    //Front Image
                    customerVehicle.VehicleImageFront = HelperClass.ImageSave(customerVehicle.FrontImage, imageTypes, ref Value, customerVehicle.VehRegNo + "FrontImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Front Image";
                        return CustomerVehicleAdditions();
                    }

                    Value = false;
                }
                //Save Rear Image
                if (customerVehicle.RearImage != null)
                {

                    customerVehicle.VehicleImageRear = HelperClass.ImageSave(customerVehicle.RearImage, imageTypes, ref Value, customerVehicle.VehRegNo + "RearImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Rear Image";
                        return CustomerVehicleAdditions();
                    }

                    Value = false;
                }
                //Save Right Side Image
                if (customerVehicle.RightImage != null)
                {

                    customerVehicle.VehicleImageRight = HelperClass.ImageSave(customerVehicle.RightImage, imageTypes, ref Value, customerVehicle.VehRegNo + "RightImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Right Side Image";
                        return CustomerVehicleAdditions();
                    }
                    Value = false;
                }
                //Save Left Side Image
                if (customerVehicle.LeftImage != null)
                {
                    customerVehicle.VehicleImageLeft = HelperClass.ImageSave(customerVehicle.LeftImage, imageTypes, ref Value, customerVehicle.VehRegNo + "LeftImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Left Side Image";
                        return CustomerVehicleAdditions();
                    }
                }
                if (customerVehicle.VehicleRCNumberImage != null)
                {
                    customerVehicle.VehicleRCNumberImagePath = HelperClass.ImageSave(customerVehicle.VehicleRCNumberImage, imageTypes, ref Value, customerVehicle.VehRegNo + "RCImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Left Side Image";
                        return CustomerVehicleAdditions();
                    }
                }

                #endregion
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
                customerVehicleDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsList();


                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == customerVehicle.VehRegNo.ToString().ToLower());
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> TagIdfiltered = customerVehicleDataList.FindAll(x => x.TagId == customerVehicle.TagId.ToString());
                if (VehRegNofiltered.Count > 0)
                {
                    TempData["Message"] = "Vehicle Registration Number already exists";
                    return CustomerVehicleAdditions();
                }
                else if (TagIdfiltered.Count > 0)
                {
                    TempData["Message"] = "Tag Id already exists";
                    return CustomerVehicleAdditions();
                }
                customerVehicle.CreationDate = DateTime.Now;
                customerVehicle.TMSId = 1;
                customerVehicle.TransferStatus = 1;
                customerVehicle.VehRegNo = customerVehicle.VehRegNo.ToUpper();
                customerVehicle.AccountBalance = 0;
                customerVehicle.IsDocVerified = 2;
                customerVehicle.RegistartionThrough = 1;
                customerVehicle.QueueStatus = 1;
                int userId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Insert(customerVehicle);
                if (userId == 0)
                {
                    TempData["Message"] = "Unable tp process";
                    return CustomerVehicleAdditions();
                }



            }
            catch (Exception)
            {
                ViewBag.SuccessMessage = "Failed to insert record";
                TempData["Message"] = "Failed to insert record";
                return CustomerVehicleAdditions();
            }
            return Redirect("~/POS/CustomerVehicleList");

        }

        private ActionResult CustomerVehicleEditions(CustomerVehicleCBE customerVehicle)
        {
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            #region Customer Account Details
            CustomerAccountCBE customerAccount = new CustomerAccountCBE();
            customerAccount.AccountId = customerVehicle.AccountId;
            customerAccount.TmsId = 1;
            customerAccount = Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customerAccount);
            ViewBag.FirstName = customerAccount.FirstName;
            ViewBag.ResidentId = customerAccount.ResidentId;
            #endregion

            #region Vehicle Class Dropdown
            List<SelectListItem> vehicleclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleclassDataList = new List<Libraries.CommonLibrary.CBE.VehicleClassCBE>();
            vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

            vehicleclassList.Add(new SelectListItem() { Text = "--Select Vehicle Class--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE cr in vehicleclassDataList)
            {
                vehicleclassList.Add(new SelectListItem() { Text = cr.Name, Value = System.Convert.ToString(cr.Id) });
            }

            ViewBag.vehicleclassList = vehicleclassList;

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

            #region Customer Data Dropdown
            List<SelectListItem> customerclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
            customerclassList.Add(new SelectListItem() { Text = "--Select Mobile No--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE cr in customerDataList)
            {
                customerclassList.Add(new SelectListItem() { Text = cr.MobileNo, Value = System.Convert.ToString(cr.AccountId) });
            }
            ViewBag.customerclassList = customerclassList;
            #endregion

            #region Vehicle Type
            List<SelectListItem> typeList = new List<SelectListItem>();
            Array artype = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.VehicleType));

            for (int i = 0; i < artype.Length; i++)
            {
                typeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.VehicleTypeName[i], Value = System.Convert.ToString((int)artype.GetValue(i)) });
            }
            ViewBag.typeName = typeList;

            #endregion

            #region Exception Flag
            List<SelectListItem> ExceptionFlagList = new List<SelectListItem>();
            Array ExceptionFlagListart = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlag));

            for (int i = 0; i < ExceptionFlagListart.Length; i++)
            {
                ExceptionFlagList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.ExceptionFlagName[i], Value = System.Convert.ToString((int)ExceptionFlagListart.GetValue(i)) });
            }
            ViewBag.ExceptionFlagType = ExceptionFlagList;

            #endregion

            #region Fuel Type
            List<SelectListItem> fuelTypeList = new List<SelectListItem>();
            Array arcfuelType = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelType));

            for (int i = 0; i < arcfuelType.Length; i++)
            {
                fuelTypeList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.FuelTypeName[i], Value = System.Convert.ToString((int)arcfuelType.GetValue(i)) });
            }
            ViewBag.fuelTypeName = fuelTypeList;

            #endregion

            #region Licence Plate Color
            List<SelectListItem> licencePlateColorList = new List<SelectListItem>();
            Array arlicencePlateColor = Enum.GetValues(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColor));

            for (int i = 0; i < arlicencePlateColor.Length; i++)
            {
                licencePlateColorList.Add(new SelectListItem() { Text = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LicencePlateColorName[i], Value = System.Convert.ToString((int)arlicencePlateColor.GetValue(i)) });
            }
            ViewBag.licencePlateColorName = licencePlateColorList;

            #endregion


            return View("CustomerVehicleEdit", customerVehicle);
        }

        [HttpGet]
        public ActionResult CustomerVehicleEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.AccountId = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("POS", "CustomerVehicleEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
                CustomerVehicleCBE customerDataList = new CustomerVehicleCBE();


                customerDataList.EntryId = id;
                customerDataList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleById(customerDataList);

                ViewBag.AccountId = customerDataList.AccountId;
                ViewBag.VehicleClassId = customerDataList.VehicleClassId;
                ViewBag.VehicleImageFront = customerDataList.VehicleImageFront;
                ViewBag.VehicleImageRear = customerDataList.VehicleImageRear;
                ViewBag.VehicleImageLeft = customerDataList.VehicleImageLeft;
                ViewBag.VehicleImageRight = customerDataList.VehicleImageRight;
                ViewBag.VehicleRCNumberImage = customerDataList.VehicleRCNumberImagePath;
                return CustomerVehicleEditions(customerDataList);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }



        }

        [HttpPost]
        public ActionResult CustomerVehicleEdit(CustomerVehicleCBE CustomerVehicleCBE)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.EntryId = CustomerVehicleCBE.EntryId;
            CustomerVehicleCBE CustomerVehicleList = new CustomerVehicleCBE();
            CustomerVehicleList.EntryId = CustomerVehicleCBE.EntryId;
            CustomerVehicleList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleById(CustomerVehicleList);
            ViewBag.AccountId = CustomerVehicleList.AccountId;
            ViewBag.VehicleClassId = CustomerVehicleList.VehicleClassId;
            try
            {
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
                customerVehicleDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsList();


                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == CustomerVehicleCBE.VehRegNo.ToString().ToLower() && x.EntryId != CustomerVehicleCBE.EntryId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> TagIdfiltered = customerVehicleDataList.FindAll(x => x.TagId == CustomerVehicleCBE.TagId.ToString() && x.EntryId != CustomerVehicleCBE.EntryId);
                if (VehRegNofiltered.Count > 0)
                {
                    TempData["Message"] = "Vehicle Registration Number already exists";

                    return CustomerVehicleEditions(CustomerVehicleList);
                }
                else if (TagIdfiltered.Count > 0)
                {
                    TempData["Message"] = "Tag Id already exists";

                    return CustomerVehicleEditions(CustomerVehicleList);
                }
                #region Save VehicleImages
                bool Value = false;
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpg",
                    "image/jpeg",
                    "image/png" };
                if (CustomerVehicleCBE.FrontImage != null)
                {
                    //Front Image
                    CustomerVehicleCBE.VehicleImageFront = HelperClass.ImageSave(CustomerVehicleCBE.FrontImage, imageTypes, ref Value, CustomerVehicleCBE.VehRegNo + "FrontImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Front Image";
                        return CustomerVehicleEditions(CustomerVehicleList);
                    }

                    Value = false;
                }
                else {
                    CustomerVehicleCBE.VehicleImageFront = CustomerVehicleList.VehicleImageFront;
                }
                //Save Rear Image
                if (CustomerVehicleCBE.RearImage != null)
                {

                    CustomerVehicleCBE.VehicleImageRear = HelperClass.ImageSave(CustomerVehicleCBE.RearImage, imageTypes, ref Value, CustomerVehicleCBE.VehRegNo + "RearImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Rear Image";
                        return CustomerVehicleEditions(CustomerVehicleList);
                    }

                    Value = false;
                }
                else {
                    CustomerVehicleCBE.VehicleImageRear = CustomerVehicleList.VehicleImageRear;
                }
                //Save Right Side Image
                if (CustomerVehicleCBE.RightImage != null)
                {

                    CustomerVehicleCBE.VehicleImageRight = HelperClass.ImageSave(CustomerVehicleCBE.RightImage, imageTypes, ref Value, CustomerVehicleCBE.VehRegNo + "RightImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Right Side Image";
                        return CustomerVehicleEditions(CustomerVehicleList);
                    }
                    Value = false;
                }
                else {
                    CustomerVehicleCBE.VehicleImageRight = CustomerVehicleList.VehicleImageRight;
                }
                //Save Left Side Image
                if (CustomerVehicleCBE.LeftImage != null)
                {
                    CustomerVehicleCBE.VehicleImageLeft = HelperClass.ImageSave(CustomerVehicleCBE.LeftImage, imageTypes, ref Value, CustomerVehicleCBE.VehRegNo + "LeftImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Left Side Image";
                        return CustomerVehicleEditions(CustomerVehicleList);
                    }
                }
                else {
                    CustomerVehicleCBE.VehicleImageLeft = CustomerVehicleList.VehicleImageLeft;
                }

                if (CustomerVehicleCBE.VehicleRCNumberImage != null)
                {
                    CustomerVehicleCBE.VehicleRCNumberImagePath = HelperClass.ImageSave(CustomerVehicleCBE.VehicleRCNumberImage, imageTypes, ref Value, CustomerVehicleCBE.VehRegNo + "RCImage");
                    if (Value)
                    {
                        TempData["Message"] = "Please choose either GIF, JPG or PNG  image Or Image less than 2mb size in Vehicle Left Side Image";
                        return CustomerVehicleEditions(CustomerVehicleList);
                    }
                }
                else {
                    CustomerVehicleCBE.VehicleRCNumberImagePath = CustomerVehicleList.VehicleRCNumberImagePath;
                }
                #endregion
                CustomerVehicleCBE.ModificationDate = DateTime.Now;
                CustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                CustomerVehicleCBE.TMSId = CustomerVehicleList.TMSId;
                CustomerVehicleCBE.TransferStatus = CustomerVehicleList.TransferStatus;
                CustomerVehicleCBE.VehRegNo = CustomerVehicleCBE.VehRegNo.ToUpper();
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Update(CustomerVehicleCBE);
                return Redirect("~/POS/CustomerVehicleList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update user" + ex);
                TempData["Message"] = "Unable to process the request.";
                return CustomerVehicleEditions(CustomerVehicleList);
            }
        }

        [HttpGet]
        public JsonResult GetUserData()
        {
            JsonResult result = new JsonResult();
            List<SelectListItem> customerclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
            var jsonSerialiser = new JavaScriptSerializer();
            result.Data = jsonSerialiser.Serialize(customerDataList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        [HttpGet]
        public ActionResult CustomerTagList(int id)//here id is account id
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("SessionPage", "Home");
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
                    return RedirectToAction("SessionPage", "Home");
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
                    accountHistory.Amount = customerVehicle.AccountBalance;//From Form 
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
                    return RedirectToAction("SessionPage", "Home");
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

        #region Customer Details and Transcation
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

                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.AccountHistoryBYAccountId(id, 2);
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

        //CustomerDocumentUpload()

        //{
        //    string filePath = "C:\\images\\";

        //    string filename = Request.QueryString["FileName"];

        //    string contenttype = "image/" +

        //    Path.GetExtension(Request.QueryString["FileName"].Replace(".", "");

        //    FileStream fs = new FileStream(filePath + filename,

        //    FileMode.Open, FileAccess.Read);

        //    BinaryReader br = new BinaryReader(fs);

        //    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

        //    br.Close();

        //    fs.Close();



        //    //Write the file to response Stream

        //    Response.Buffer = true;

        //    Response.Charset = "";

        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //    Response.ContentType = contenttype;

        //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);

        //    Response.BinaryWrite(bytes);

        //    Response.Flush();

        //    Response.End();

        //}

        [HttpPost]
        public JsonResult GetTagId(Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle)
        {
            JsonResult result = new JsonResult();
            result.Data = Libraries.CommonLibrary.Constants.VRNToByte(customerVehicle.VehicleClassId, customerVehicle.VehRegNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}