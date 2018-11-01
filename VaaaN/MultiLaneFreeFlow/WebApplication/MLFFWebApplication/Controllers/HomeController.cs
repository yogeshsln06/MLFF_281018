using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.Common;
using VaaaN.MLFF.Libraries.CommonLibrary.MSMQ;
using VaaaN.MLFF.WebApplication.Models;

namespace VaaaN.MLFF.WebApplication
{

    public class HomeController : Controller
    {
        static MessageQueue eventQueue;
        //static System.Messaging.MessageQueue eventQueue;

        public HomeController()
        {

            //eventQueue = Queue.Create(Queue.eventQueue);
        }

        #region Login and Logout Section
        //public ActionResult Login()
        //{
        //    return View();
        //}

        public JsonResult DoLogin(string username, string password, string LanguageAbbreviation)
        {

            JsonResult result = new JsonResult();
            try
            {
                if (!(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)))
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
                    user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.ValidateUser(username, password);

                    if (user != null)
                    {
                        if (user.user_status != true)
                        {
                            result.Data = "Inactive";
                        }
                        else if (user.AccountExpiryDate <= DateTime.Now)
                        {
                            result.Data = "Expired";
                        }
                        else
                        {
                            Session["LoggedUserId"] = user.UserId;
                            Session["RoleId"] = user.RoleId;
                            Session["UserName"] = user.FirstName + " " + user.LastName;
                            Session["LastLogin"] = user.UserId;
                            Session["RoleName"] = user.RoleName;
                            Session["ProjectName"] = "MLFF";

                            // save the login time in to db
                            int login_Id = 0;
                            login_Id = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.User_Insert_LoginInfo(user.UserId);

                            Session["LoginId"] = login_Id;

                            result.Data = "Ok";
                            HelperClass.LogMessage("User validated successfully.");

                            #region Cookie For Language
                            if (LanguageAbbreviation != null)
                            {
                                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbreviation);
                                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbreviation);
                            }
                            HttpCookie cookie = new HttpCookie("Language");
                            cookie.Value = LanguageAbbreviation;
                            Response.Cookies.Add(cookie);
                            #endregion

                            RedirectToAction("Userlist", "Home");
                        }
                    }
                    else
                    {
                        result.Data = "Error";
                        HelperClass.LogMessage("Invalid User Id or Password or User not found.");
                    }
                }
                else
                {

                    result.Data = "Error";
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to validate user." + ex.Message);
                result.Data = "Error";

            }
            return result;
        }

        public JsonResult validateLoginName(string loginName, string email_id)
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.ValidateLoginName(loginName, email_id);
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to validate login name" + ex);
            }
            return result;
        }

        public ActionResult Logout()
        {
            try
            {
                if (Session["LoginId"] != null && Session["LoggedUserId"] != null)
                {
                    int atmsId = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentTMSId();

                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.User_update_LoginInfo(Convert.ToInt32(Session["LoggedUserId"]), Convert.ToInt32(Session["LoginId"]));

                    HelperClass.LogMessage("Logout time updated");
                }
            }
            catch
            {
                HelperClass.LogMessage("Unable to update Logout time");
            }
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            TempData["ProjectName"] = "Parking Mangement System";
            return View();
        }
        #endregion

        public ActionResult HomePage()
        {
            return View();
        }

        #region------------UserDashboard--------
        public ActionResult UserDashboard()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }

        #endregion

        #region LiveEvent
        public ActionResult LiveMonitoringList()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }

        //public JsonResult GetMSMQLiveEvent()
        //{
        //    JsonResult result = new JsonResult();

        //    try
        //    {
        //        //eventQueue = Queue.Create(Queue.eventQueue);
        //        if (eventQueue.GetAllMessages().Count() > 0)
        //        {
        //            #region Read Message from Queue
        //            Message m = eventQueue.Receive();
        //            m.Formatter = new BinaryMessageFormatter();
        //            if (m != null)
        //            {
        //                m.Formatter = new BinaryMessageFormatter();
        //                if (m.Body != null)
        //                {
        //                    #region Processing packets
        //                    if (m.Body is CrossTalkPacket)
        //                    {
        //                        #region CrossTalk packet
        //                        CrossTalkPacket crossTalkPacket = (CrossTalkPacket)m.Body;
        //                        if (crossTalkPacket.Payload != null)
        //                        {
        //                            if (crossTalkPacket.Payload is CrossTalkPacketCBE)
        //                            {
        //                                CrossTalkPacketCBE ctp = crossTalkPacket.Payload;
        //                                //response = Request.CreateResponse(HttpStatusCode.OK, ctp);
        //                                result.Data = ctp;
        //                            }
        //                            //else
        //                            //{

        //                            //    Log("GetMSMQLiveEvent CT:Payload is not of transaction type.");
        //                            //}
        //                        }
        //                        //else
        //                        //{
        //                        //    Log("GetMSMQLiveEvent CT:Transaction packet's payload is null.");
        //                        //}
        //                        #endregion
        //                    }
        //                    else if (m.Body is NodeFluxPacket)
        //                    {
        //                        #region NodeFlux packet

        //                        NodeFluxPacket nfp = (NodeFluxPacket)m.Body;

        //                        if (nfp.Payload != null)
        //                        {
        //                            if (nfp.Payload is NodeFluxPacketCBE)
        //                            {
        //                                NodeFluxPacketCBE ntp = nfp.Payload;
        //                                result.Data = ntp;
        //                                //response = Request.CreateResponse(HttpStatusCode.OK, ntp);
        //                            }
        //                            //else
        //                            //{
        //                            //    Log("GetMSMQLiveEvent NF:Payload is not of entry lane transaction type.");
        //                            //}
        //                        }
        //                        //else
        //                        //{
        //                        //    Log("GetMSMQLiveEvent NF:Transaction packet's payload is null.");
        //                        //}
        //                        #endregion
        //                    }
        //                    #endregion
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {

        //            result.Data = "No Data found";

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result.Data = ex.Message.ToString();
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public string Transaction_LiveData()
        {
            string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_LiveData(), Formatting.Indented);
            return Det.Replace("\r", "").Replace("\n", "");
            //return Json(VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.Transaction_LiveData(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region---------------MLFFRealTime-----------
        public ActionResult MLFFRealTime()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }
        #endregion

        #region---------------TransactionAudit-----------
        public ActionResult TransactionAudit()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }

            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }
        #endregion

        #region-------Report-----------
        public ActionResult Report()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            return View();
        }
        #endregion

        #region User Section
        public JsonResult ValidateUser(string email, string password)
        {
            JsonResult result = new JsonResult();

            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE UserCBE = new Libraries.CommonLibrary.CBE.UserCBE();

            // Call the function which check the user credential in db
            VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult LoginResult = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LoginBLL.ValidateUser(email, password, ref UserCBE);
            if (LoginResult.ToString() == "Successful")
            {
                result.Data = "valid";
                Session["LoggedUserId"] = 1;
            }
            else
            {
                result.Data = "Invalid";
            }

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }

        [HttpGet]
        public ActionResult UsersList()
        {

            Int32 RecordCount = 0;
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                int userId = Convert.ToInt32(Session["LoggedUserId"]);

                VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info = new VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo();
                //info.PageSize = 5;
                info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);

                info.CurrentPageIndex = (TempData["currentpage"] == null || Convert.ToString(Session["currentmodule"]) != "user") ? 0 : Convert.ToInt32(TempData["currentpage"]);

                TempData["currentpage"] = info.CurrentPageIndex;

                info.SearchText = "";

                IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> userList;
                userList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetAll(info, ref RecordCount);
                info.RecordCount = RecordCount;

                // Calculate the Page Count
                info.PageCount = Convert.ToInt32(Math.Ceiling(((double)RecordCount / info.PageSize)));
                if (info.PageCount == 0) info.PageCount = 1;
                ViewBag.SortingPagingInfo = info;

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE submodule_right = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE();
                submodule_right = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubModuleActivityRightBLL.GetSubModuleRightByUserIdandSubmoduleId(userId, 1);// 1 for submodule id



                if (submodule_right.SubModuleEdit == false)
                {
                    ViewBag.Visibility = "visibility:hidden";
                }
                else
                {
                    ViewBag.Visibility = "visibility:visible";
                }

                Session["currentmodule"] = "user";
                return View(userList);
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to open userlist page..Check user submodule rights" + ex);
                TempData["Failedmsg"] = "<script>alert('Failed to login.Try again later..');</script>";
                return Redirect("~/Home/SessionPage");
            }
        }

        [HttpPost]
        public ActionResult UsersList(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, string SearchContent, string ddlPagesize)
        {
            Int32 RecordCount = 0;
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);
            info.SearchText = SearchContent;

            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> userList;
            userList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetAll(info, ref RecordCount);

            // Calculate the Page Count
            info.PageCount = Convert.ToInt16(Math.Ceiling(((double)RecordCount / info.PageSize)));
            if (info.PageCount == 0) info.PageCount = 1;
            info.RecordCount = RecordCount;
            ViewBag.SortingPagingInfo = info;

            TempData["currentpage"] = info.CurrentPageIndex;

            return View(userList);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
            return View(user);

        }

        [HttpPost]
        public ActionResult AddUser(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }

                // JsonResult data = new JsonResult();
                string strResult = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.ValidateLoginName(user.LoginName, user.EmailId);

                if (strResult == "Login name found")
                {
                    TempData["Message"] = "Login Name already exists";
                    ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });

                    return View("AddUser");
                }
                else if (strResult == "Email Id found")
                {
                    TempData["Message"] = "Email Id already exists";

                    ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });

                    return View("AddUser");
                }
                else if (strResult == "Both found")
                {
                    TempData["Message"] = "Login name and Email Id already exists";
                    ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });

                    return View("AddUser");
                }
                else
                {
                    var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png",
                    "application/pdf"
                };


                    //-------------------Validating image-----------------------------

                    //if (user.UserProfilePic != null && user.UserProfilePic.ContentLength > 0)
                    //{
                    //    if (!imageTypes.Contains(user.UserProfilePic.ContentType))
                    //    {
                    //        ViewBag.SuccessMessage = "Please choose either a pdf, GIF, JPG or PNG  image.";
                    //        return View();
                    //    }

                    //    if (user.UserProfilePic.ContentLength > 2097152) // about 2 MB
                    //    {
                    //        // Notify the user why their file was not uploaded.
                    //        ViewBag.SuccessMessage = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                    //        return View(user);
                    //    }
                    //}


                    int userId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.Insert(user);

                    //if (user.UserProfilePic != null && user.UserProfilePic.ContentLength > 0)
                    //{
                    //    string userImageName = userId.ToString() + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    //    string extension = System.IO.Path.GetExtension(user.UserProfilePic.FileName).ToLower();

                    //    String uploadFilePath = "\\Attachment\\";
                    //    // create a folder for distinct user -
                    //    string userFolderName = userId.ToString();
                    //    string pathWithFolderName = Server.MapPath(uploadFilePath + userFolderName);

                    //    bool folderExists = Directory.Exists(pathWithFolderName);
                    //    if (!folderExists)
                    //        Directory.CreateDirectory(pathWithFolderName);

                    //    if (extension.ToLower() == ".pdf")
                    //    {
                    //        //string renamedFile = System.Guid.NewGuid().ToString("N");
                    //        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", userImageName, extension);
                    //        user.UserProfilePic.SaveAs(filePath);
                    //    }
                    //    else
                    //    {
                    //        using (var img = System.Drawing.Image.FromStream(user.UserProfilePic.InputStream))
                    //        {
                    //            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", userImageName, extension);

                    //            // Save large size image, 600 x 600
                    //            VaaaN.PMS.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new Size(600, 600), filePath);
                    //        }
                    //    }
                    //    user.UserId = userId;
                    //    user.UserProfilePicName = userImageName + extension;
                    //    VaaaN.PMS.CommonLibrary.BLL.UsersBLL.Update(user);


                    //}
                }
            }
            catch (Exception)
            {
                ViewBag.SuccessMessage = "Failed to insert record";
            }
            return Redirect("~/Home/UsersList");

        }
        [HttpGet]
        public ActionResult EditUser(int id, string name, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Username = name;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            // ViewBag.DynamicMainMenu = CreateMainMenu(Convert.ToInt16(Session["LoggedUserId"]));
            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "EditUser", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
                user.UserId = id;
                user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserById(user);
                ViewBag.UserId = id;
                ViewBag.RoleId = new SelectList(VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll(), "RoleId", "RoleName", user.RoleId);
                ViewBag.UserProfilePicName = user.UserProfilePicName;
                TempData["UserProfilePicName"] = user.UserProfilePicName;
                return View(user);
            }
            else
            {
                ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }

            //String uploadFilePath = "\\Attachment\\";
            //string imagepath = user.UserId.ToString();
            //string url = Server.MapPath(uploadFilePath + imagepath);
            //ViewBag.ImagePath = url;

        }

        [HttpPost]
        public ActionResult EditUser(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.UserId = user.UserId;
            //user.UserProfilePicName = Convert.ToString(TempData["UserProfilePicName"]);
            try
            {
                //    var imageTypes = new string[]{
                //        "image/gif",
                //        "image/jpeg",
                //        "image/pjpeg",
                //        "image/png",
                //        "application/pdf"
                //    };


                //-------------------Validating image-----------------------------

                //if (user.UserProfilePic != null && user.UserProfilePic.ContentLength > 0)
                //{
                //    if (!imageTypes.Contains(user.UserProfilePic.ContentType))
                //    {
                //        ViewBag.SuccessMessage = "Please choose either a pdf, GIF, JPG or PNG  image.";
                //        return View();
                //    }

                //    if (user.UserProfilePic.ContentLength > 2097152) // about 2 MB
                //    {
                //        // Notify the user why their file was not uploaded.
                //        ViewBag.SuccessMessage = "Attachment file can not uploaded because it exceeds the 2 MB size limit.";
                //        return View(user);
                //    }
                //}
                int userId = user.UserId;

                //if (user.UserProfilePic != null && user.UserProfilePic.ContentLength > 0)
                //{
                //    string userImageName = userId.ToString() + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                //    string extension = System.IO.Path.GetExtension(user.UserProfilePic.FileName).ToLower();

                //    String uploadFilePath = "\\PMSAdmin\\Attachment\\";
                //    // create a folder for distinct user -
                //    string userFolderName = userId.ToString();
                //    string pathWithFolderName = Server.MapPath(uploadFilePath + userFolderName);
                //    VaaaN.PMS.CommonLibrary.Common.CommonClass.LogMessage("Path for saving image" + pathWithFolderName);

                //    bool folderExists = Directory.Exists(pathWithFolderName);
                //    if (!folderExists)
                //        Directory.CreateDirectory(pathWithFolderName);

                //    if (extension.ToLower() == ".pdf")
                //    {
                //        //string renamedFile = System.Guid.NewGuid().ToString("N");
                //        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", userImageName, extension);
                //        user.UserProfilePic.SaveAs(filePath);
                //    }
                //    else
                //    {
                //        using (var img = System.Drawing.Image.FromStream(user.UserProfilePic.InputStream))
                //        {
                //            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", userImageName, extension);

                //            // Save large size image, 600 x 600
                //            VaaaN.PMS.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new Size(600, 600), filePath);
                //        }
                //    }
                //    user.UserId = userId;
                //    user.UserProfilePicName = userImageName + extension;


                //}
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.Update(user);
                return Redirect("~/Home/UsersList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update user" + ex);
                ViewBag.RoleId = new SelectList(VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll(), "RoleId", "RoleName", user.RoleId);
                return View(user);
            }
        }
        #endregion

        #region Hardware Section
        public ActionResult HardwareList()
        {

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> hardwareDataList = new List<Libraries.CommonLibrary.CBE.HardwareCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                hardwareDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE>().ToList();

                return View(hardwareDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Hardware List " + ex.Message.ToString());
            }
            return View(hardwareDataList);
        }

        [HttpGet]
        public ActionResult HardwareAdd()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

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

            #region Hardware Type
            ViewBag.HardwareType = HelperClass.GetHardwareType();
            #endregion

            #region Hardware Position
            ViewBag.HardwarePosition = HelperClass.GetHardwarePosition();
            #endregion


            // ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE();

            return View(hardware);

        }

        [HttpPost]
        public ActionResult HardwareAdd(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                hardware.TMSId = 1;
                hardware.TransferStatus = 1;
                hardware.CreationDate = DateTime.Now;
                int userId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.Insert(hardware);

            }
            catch (Exception ex)
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
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

                #region Hardware Type
                ViewBag.HardwareType = HelperClass.GetHardwareType();
                #endregion

                #region Hardware Position
                ViewBag.HardwarePosition = HelperClass.GetHardwarePosition();
                #endregion
                HelperClass.LogMessage("Failed To insert Hardware " + ex.Message.ToString());
                return View("HardwareAdd");
            }
            return Redirect("~/Home/HardwareList");

        }

        [HttpGet]
        public ActionResult HardwareEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.HardwareId = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "HardwareEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
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

                #region Hardware Type
                ViewBag.HardwareType = HelperClass.GetHardwareType();
                #endregion

                #region Hardware Position
                ViewBag.HardwarePosition = HelperClass.GetHardwarePosition();
                #endregion
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE();
                hardware.HardwareId = id;
                hardware = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetHardwareById(hardware);
                ViewBag.hfPlazaId = hardware.PlazaId;
                ViewBag.hfHardwareType = hardware.HardwareType;
                ViewBag.hfHardwarePosition = hardware.HardwarePosition;
                return View(hardware);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }



        }

        [HttpPost]
        public ActionResult HardwareEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.HardwareId = hardware.HardwareId;
            try
            {
                hardware.TMSId = 1;
                hardware.TransferStatus = 1;
                hardware.ModificationDate = DateTime.Now;
                hardware.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.Update(hardware);
                return Redirect("~/Home/HardwareList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update Hardware" + ex);
                ViewBag.HardwareId = hardware.HardwareId;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

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

                #region Hardware Type
                ViewBag.HardwareType = HelperClass.GetHardwareType();
                #endregion

                #region Hardware Position
                ViewBag.HardwarePosition = HelperClass.GetHardwarePosition();
                #endregion
                hardware = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetHardwareById(hardware);
                ViewBag.hfPlazaId = hardware.PlazaId;
                ViewBag.hfHardwareType = hardware.HardwareType;
                ViewBag.hfHardwarePosition = hardware.HardwarePosition;
                return View(hardware);
            }
        }
        #endregion

        #region Plaza Section
        public ActionResult PlazaList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plazaDataList = new List<Libraries.CommonLibrary.CBE.PlazaCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                plazaDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

                return View(plazaDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Plaza List " + ex.Message.ToString());
            }
            return View(plazaDataList);
        }

        //Get
        public ActionResult PlazaAdd()
        {
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Add Plaza Get Method" + ex);
            }
            return View();
        }

        [HttpPost]
        public ActionResult PlazaAdd(Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            try
            {
                plaza.TmsId = 1;
                plaza.CreationDate = DateTime.Now;
                Libraries.CommonLibrary.BLL.PlazaBLL.Insert(plaza);
                TempData["Message"] = "Successfully Created New Gantry";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Gantry Not Created";
                HelperClass.LogMessage("Failed To Add Plaza Post Method" + ex);
            }
            return Redirect("~/Home/PlazaList");
        }


        [HttpGet]
        public ActionResult PlazaEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.PlazaId = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "PlazaEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE();
                plaza.PlazaId = id;
                plaza.TmsId = 1;
                plaza.ModificationDate = DateTime.Now;
                plaza.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetPlazaById(plaza);

                return View(plaza);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }




        }

        [HttpPost]
        public ActionResult PlazaEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.PlazaId = plaza.PlazaId;
            try
            {
                plaza.TmsId = 1;
                plaza.ModificationDate = DateTime.Now;
                plaza.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.Update(plaza);
                return Redirect("~/Home/PlazaList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update Hardware" + ex);
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetPlazaById(plaza);
                return View(plaza);
            }
        }
        #endregion

        #region Lane Section
        public ActionResult LanebyPlazaList(int id, string urltoken)
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDataList = new List<Libraries.CommonLibrary.CBE.LaneCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "LanebyPlazaList", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
                if (token == urltoken)
                {

                    LaneCBE lane = new LaneCBE();
                    lane.PlazaId = id;
                    ViewBag.PlazaId = id;
                    laneDataList = Libraries.CommonLibrary.BLL.LaneBLL.GetLaneByPlazaId(lane).Cast<LaneCBE>().ToList();
                    return View(laneDataList);

                }
                else
                {
                    ViewBag.urlForged = "URL has been modified or forged.";
                    return View();
                }


            }
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View(laneDataList);
        }

        private ActionResult LaneAdditions(int id)
        {
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> hardwareDataList = new List<Libraries.CommonLibrary.CBE.HardwareCBE>();

            hardwareDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE>().ToList();
            ViewBag.PlazaId = id;
            #region ANPR Dropdown
            List<SelectListItem> anprList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> ANPRfiltered = hardwareDataList.FindAll(x => x.HardwareType == 1);
            anprList.Add(new SelectListItem() { Text = "--Select ANPR--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE h in ANPRfiltered)
            {
                anprList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }

            ViewBag.ANPR = anprList;

            #endregion

            #region RFID Dropdown
            List<SelectListItem> rfidList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> RFIDfiltered = hardwareDataList.FindAll(x => x.HardwareType == 2);
            rfidList.Add(new SelectListItem() { Text = "--Select RFID--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE h in RFIDfiltered)
            {
                rfidList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }


            ViewBag.RFID = rfidList;

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

            #region Lane Type
            ViewBag.LaneType = HelperClass.GetLaneType();
            #endregion
            return View("LaneAdd");
        }

        private ActionResult LaneEditions(LaneCBE lane)
        {
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> hardwareDataList = new List<Libraries.CommonLibrary.CBE.HardwareCBE>();

            hardwareDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE>().ToList();

            #region ANPR Dropdown
            List<SelectListItem> anprList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> ANPRfiltered = hardwareDataList.FindAll(x => x.HardwareType == 1);
            anprList.Add(new SelectListItem() { Text = "--Select ANPR--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE h in ANPRfiltered)
            {
                anprList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }

            ViewBag.ANPR = anprList;

            #endregion

            #region RFID Dropdown
            List<SelectListItem> rfidList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> RFIDfiltered = hardwareDataList.FindAll(x => x.HardwareType == 2);
            rfidList.Add(new SelectListItem() { Text = "--Select RFID--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE h in RFIDfiltered)
            {
                rfidList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }


            ViewBag.RFID = rfidList;

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

            #region Lane Type
            ViewBag.LaneType = HelperClass.GetLaneType();
            #endregion
            return View("LaneEdit", lane);
        }

        public ActionResult LaneList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDataList = new List<Libraries.CommonLibrary.CBE.LaneCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                ViewBag.PlazaId = 0;
                laneDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>().ToList();

                return View(laneDataList);

            }
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View(laneDataList);
        }

        //Get
        public ActionResult LaneAdd(int id)
        {
            try
            {
                return LaneAdditions(id);
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Add Plaza Get Method" + ex);
            }
            return View();
        }

        [HttpPost]
        public ActionResult LaneAdd(Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            try
            {
                string[] urlid = Request.Url.AbsolutePath.Split('/');
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDataList = new List<Libraries.CommonLibrary.CBE.LaneCBE>();
                laneDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>().ToList();

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> LaneNamefiltered = laneDataList.FindAll(x => x.LaneName.ToLower() == lane.LaneName.ToLower() && x.PlazaId == lane.PlazaId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> ANPRFrontfiltered = laneDataList.FindAll(x => x.CameraIdFront == lane.CameraIdFront || x.CameraIdRear == lane.CameraIdFront);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> ANPRRearfiltered = laneDataList.FindAll(x => x.CameraIdFront == lane.CameraIdRear || x.CameraIdRear == lane.CameraIdRear);
                //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> RIFDfiltered = laneDataList.FindAll(x => x.EtcReaderId == lane.EtcReaderId);
                if (LaneNamefiltered.Count > 0)
                {

                    TempData["Message"] = "Lajur Name already exists with selected Gantry";
                    return LaneAdditions(Convert.ToInt32(urlid[urlid.Length - 1]));

                }
                if (ANPRFrontfiltered.Count > 0)
                {
                    TempData["Message"] = "Camera Name Front already exists";
                    return LaneAdditions(Convert.ToInt32(urlid[urlid.Length - 1]));
                }
                else if (ANPRRearfiltered.Count > 0)
                {
                    TempData["Message"] = "Camera Name Rear already exists";
                    return LaneAdditions(Convert.ToInt32(urlid[urlid.Length - 1]));
                }
                //else if (RIFDfiltered.Count > 0)
                //{
                //    TempData["Message"] = "ETC Reader Name already exists";
                //    return LaneAdditions(Convert.ToInt32(urlid[urlid.Length - 1]));
                //}


                lane.TMSId = 1;
                lane.CreationDate = DateTime.Now;
                Libraries.CommonLibrary.BLL.LaneBLL.Insert(lane);
                TempData["Message"] = "Successfully Created New Gantry";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Gantry Not Created";
                HelperClass.LogMessage("Failed To Add Plaza Post Method" + ex);
            }
            return Redirect("~/Home/LaneList");
        }


        [HttpGet]
        public ActionResult LaneEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Lane = id;
            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "LaneEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE();
                lane.LaneId = id;
                lane.TMSId = 1;
                lane.ModificationDate = DateTime.Now;
                lane.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                lane = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetLaneById(lane);

                ViewBag.PlazaId = lane.PlazaId;
                ViewBag.LaneTypeId = lane.LaneTypeId;
                ViewBag.CameraIdFront = lane.CameraIdFront;
                ViewBag.CameraIdRear = lane.CameraIdRear;
                //ViewBag.EtcReaderId = lane.EtcReaderId;
                TempData["Message"] = "";
                return LaneEditions(lane);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return Redirect("~/Home/LaneList");
            }

        }

        [HttpPost]
        public ActionResult LaneEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.LaneId = lane.LaneId;
            ViewBag.PlazaId = lane.PlazaId;
            ViewBag.LaneTypeId = lane.LaneTypeId;
            ViewBag.CameraIdFront = lane.CameraIdFront;
            ViewBag.CameraIdRear = lane.CameraIdRear;
            //ViewBag.EtcReaderId = lane.EtcReaderId;
            try
            {

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDataList = new List<Libraries.CommonLibrary.CBE.LaneCBE>();
                laneDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE>().ToList();

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> LaneNamefiltered = laneDataList.FindAll(x => (x.LaneName.ToLower() == lane.LaneName.ToLower() && x.PlazaId == lane.PlazaId) && x.LaneId != lane.LaneId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> ANPRFrontfiltered = laneDataList.FindAll(x => (x.CameraIdFront == lane.CameraIdFront || x.CameraIdRear == lane.CameraIdFront) && x.LaneId != lane.LaneId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> ANPRRearfiltered = laneDataList.FindAll(x => (x.CameraIdFront == lane.CameraIdRear || x.CameraIdRear == lane.CameraIdRear) && x.LaneId != lane.LaneId);
                //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> RIFDfiltered = laneDataList.FindAll(x => x.EtcReaderId == lane.EtcReaderId && x.LaneId != lane.LaneId);
                if (LaneNamefiltered.Count > 0)
                {
                    TempData["Message"] = "Lajur Name already exists with selected Gantry";
                    return LaneEditions(lane);

                }
                if (ANPRFrontfiltered.Count > 0)
                {
                    TempData["Message"] = "Camera Name Front already exists";
                    return LaneEditions(lane);
                }
                else if (ANPRRearfiltered.Count > 0)
                {
                    TempData["Message"] = "Camera Name Rear already exists";
                    return LaneEditions(lane);
                }
                //else if (RIFDfiltered.Count > 0)
                //{
                //    TempData["Message"] = "ETC Reader Name already exists";
                //    return LaneEditions(lane);
                //}



                lane.TMSId = 1;
                lane.ModificationDate = DateTime.Now;
                lane.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.Update(lane);
                return Redirect("~/Home/LaneList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update Hardware" + ex);
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                TempData["Message"] = "Failed to update Hardware" + ex;
                return LaneEditions(lane);
            }
        }
        #endregion

        #region Vehicle Class Section
        public ActionResult VehicleClassList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleclassDataList = new List<Libraries.CommonLibrary.CBE.VehicleClassCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                vehicleclassDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAll();

                return View(vehicleclassDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Vehicle Class List " + ex.Message.ToString());
            }
            return View(vehicleclassDataList);
        }

        public ActionResult VehicleClassAdd()
        {
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Add Plaza Get Method" + ex);
            }
            return View();
        }

        [HttpPost]
        public ActionResult VehicleClassAdd(Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            try
            {
                vehicleClass.TMSId = 1;
                vehicleClass.CreationDate = DateTime.Now;
                Libraries.CommonLibrary.BLL.VehicleClassBLL.Insert(vehicleClass);
                TempData["Message"] = "Successfully Created New Vehicle Class";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Vehicle Class Not Created";
                HelperClass.LogMessage("Failed To Add Plaza Post Method" + ex);
            }
            return Redirect("~/Home/VehicleClassList");
        }

        [HttpGet]
        public ActionResult VehicleClassEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Id = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "VehicleClassEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE();
                vehicleClass.Id = id;
                vehicleClass.TMSId = 1;
                vehicleClass.ModificationDate = DateTime.Now;
                vehicleClass.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                vehicleClass = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetVehicleClassId(vehicleClass);
                return View(vehicleClass);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return Redirect("~/Home/VehicleClassList");
            }
        }

        [HttpPost]
        public ActionResult VehicleClassEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Id = vehicleClass.Id;
            try
            {
                vehicleClass.TMSId = 1;
                vehicleClass.ModificationDate = DateTime.Now;
                vehicleClass.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.Update(vehicleClass);
                return Redirect("~/Home/VehicleClassList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update Hardware" + ex);
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                vehicleClass = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetVehicleClassId(vehicleClass);
                return View(vehicleClass);
            }
        }

        #endregion

        #region Customer Section
        public ActionResult CustomerList()
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
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View(customerDataList);
        }

        [HttpGet]
        public ActionResult CustomerAdd()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            // ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
            return View(customer);

        }

        [HttpPost]
        public ActionResult CustomerAdd(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

                if (!string.IsNullOrEmpty(customer.MobileNo))
                {
                    string MobileNoinital = string.Empty;
                    MobileNoinital = customer.MobileNo.Substring(0, 2);
                    if (MobileNoinital.StartsWith("0"))
                    {
                        customer.MobileNo = "62" + customer.MobileNo.Substring(1, customer.MobileNo.Length - 1);
                    }
                    else if (MobileNoinital != "62")
                    {
                        customer.MobileNo = "62" + customer.MobileNo;
                    }

                }
                if (string.IsNullOrEmpty(customer.Description))
                {
                    customer.Description = string.Empty;
                }
                if (string.IsNullOrEmpty(customer.LastName))
                {
                    customer.LastName = string.Empty;
                }
                if (string.IsNullOrEmpty(customer.Address))
                {
                    customer.Address = string.Empty;
                }

                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == customer.MobileNo.ToString());
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == customer.EmailId.ToString());
                if (Mobilefiltered.Count > 0)
                {
                    TempData["Message"] = "Mobile Number already exists";
                    return View("CustomerAdd");
                }
                else if (Emailfiltered.Count > 0)
                {
                    TempData["Message"] = "Email Id already exists";
                    return View("CustomerAdd");
                }
                customer.CreationDate = DateTime.Now;
                int userId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Insert(customer);

            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = "Failed to insert record";
            }
            return Redirect("~/Home/CustomerList");

        }

        [HttpGet]
        public ActionResult CustomerEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.AccountId = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "CustomerEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                customer.AccountId = id;
                customer = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetCustomerById(customer);
                return View(customer);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View();
            }



        }

        [HttpPost]
        public ActionResult CustomerEdit(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.AccountId = customer.AccountId;
            try
            {
                int customerId = customer.AccountId;
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

                if (!string.IsNullOrEmpty(customer.MobileNo))
                {
                    string MobileNo = customer.MobileNo;
                    MobileNo = MobileNo.Substring(0, 2);
                    if (MobileNo.StartsWith("0"))
                    {
                        customer.MobileNo = "62" + customer.MobileNo.Substring(1, customer.MobileNo.Length - 1);
                    }
                    else if (MobileNo != "62")
                    {
                        customer.MobileNo = "62" + customer.MobileNo;
                    }

                }
                if (string.IsNullOrEmpty(customer.Description))
                {
                    customer.Description = string.Empty;
                }
                if (string.IsNullOrEmpty(customer.LastName))
                {
                    customer.LastName = string.Empty;
                }
                if (string.IsNullOrEmpty(customer.Address))
                {
                    customer.Address = string.Empty;
                }

                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == customer.MobileNo.ToString() && x.AccountId != customerId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == customer.EmailId.ToString() && x.AccountId != customerId);
                if (Mobilefiltered.Count > 0)
                {
                    TempData["Message"] = "Mobile Number already exists";
                    return View("CustomerEdit");
                }
                else if (Emailfiltered.Count > 0)
                {
                    TempData["Message"] = "Email Id already exists";
                    return View("CustomerEdit");
                }
                customer.ModificationDate = DateTime.Now;
                customer.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.Update(customer);
                return Redirect("~/Home/CustomerList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update user" + ex);
                return View(customer);
            }
        }

        #endregion

        #region Customer Vehicle Section

        public ActionResult CustomerVehicleListbyCustomer(int id, string urltoken)
        {
            try
            {

                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.AccountId = id;
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

                string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "CustomerVehicleListbyCustomer", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
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

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsList();

                return View(customerDataList);

            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Customer Vehicle List " + ex.Message.ToString());
            }
            return View(customerDataList);
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

            return View("CustomerVehicleAdd");
        }

        [HttpGet]
        public ActionResult CustomerVehicleAdd()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
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
                    return Redirect("SessionPage");
                }
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
                int userId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Insert(customerVehicle);

            }
            catch (Exception)
            {
                ViewBag.SuccessMessage = "Failed to insert record";
            }
            return Redirect("~/Home/CustomerVehicleList");

        }

        private ActionResult CustomerVehicleEditions(CustomerVehicleCBE customerVehicle)
        {
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
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

            #region Customer Data Dropdown
            List<SelectListItem> customerclassList = new List<SelectListItem>();
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerDataList = new List<Libraries.CommonLibrary.CBE.CustomerAccountCBE>();
            customerDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsList();
            customerclassList.Add(new SelectListItem() { Text = "--Select Mobile No--", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE cr in customerDataList)
            {
                if (cr.AccountId == ViewBag.AccountId)
                {
                    ViewBag.FirstName = cr.FirstName;
                    ViewBag.LastName = cr.LastName;
                }
                customerclassList.Add(new SelectListItem() { Text = cr.MobileNo, Value = System.Convert.ToString(cr.AccountId) });
            }

            ViewBag.customerclassList = customerclassList;
            #endregion


            return View("CustomerVehicleEdit", customerVehicle);
        }

        [HttpGet]
        public ActionResult CustomerVehicleEdit(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.AccountId = id;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "CustomerVehicleEdit", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            if (token == urltoken)
            {
                CustomerVehicleCBE customerDataList = new CustomerVehicleCBE();
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                customerDataList.EntryId = id;
                customerDataList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleById(customerDataList);
                ViewBag.AccountId = customerDataList.AccountId;
                ViewBag.VehicleClassId = customerDataList.VehicleClassId;
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
                return Redirect("SessionPage");
            }
            ViewBag.EntryId = CustomerVehicleCBE.EntryId;
            try
            {
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleDataList = new List<Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
                customerVehicleDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsList();


                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == CustomerVehicleCBE.VehRegNo.ToString().ToLower() && x.EntryId != CustomerVehicleCBE.EntryId);
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> TagIdfiltered = customerVehicleDataList.FindAll(x => x.TagId == CustomerVehicleCBE.TagId.ToString() && x.EntryId != CustomerVehicleCBE.EntryId);
                if (VehRegNofiltered.Count > 0)
                {
                    TempData["Message"] = "Vehicle Registration Number already exists";
                    CustomerVehicleCBE customerDataList = new CustomerVehicleCBE();
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                    customerDataList.EntryId = CustomerVehicleCBE.EntryId;
                    customerDataList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleById(customerDataList);
                    ViewBag.AccountId = customerDataList.AccountId;
                    ViewBag.VehicleClassId = customerDataList.VehicleClassId;
                    return CustomerVehicleEditions(customerDataList);
                }
                else if (TagIdfiltered.Count > 0)
                {
                    TempData["Message"] = "Tag Id already exists";
                    CustomerVehicleCBE customerDataList = new CustomerVehicleCBE();
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE();
                    customerDataList.EntryId = CustomerVehicleCBE.EntryId;
                    customerDataList = Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleById(customerDataList);
                    ViewBag.AccountId = customerDataList.AccountId;
                    ViewBag.VehicleClassId = customerDataList.VehicleClassId;
                    return CustomerVehicleEditions(customerDataList);
                }
                CustomerVehicleCBE.ModificationDate = DateTime.Now;
                CustomerVehicleCBE.ModifiedBy = Convert.ToInt16(Session["LoggedUserId"]);
                CustomerVehicleCBE.TMSId = 1;
                CustomerVehicleCBE.TransferStatus = 1;
                CustomerVehicleCBE.VehRegNo = CustomerVehicleCBE.VehRegNo.ToUpper();
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Update(CustomerVehicleCBE);
                return Redirect("~/Home/CustomerVehicleList");
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to update user" + ex);
                return Redirect("~/Home/CustomerVehicleList");
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

        #region Live Events
        public ActionResult LiveEvent()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDataList = new List<Libraries.CommonLibrary.CBE.LaneCBE>();
            ViewData["apiPath"] = System.Configuration.ConfigurationManager.AppSettings["apiPath"];
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

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



            }
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetMSMQLiveEvent()
        {

            JsonResult result = new JsonResult();
            LiveEventCollection liveEvents = new LiveEventCollection();
            try
            {
                eventQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueue);
                Message[] msgs = eventQueue.GetAllMessages();
                eventQueue.Purge();
                if (msgs.Length > 0)
                {
                    foreach (Message msg in msgs)
                    {
                        Message m = msg;
                        m.Formatter = new BinaryMessageFormatter();
                        if (m != null)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            if (m.Body != null)
                            {
                                #region Processing packets
                                if (m.Body is CrossTalkEvent)
                                {
                                    #region CrossTalk packet
                                    CrossTalkEvent ctp = (CrossTalkEvent)m.Body;
                                    //CrossTalkEvent ctp = crossTalkPacket.Payload;
                                    LiveEventCBE liveEvent = new LiveEventCBE();
                                    liveEvent.PacketName = "CrossTalk";
                                    liveEvent.PlazaId = ctp.PlazaId;
                                    liveEvent.PlazaName = ctp.PlazaName;
                                    liveEvent.LaneName = ctp.LaneName;
                                    liveEvent.VehicleClassName = ctp.VehicleClassName;
                                    liveEvent.VRN = ctp.VRN;
                                    liveEvent.Datepacket = ctp.Timestamp.ToString(Libraries.CommonLibrary.Constants.dateTimeFormat24HsqlServer);
                                    liveEvents.Add(liveEvent);
                                    #endregion
                                }
                                else if (m.Body is NodeFluxEvent)
                                {
                                    #region NodeFlux packet
                                    NodeFluxEvent ntp = (NodeFluxEvent)m.Body;
                                    LiveEventCBE liveEvent = new LiveEventCBE();
                                    liveEvent.PacketName = "NodeFlux - " + ntp.CameraLocation;
                                    liveEvent.PlazaName = ntp.PlazaName;
                                    liveEvent.PlazaId = ntp.PlazaId;
                                    liveEvent.LaneName = ntp.LaneName;
                                    liveEvent.VehicleClassName = ntp.VehicleClassName;
                                    liveEvent.VRN = ntp.VRN;
                                    liveEvent.CameraLocation = ntp.CameraLocation;
                                    liveEvent.NumberPlatePath = ntp.NumberPlatePath.Replace('\\', '^'); ;
                                    liveEvent.VehiclePath = ntp.VehiclePath.Replace('\\','^');
                                    liveEvent.VideoURL = ntp.VideoURL.Replace('\\', '^'); ;
                                    liveEvent.Datepacket = ntp.Timestamp.ToString(Libraries.CommonLibrary.Constants.dateTimeFormat24HsqlServer);
                                    liveEvents.Add(liveEvent);
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to read MSMQ liveEvets data. " + ex.Message);
            }
            result.Data = liveEvents;
            return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ---------------------------------------------- Forgot Password Section ---------------------------------------------
        [HttpGet]
        public ActionResult ForgotPassword()
        {

            return View();
        }

        public JsonResult SendNewPassword(string email_id)
        {
            bool IsValid = false;
            JsonResult result = new JsonResult();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
            user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserByEmailId(email_id);
            if (user != null)
            {
                if (user.RoleId == 1) // 1 is super admin and not allowed to change password.
                {
                    result.Data = "Not Allowed";
                }
                else
                {
                    IsValid = true;
                }


                if (IsValid)
                {
                    string changedPassword = RandomString(8);
                    string mail_msg = string.Empty;
                    string update_msg = string.Empty;
                    mail_msg = SendMail(email_id, string.Empty, "ATMS" + " " + "-" + " " + "Response to forgot password", "Your password has been successfully changed. Your current password is" + " " + changedPassword);
                    if (mail_msg == "Failed")
                    {
                        result.Data = mail_msg;
                    }
                    else
                    {
                        update_msg = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.UpdatePassword("none", changedPassword, 0, email_id);//none - old password and 0 for user_id in case of forgot password only.
                        result.Data = update_msg;
                    }
                }
            }
            else
            {
                result.Data = "Not found";//  no user have this email id.
            }
            return result;

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string SendMail(string mailTo, string mailCC, string mailSubject, string mailBody)
        {
            String msg = string.Empty;
            const String password = "vaaan@223";
            String emailFrom = "tollmax.info@vaaaninfra.com";
            try
            {
                using (MailMessage mm = new MailMessage(emailFrom, mailTo))
                {
                    if (!string.IsNullOrEmpty(mailCC))
                    {
                        MailAddress copy = new MailAddress(mailCC);
                        mm.CC.Add(copy);
                    }
                    // Heading.
                    mm.Subject = mailSubject;
                    //Message Body Declaration.
                    mm.Body = mailBody;

                    //#region For File Attachmnent.
                    //if (!string.IsNullOrEmpty(fileUpload1.FileName))
                    //{
                    //    string FileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                    //    mm.Attachments.Add(new Attachment(fileUpload1.PostedFile.InputStream, FileName));
                    //}
                    //#endregion

                    // Send message to recipient
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.office365.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(emailFrom, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
                msg = "Mail sent";
                return msg;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to send mail. " + ex.Message);
                msg = "Failed";
                return msg;
            }
        }
        #endregion
        #region ---------------------------------------------- Change Password Section ---------------------------------------------
        public JsonResult ChangePassword(string old_pass, string new_pass)
        {
            JsonResult result = new JsonResult();
            int user_id = Convert.ToInt32(Session["LoggedUserId"]);
            int role_id = Convert.ToInt32(Session["RoleId"]);
            if (role_id != 1)
            {
                result.Data = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.UpdatePassword(old_pass, new_pass, user_id, "");// email id not needed at the time of change password.
            }
            else
            {
                result.Data = "Not Allowed";
            }
            return result;
        }
        #endregion

        #region RoleSection
        [HttpGet]
        public ActionResult RoleList()
        {
            Int32 RecordCount = 0;
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            //ViewBag.DynamicMainMenu = CreateMainMenu(Convert.ToInt16(Session["LoggedUserId"]));
            VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info = new VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo();
            info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);
            int userId = Convert.ToInt32(Session["LoggedUserId"]);
            info.CurrentPageIndex = (TempData["currentpage"] == null || Convert.ToString(Session["currentmodule"]) != "role") ? 0 : Convert.ToInt32(TempData["currentpage"]);

            TempData["currentpage"] = info.CurrentPageIndex;

            info.SearchText = "";

            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> roleList;
            roleList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetRoleList(info, ref RecordCount);
            info.RecordCount = RecordCount;

            // Calculate the Page Count
            info.PageCount = Convert.ToInt32(Math.Ceiling(((double)RecordCount / info.PageSize)));
            if (info.PageCount == 0) info.PageCount = 1;
            ViewBag.SortingPagingInfo = info;

            //VaaaN.ATMS.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE submodule_right = new VaaaN.ATMS.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE();
            //submodule_right = VaaaN.ATMS.Libraries.CommonLibrary.BLL.UserSubModuleActivityRightBLL.GetSubModuleRightByUserIdandSubmoduleId(userId, 2);

            //if (submodule_right.SubModuleEdit == false)
            //{
            //    ViewBag.Visibility = "visibility:hidden";
            //}
            //else
            //{
            //    ViewBag.Visibility = "visibility:visible";
            //}

            Session["currentmodule"] = "role";
            return View(roleList);
        }

        [HttpPost]
        public ActionResult RoleList(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, string SearchContent, string ddlPagesize)
        {
            Int32 RecordCount = 0;
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            //ViewBag.DynamicMainMenu = CreateMainMenu(Convert.ToInt16(Session["LoggedUserId"]));
            info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);
            info.SearchText = SearchContent;

            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> roleList;
            roleList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetRoleList(info, ref RecordCount);

            // Calculate the Page Count
            info.PageCount = Convert.ToInt32(Math.Ceiling(((double)RecordCount / info.PageSize)));
            if (info.PageCount == 0) info.PageCount = 1;
            info.RecordCount = RecordCount;
            ViewBag.SortingPagingInfo = info;

            TempData["currentpage"] = info.CurrentPageIndex;

            return View(roleList);
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }

            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            return View();
        }

        public JsonResult SaveRole(string role_name, string active, string remark)
        {
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                result.Data = "sessionout";
                return result;
            }

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE();
                role.RoleId = 0;
                role.RoleName = role_name;
                role.ISActive = Convert.ToInt32(active);
                role.ModifierId = Convert.ToInt32(Session["LoggedUserId"]);

                //role.Remark = remark;
                // Save new Role details
                // result.Data = VaaaN.ATMS.Libraries.CommonLibrary.BLL.RoleBLL.Insert(role);
                string strmsg = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.Insert(role);
                if (strmsg == "ROLE CREATED")
                {
                    result.Data = "saved";
                }
                else
                {
                    result.Data = "found";
                }

            }
            catch
            {
                result.Data = "error";
            }
            return result;
        }

        [HttpGet]
        public ActionResult EditRole(int id, string Rolename, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            ViewBag.Rolename = Rolename;
            ViewBag.RoleId = id;
            return View();
        }

        public JsonResult GetRoleByRoleId(string role_id)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE roleid = new Libraries.CommonLibrary.CBE.RoleCBE();
            roleid.RoleId = Convert.ToInt32(role_id);
            DataTable ldt = new DataTable();
            ldt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetRoleById(roleid);
            List<Dictionary<string, object>> lstRole = GetTableRows(ldt);
            return Json(lstRole, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRole(string role_id, string role_name, string oldrole_name, string active, string remark)
        {
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                result.Data = "sessionout";
                return result;
            }

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE();
                role.RoleId = Convert.ToInt32(role_id);
                role.RoleName = role_name;
                role.ISActive = Convert.ToInt32(active);
                role.ModifierId = Convert.ToInt32(Session["LoggedUserId"]);
                role.Description = "";
                // update role details
                string strresult = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.Update(role, oldrole_name);

                if (strresult == "ROLE CREATED")
                {
                    result.Data = "updated";
                }
                else
                {
                    result.Data = "found";
                }

            }
            catch
            {
                result.Data = "error";
            }
            return result;
        }
        #endregion

        #region ---------------------------------------------- Role Module Rights ----------------------------------------
        [HttpGet]
        public ActionResult RoleModuleRight(Int32 Id, string Rolename) // , string urltoken // here Id = Role_Id
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Rolename = Rolename;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            ////This method employs a common business logic for all urls, but only the parameters are different. It helps the url password protected
            //string token = VaaaN.PMS.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "RoleModuleRight", Convert.ToString(Id), VaaaN.PMS.CommonLibrary.Common.CommonClass.urlProtectPassword);

            //if (token == urltoken)
            //{
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE> roleModuleRight;
            roleModuleRight = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleModuleRightBLL.GetRoleModuleRightByRoleId(Id); // here Id = Role_Id
            return View(roleModuleRight);
            //}
            //else
            //{
            //    ViewBag.urlForged = "URL has been modified or forged.";
            //    return View();
            //}
        }

        [HttpGet]
        public ActionResult ResetRoleModuleRight(int id, string Rolename, string prm)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            ViewBag.Rolename = Rolename;

            VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE module = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE();

            string parameters = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.Decrypt(prm);

            string[] prms = parameters.Split('~');
            module.Id = id;
            module.RoleId = Convert.ToInt32(prms[0]);
            module.ModuleId = Convert.ToInt32(prms[1]);
            module.ModuleName = prms[2];
            module.ModuleView = Convert.ToBoolean(prms[3]);
            module.ModuleAdd = Convert.ToBoolean(prms[4]);
            module.ModuleEdit = Convert.ToBoolean(prms[5]);
            module.ModuleDelete = Convert.ToBoolean(prms[6]);

            // This viewbag variable is used in ResetModuleRight.cshtml page i.e. Back to List link
            ViewBag.RoleId = module.RoleId;
            return View(module);
        }

        [HttpPost]
        public ActionResult ResetRoleModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE module)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleModuleRightBLL.InsertUpdateRoleModuleRight(module);
            }
            catch { }

            //string token = VaaaN.PMS.CommonLibrary.CommonClass.generateUrlToken("Admin", "RoleModuleRight", Convert.ToString(module.RoleId), VaaaN.PMS.CommonLibrary.CommonClass.urlProtectPassword);
            return RedirectToAction("RoleModuleRight", new { id = module.RoleId });
        }
        #endregion

        #region ---------------------------------------------- Role Sub Module Rights ------------------------------------
        [HttpGet]
        public ActionResult RoleSubModuleRight(int id, string Rolename, int roleId, int moduleId)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Rolename = Rolename;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE> roleSubModuleRight;
            roleSubModuleRight = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleSubmoduleRightBLL.GetRoleSubmoduleRightByRoleId(roleId, moduleId);
            ViewBag.RoleID = Convert.ToString(roleId);
            return View(roleSubModuleRight);
        }

        [HttpGet]
        public ActionResult ResetSubModuleRoleRight(int id, string Rolename, string prm)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Rolename = Rolename;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            // prm= item.RoleId + "~" + item.ModuleId + "~" + item.ModuleName + "~" + item.SubmoduleId + "~" + item.SubmoduleName + "~" + item.SubmoduleView + "~" + item.SubmoduleAdd + "~" + item.SubmoduleEdit + "~" + item.SubmoduleDelete

            string parameters = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.Decrypt(prm);
            string[] prms = parameters.Split('~');
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE subModule = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE();
            subModule.Id = id;
            subModule.RoleId = Convert.ToInt32(prms[0]);
            subModule.ModuleId = Convert.ToInt32(prms[1]);
            subModule.ModuleName = prms[2];
            subModule.SubModuleId = Convert.ToInt32(prms[3]);
            subModule.SubModuleName = prms[4];
            subModule.SubModuleView = Convert.ToBoolean(prms[5]);
            subModule.SubModuleAdd = Convert.ToBoolean(prms[6]);
            subModule.SubModuleEdit = Convert.ToBoolean(prms[7]);
            subModule.SubModuleDelete = Convert.ToBoolean(prms[8]);
            return View(subModule);
        }

        //  Post sub module rights and saved
        [HttpPost]
        public ActionResult ResetSubModuleRoleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE subModule)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleSubmoduleRightBLL.InsertUpdateRoleSubModuleRight(subModule);
            }
            catch
            {
            }
            return RedirectToAction("RoleSubModuleRight", new { id = subModule.Id, roleId = subModule.RoleId, moduleId = subModule.ModuleId });
        }
        #endregion

        #region ---------------------------------------------- User Module and  Sub Module Rights ------------------------
        [HttpGet]
        public ActionResult UserModuleRight(int id, string name, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Username = name;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "UserModuleRight", Convert.ToString(id), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> userRight = null;
            if (token == urltoken)
            {
                userRight = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserModuleActivityRightBLL.GetUserModuleRightByUserId(id);
                ViewBag.UserID = Convert.ToString(id);
                return View(userRight);
            }
            else
            {
                ViewBag.urlForged = "URL has been modified or forged.";
                return View(userRight);
            }
        }

        [HttpGet]
        public ActionResult ResetUserModuleRight(int id, string name, int uid, int mid)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("SessionPage", "Home");
            }
            ViewBag.Username = name;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));

            string moduleName = string.Empty;
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE moduleRight = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE();

            moduleRight.Id = id;
            moduleRight.ModuleId = mid;
            moduleRight.UserId = uid;

            if (id == 0)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE();
                module = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ModuleBLL.GetModuleById(mid);
                moduleRight.ModuleName = module.ModuleName;
                moduleRight.ModuleView = false;
                moduleRight.ModuleAdd = false;
                moduleRight.ModuleEdit = false;
                moduleRight.ModuleDelete = false;
            }
            else
            {
                // 
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE module = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE();
                module = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserModuleActivityRightBLL.GetModuleRightById(id);
                moduleRight.ModuleName = module.ModuleName;
                moduleRight.ModuleView = module.ModuleView;
                moduleRight.ModuleAdd = module.ModuleAdd;
                moduleRight.ModuleEdit = module.ModuleEdit;
                moduleRight.ModuleDelete = module.ModuleDelete;
            }

            ViewBag.UserId = uid.ToString();
            return View(moduleRight);
        }

        [HttpPost]
        public ActionResult ResetUserModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE module)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserModuleActivityRightBLL.InsertUpdateUserModuleActivityRight(module);

            string token = VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.generateUrlToken("Home", "UserModuleRight", Convert.ToString(module.UserId), VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.urlProtectPassword);
            return RedirectToAction("UserModuleRight", new { id = module.UserId, urltoken = token });
        }

        [HttpGet]
        public ActionResult UserSubModulesRight(int id, string userName, int mid) // here Id is user id and mid is module id
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.Username = userName;
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            // Display the list of Module, Sub Module and Rights
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> subModuleRight;
            subModuleRight = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubModuleActivityRightBLL.GetUserSubModuleRightByUserIdAndModuleId(id, mid);
            ViewBag.UserID = Convert.ToString(id);
            ViewBag.ModuleId = Convert.ToString(mid);
            return View(subModuleRight);
        }

        [HttpGet]
        public ActionResult ResetSubModuleRight(int id, string Rolename, int uid, int mid, int sid)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            ViewBag.Rolename = Rolename;
            string moduleName = string.Empty;
            string subModuleName = string.Empty;
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModuleRight = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE();

            subModuleRight.Id = id;
            subModuleRight.UserId = uid;
            subModuleRight.ModuleId = mid;
            subModuleRight.SubModuleId = sid;

            if (id == 0) // Data not existed in database
            {
                // Get Module Name
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE();
                module = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ModuleBLL.GetModuleById(mid);

                // Get sub module Name
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE();
                DataTable dt = new DataTable();
                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetSubModuleBySubModuleId(Convert.ToString(sid));
                if (dt != null)
                {
                    subModule.ModuleId = mid;
                    subModule.SubModuleId = sid;
                    subModule.SubModuleName = Convert.ToString(dt.Rows[0]["SUBMODULE_NAME"]);
                    subModule.IsGuiVisible = Convert.ToInt32(dt.Rows[0]["IS_GUI_VISIBLE"]);
                }

                subModuleRight.ModuleName = module.ModuleName;
                subModuleRight.SubModuleName = subModule.SubModuleName;
                subModuleRight.SubModuleView = false;
                subModuleRight.SubModuleAdd = false;
                subModuleRight.SubModuleEdit = false;
                subModuleRight.SubModuleDelete = false;
            }
            else
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE();
                subModule = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubModuleActivityRightBLL.GetSubModuleRightById(id);
                subModuleRight.ModuleName = subModule.ModuleName;
                subModuleRight.SubModuleName = subModule.SubModuleName;
                subModuleRight.SubModuleView = subModule.SubModuleView;
                subModuleRight.SubModuleAdd = subModule.SubModuleAdd;
                subModuleRight.SubModuleEdit = subModule.SubModuleEdit;
                subModuleRight.SubModuleDelete = subModule.SubModuleDelete;
            }
            ViewBag.UserId = uid;
            ViewBag.ModuleId = mid;
            ViewBag.SubModuleID = sid;
            return View(subModuleRight);
        }

        [HttpPost]
        public ActionResult ResetSubModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule)
        {
            if (Session["LoggedUserId"] == null)
            {
                return Redirect("SessionPage");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubModuleActivityRightBLL.InsertUpdateUserSubModuleActivityRight(subModule);
            }
            catch
            {
            }
            return RedirectToAction("UserSubModulesRight", new { id = subModule.UserId, mid = subModule.ModuleId });
        }
        #endregion


        #region ---------------Admin-----------------------------------
        public ActionResult TollRateList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE> tollrateDataList = new List<Libraries.CommonLibrary.CBE.TollRateCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return Redirect("SessionPage");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]));
                //Call Bll To get all Plaza List
                tollrateDataList = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll().Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE>().ToList();

                return View(tollrateDataList);

            }
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View(tollrateDataList);

        }
        #endregion
        public ActionResult SessionPage()
        {
            return View();
        }

        #region Helper Method



        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }



        //bool ISAuthorize(int userId, string strlink)
        //{
        //    try
        //    {
        //        VaaaN.ATMS.Libraries.CommonLibrary.CBE.ModuleCollection modules = null;
        //        // Get all modules which are authorized to this user.
        //        if (userId == 1)// super admin
        //        {
        //            modules = VaaaN.ATMS.Libraries.CommonLibrary.DAL.ModuleDAL.GetAll(VaaaN.ATMS.Libraries.CommonLibrary.Constants.GetCurrentATMSId());
        //        }
        //        else
        //        {
        //            modules = VaaaN.ATMS.Libraries.CommonLibrary.DAL.ModuleDAL.GetByUserId(userId);
        //        }
        //        foreach (VaaaN.ATMS.Libraries.CommonLibrary.CBE.ModuleCBE module in modules)
        //        {
        //            string link = "";

        //            // Get all submodules which are authorized to this user
        //            VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = new VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleCollection();

        //            if (userId == 1)// super admin
        //            {
        //                VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleCollection subModulesTemp = VaaaN.ATMS.Libraries.CommonLibrary.BLL.SubModuleBLL.GetAll(VaaaN.ATMS.Libraries.CommonLibrary.Constants.GetCurrentATMSId());

        //                foreach (VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule in subModulesTemp)
        //                {
        //                    if (subModule.ModuleId == module.ModuleId)
        //                    {
        //                        subModules.Add(subModule);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                subModules = VaaaN.ATMS.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(userId, module.ModuleId);
        //            }

        //            link = string.Empty;

        //            if (subModules.Count == 0)
        //            {
        //                sb.Append("<a  class=\"list-group-item list-group-item-info\" style=\"background-color:rgb(46, 109, 164);  color:rgb(217, 237, 247);\"  data-parent=\"#MainMenu\" href=\"" + module.ModuleUrl + "\">");//module.Url
        //                sb.Append("<i  style=\"padding-right:10%; padding-left:1%\" class=\"" + "" + "\"></i>" + module.ModuleName + "</a>");
        //            }
        //            else
        //            {
        //                sb.Append("<a id=\"" + module.ModuleName + "\" class=\"list-group-item list-group-item-info\" style=\"background-color:rgb(46, 109, 164);  color:rgb(217, 237, 247);\" data-toggle=\"collapse\"  data-Parent=\"#MainMenu\" href=\"#" + module.ModuleName + "subMenu\">");

        //                sb.Append("<i  style=\"padding-right:10%; padding-left:1%\" class=\"" + "" + "\"></i>" + module.ModuleName + "");
        //                sb.Append("<i  style=\"float:right\" class=\"fa fa-caret-down\"></i></a>");
        //                sb.Append("<div class=\"collapse list-group-submenu\"  id=\"" + module.ModuleName + "subMenu\">");
        //                foreach (VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule in subModules)
        //                {
        //                    sb.Append("<a class=\"list-group-item list-group-item-info\" data-parent=\"#" + module.ModuleName + "subMenu\" href=\"" + subModule.SubmoduleUrl + "\">");//Submodule.Url
        //                    sb.Append("<i  style=\"padding-right:10%; padding-left:1%\" class=\"fa fa-caret-right\"></i>" + subModule.SubModuleName + "</a>");
        //                }
        //                sb.Append("</div>");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    return true;
        //}
        #endregion
    }
}