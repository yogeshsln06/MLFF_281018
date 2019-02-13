using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using static MLFFWebUI.Models.HelperClass;

namespace MLFFWebUI.Controllers
{
    public class SetUpController : Controller
    {
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();
        // GET: SetUp
        public ActionResult Index()
        {
            return View();
        }

        #region User Section
        [HttpGet]
        public ActionResult UsersList()
        {
            List<UserCBE> userList = new List<UserCBE>();
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Users");
                int userId = Convert.ToInt32(Session["LoggedUserId"]);
                userList = UserBLL.GetUserAll().Cast<UserCBE>().ToList();
                ViewData["UserDataList"] = UserBLL.GetUserAll().Cast<UserCBE>().ToList();
                UserSubModuleActivityRightCBE submodule_right = new UserSubModuleActivityRightCBE();
                submodule_right = UserSubModuleActivityRightBLL.GetSubModuleRightByUserIdandSubmoduleId(userId, 1);

                if (submodule_right.SubModuleEdit == false)
                {
                    ViewBag.Visibility = "visibility:hidden";
                }
                else
                {
                    ViewBag.Visibility = "visibility:visible";
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to open userlist page..Check user submodule rights" + ex);
                TempData["Failedmsg"] = "<script>alert('Failed to login.Try again later..');</script>";
                return RedirectToAction("Logout", "Login");
            }
            return View("UsersList", userList);
        }
        public JsonResult UserReload()
        {
            List<UserCBE> UserDataList = new List<UserCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                UserDataList = UserBLL.GetUserAll().Cast<UserCBE>().ToList(); ;
                result.Data = UserDataList;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Classification List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UserNew()
        {
            ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            return View("UserPopUp");
        }

        [HttpPost]
        public ActionResult GetUser(int id, string urltoken)
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.UserId = id;

            UserCBE user = new UserCBE();
            user.UserId = id;
            //user.TmsId = 1;
            user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserById(user);

            ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            ViewBag.SlRoleId = user.RoleId;
            return View("UserPopUp", user);
        }

        [HttpPost]
        public JsonResult AddUser(UserCBE User)
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
                    #region Mobile No validate by using country code
                    if (!string.IsNullOrEmpty(User.MobileNo))
                        {
                        User.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(User.MobileNo);
                        }
                    #endregion

                    #region Validate duplicate 
                    DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.dtGetUserAll();
                    DataRow[] Loginfiltered = dt.Select("LOGIN_NAME ='" + User.LoginName + "' AND USER_ID<>" + User.UserId + "");
                    DataRow[] Mobilefiltered = dt.Select("MOBILE_NO ='" + User.MobileNo + "' AND USER_ID<>" + User.UserId + "");
                    DataRow[] Emailfiltered = dt.Select("EMAIL_ID ='" + User.EmailId + "' AND USER_ID<>" + User.UserId + "");
                    #endregion
                    if (Loginfiltered.Length > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Username already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Mobilefiltered.Length > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Mobile Number already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Emailfiltered.Length > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Email Id already exists";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        #region Insert Into User Data
                        User.CreationDate = DateTime.Now;
                        int userId = UserBLL.Insert(User);
                        if (userId > 0)
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
                HelperClass.LogMessage("Failed to Insert User in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UserUpdate(UserCBE UserAccount)
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

                    int UserId = UserAccount.UserId;

                    List<UserCBE> UserDataList = new List<UserCBE>();
                    #region Mobile No validate by using country code
                    if (!string.IsNullOrEmpty(UserAccount.MobileNo))
                    {
                        UserAccount.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(UserAccount.MobileNo);

                    }
                    #endregion

                    #region Validate duplicate 
                    DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.dtGetUserAll();
                    //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> Mobilefiltered = UserDataList.FindAll(x => x.MobileNo == UserAccount.MobileNo.ToString() && x.UserId != UserId);
                    //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> Emailfiltered = UserDataList.FindAll(x => x.EmailId == UserAccount.EmailId.ToString() && x.UserId != UserId);
                    DataRow[] Mobilefiltered = dt.Select("MOBILE_NO ='" + UserAccount.MobileNo + "' AND USER_ID<>" + UserAccount.UserId + "");
                    DataRow[] Emailfiltered = dt.Select("EMAIL_ID ='" + UserAccount.EmailId + "' AND USER_ID<>" + UserAccount.UserId + "");
                    #endregion
                    if (Mobilefiltered.Length > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Mobile Number already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Emailfiltered.Length > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Email Id already exists";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        UserBLL.Update(UserAccount);
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert User Registration List in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chnage Password
        [HttpPost]
        public JsonResult ChnagePassword(string CurrentPassword, string NewPassword)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = UserBLL.UpdatePassword(CurrentPassword, NewPassword, Convert.ToInt32(Session["LoggedUserId"]), "");
                    objResponseMessage.Add(objModelState);

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed Chnage Password in Setup Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Role Section
        public ActionResult RoleList()
        {
            IEnumerable<RoleCBE> roleList;
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Roles");
                Int32 RecordCount = 0;

                VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info = new VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo();
                info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);
                int userId = Convert.ToInt32(Session["LoggedUserId"]);
                info.CurrentPageIndex = (TempData["currentpage"] == null || Convert.ToString(Session["currentmodule"]) != "role") ? 0 : Convert.ToInt32(TempData["currentpage"]);

                TempData["currentpage"] = info.CurrentPageIndex;

                info.SearchText = "";


                roleList = RoleBLL.GetRoleList(info, ref RecordCount);
                info.RecordCount = RecordCount;
                ViewData["RolesDataList"] = RoleBLL.GetRoleList(info, ref RecordCount);
                // Calculate the Page Count
                info.PageCount = Convert.ToInt32(Math.Ceiling(((double)RecordCount / info.PageSize)));
                if (info.PageCount == 0) info.PageCount = 1;
                ViewBag.SortingPagingInfo = info;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to open rolelist page..Check user submodule rights" + ex);
                return RedirectToAction("Logout", "Login");
            }
            return View(roleList);
        }

        [HttpGet]
        public JsonResult RoleReload()
        {
            List<RoleCBE> RoleDataList = new List<RoleCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                //Int32 RecordCount = 0;

                //VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info = new VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo();
                //info.PageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["GridPageSize"]);
                RoleDataList = RoleBLL.GetRoleAll().Cast<RoleCBE>().ToList();
                result.Data = RoleDataList;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Role List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRoles(int id, string urltoken)
        {
            RoleCBE Roles = new RoleCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    Roles.RoleId = id;
                    Roles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetRoleByIdcollection(Roles);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetRoles " + ex);
            }
            return View("RoleListPopUp", Roles);
        }

        [HttpGet]
        public ActionResult RolesNew()
        {
            return View("RoleListPopUp");
        }

        [HttpPost]
        public JsonResult RolesAdd(RoleCBE Roles)
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

                    List<RoleCBE> RoleDataList = new List<RoleCBE>();
                    RoleDataList = RoleBLL.GetRoleAll().Cast<RoleCBE>().ToList();

                    List<RoleCBE> RoleNamefiltered = RoleDataList.FindAll(x => x.RoleName.ToLower() == Roles.RoleName.ToLower() && x.RoleId != Roles.RoleId);
                    //List<PlazaCBE> IpAddressfiltered = PalazaDataList.FindAll(x => x.IpAddress == Plaza.IpAddress);
                    if (RoleNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Role Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        #region Insert Into Roles Data
                        //Roles.RoleId = 1;
                        Roles.TransferStatus = 1;
                        Roles.CreationDate = DateTime.Now;
                        Roles.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        string Result = RoleBLL.Insert(Roles);
                        if (Result != "")
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
                HelperClass.LogMessage("Failed to Insert Roles in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RolesUpdate(RoleCBE Roles)
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
                    //Roles.RoleId = 1;
                    Roles.TransferStatus = 1;
                    Roles.ModificationDate = DateTime.Now;
                    Roles.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                    string Result = RoleBLL.Update(Roles, Roles.RoleName);
                        if (Result != "")
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
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Update Roles List in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Plaza Section
        public ActionResult PlazaList()
        {
            List<PlazaCBE> plazaDataList = new List<PlazaCBE>();
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Plaza");
                ViewData["GantryDataList"] = PlazaBLL.GetAllAsList();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load Plaza List " + ex.Message.ToString());
            }
            return View(plazaDataList);
        }

        [HttpGet]
        public JsonResult GantryReload()
        {
            List<PlazaCBE> PlazaDataList = new List<PlazaCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                PlazaDataList = PlazaBLL.GetAllAsList().Cast<PlazaCBE>().ToList(); ;
                result.Data = PlazaDataList;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Gantry List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetGantry(int id, string urltoken)
        {
            PlazaCBE plaza = new PlazaCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    plaza.PlazaId = id;
                    plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetPlazaById(plaza);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetPlaza " + ex);
            }
            return View("PlazaListPopUp", plaza);
        }

        [HttpGet]
        public ActionResult GantryNew()
        {
            return View("PlazaListPopUp");
        }

        [HttpPost]
        public JsonResult GantryAdd(PlazaCBE Plaza)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<PlazaCBE> PalazaDataList = new List<PlazaCBE>();
                    PalazaDataList = PlazaBLL.GetAllAsList().Cast<PlazaCBE>().ToList();

                    List<PlazaCBE> PlazaNamefiltered = PalazaDataList.FindAll(x => x.PlazaName.ToLower() == Plaza.PlazaName.ToLower() && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> IpAddressfiltered = PalazaDataList.FindAll(x => x.IpAddress == Plaza.IpAddress && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> Longitudefiltered = PalazaDataList.FindAll(x => x.Longitude == Plaza.Longitude && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> Latitudefiltered = PalazaDataList.FindAll(x => x.Latitude == Plaza.Latitude && x.PlazaId != Plaza.PlazaId);
                    if (PlazaNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Gantry Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                     if (IpAddressfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "IpAddress already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Longitudefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Longitude already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Latitudefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Latitude already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        Plaza.TmsId = 1;
                        Plaza.CreationDate = DateTime.Now;
                        Plaza.ModifierId= Convert.ToInt16(Session["LoggedUserId"]);
                        int id = PlazaBLL.Insert(Plaza);
                        if (id > 0)
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Gantry in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GantryUpdate(PlazaCBE Plaza)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<PlazaCBE> PalazaDataList = new List<PlazaCBE>();
                    PalazaDataList = PlazaBLL.GetAllAsList().Cast<PlazaCBE>().ToList();

                    List<PlazaCBE> PlazaNamefiltered = PalazaDataList.FindAll(x => x.PlazaName.ToLower() == Plaza.PlazaName.ToLower() && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> IpAddressfiltered = PalazaDataList.FindAll(x => x.IpAddress == Plaza.IpAddress && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> Longitudefiltered = PalazaDataList.FindAll(x => x.Longitude == Plaza.Longitude && x.PlazaId != Plaza.PlazaId);
                    List<PlazaCBE> Latitudefiltered = PalazaDataList.FindAll(x => x.Latitude == Plaza.Latitude && x.PlazaId != Plaza.PlazaId);
                    if (PlazaNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Gantry Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (IpAddressfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "IpAddress already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Longitudefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Longitude already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (Latitudefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Latitude already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        Plaza.TmsId = 1;
                        Plaza.CreationDate = DateTime.Now;
                        Plaza.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        PlazaBLL.Update(Plaza);
                       
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "success";
                            objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Lane Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lane Section

        public ActionResult LaneList()
        {

            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");

            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Lane");
                ViewData["LaneDataList"] = LaneBLL.GetAll().Cast<LaneCBE>().ToList();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load Hardware List " + ex.Message.ToString());
            }
            return View();
        }

        public JsonResult LaneReload()
        {
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                result.Data = LaneBLL.GetAll().Cast<LaneCBE>().ToList();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Classification List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LaneNew()
        {
            List<HardwareCBE> hardwareDataList = HardwareBLL.GetAll().Cast<HardwareCBE>().ToList();
            #region ANPR Dropdown
            List<SelectListItem> anprList = new List<SelectListItem>();
            List<HardwareCBE> ANPRfiltered = hardwareDataList.FindAll(x => x.HardwareType == 1);
            anprList.Add(new SelectListItem() { Text = "--Select ANPR--", Value = "0" });
            foreach (HardwareCBE h in ANPRfiltered)
            {
                anprList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }

            ViewBag.ANPR = anprList;

            #endregion

            #region RFID Dropdown
            List<SelectListItem> rfidList = new List<SelectListItem>();
            List<HardwareCBE> RFIDfiltered = hardwareDataList.FindAll(x => x.HardwareType == 2);
            rfidList.Add(new SelectListItem() { Text = "--Select RFID--", Value = "0" });
            foreach (HardwareCBE h in RFIDfiltered)
            {
                rfidList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
            }


            ViewBag.RFID = rfidList;

            #endregion

            #region Gantry Class Dropdown
            List<SelectListItem> gantryList = new List<SelectListItem>();
            List<PlazaCBE> plaza = PlazaBLL.GetAllAsList();

            gantryList.Add(new SelectListItem() { Text = "--Select Gantry--", Value = "0" });
            foreach (PlazaCBE cr in plaza)
            {
                gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
            }

            ViewBag.Gantry = gantryList;

            #endregion

            #region Lane Type
            ViewBag.LaneType = HelperClass.GetLaneType();
            #endregion
            return View("LaneListPopUp");
        }

        [HttpPost]
        public JsonResult LaneAdd(LaneCBE lane)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else
                {
                    List<LaneCBE> laneDataList = new List<LaneCBE>();
                    laneDataList = LaneBLL.GetAll().Cast<LaneCBE>().ToList();

                    List<LaneCBE> LaneNamefiltered = laneDataList.FindAll(x => x.LaneName.ToLower() == lane.LaneName.ToLower() && x.PlazaId == lane.PlazaId);
                    List<LaneCBE> ANPRFrontfiltered = laneDataList.FindAll(x => x.CameraIdFront == lane.CameraIdFront || x.CameraIdRear == lane.CameraIdFront);
                    List<LaneCBE> ANPRRearfiltered = laneDataList.FindAll(x => x.CameraIdFront == lane.CameraIdRear || x.CameraIdRear == lane.CameraIdRear);
                    List<LaneCBE> RFIDFrontfiltered = laneDataList.FindAll(x => x.AntennaIdFront == lane.AntennaIdFront || x.AntennaIdRear == lane.AntennaIdFront);
                    List<LaneCBE> RFIDRearfiltered = laneDataList.FindAll(x => x.AntennaIdFront == lane.AntennaIdRear || x.AntennaIdRear == lane.AntennaIdRear);
                    if (LaneNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Lajur Name already exists with selected Gantry.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (ANPRFrontfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Camera Front already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (ANPRRearfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Camera Rear already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (RFIDFrontfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "RFID Front already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (RFIDRearfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "RFID Rear already exists.";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        lane.TMSId = 1;
                        lane.CreationDate = DateTime.Now;
                        int id = LaneBLL.Insert(lane);
                        if (id > 0)
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Hardware in Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LaneUpdate(LaneCBE lane)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else
                {
                    List<LaneCBE> laneDataList = new List<LaneCBE>();
                    laneDataList = LaneBLL.GetAll().Cast<LaneCBE>().ToList();

                    List<LaneCBE> LaneNamefiltered = laneDataList.FindAll(x => (x.LaneName.ToLower() == lane.LaneName.ToLower() && x.PlazaId == lane.PlazaId) && x.LaneId != lane.LaneId);
                    List<LaneCBE> ANPRFrontfiltered = laneDataList.FindAll(x => (x.CameraIdFront == lane.CameraIdFront || x.CameraIdRear == lane.CameraIdFront) && x.LaneId != lane.LaneId);
                    List<LaneCBE> ANPRRearfiltered = laneDataList.FindAll(x => (x.CameraIdFront == lane.CameraIdRear || x.CameraIdRear == lane.CameraIdRear) && x.LaneId != lane.LaneId);
                    List<LaneCBE> RFIDFrontfiltered = laneDataList.FindAll(x => x.AntennaIdFront == lane.AntennaIdFront || x.AntennaIdRear == lane.AntennaIdFront && x.LaneId != lane.LaneId);
                    List<LaneCBE> RFIDRearfiltered = laneDataList.FindAll(x => x.AntennaIdFront == lane.AntennaIdRear || x.AntennaIdRear == lane.AntennaIdRear && x.LaneId != lane.LaneId);
                    if (LaneNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Lajur Name already exists with selected Gantry.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (ANPRFrontfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Camera Front already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (ANPRRearfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Camera Rear already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (RFIDFrontfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "RFID Front already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    else if (RFIDRearfiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "RFID Rear already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        lane.TMSId = 1;
                        lane.ModificationDate = DateTime.Now;
                        lane.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        LaneBLL.Update(lane);

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }


                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Lane Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLane(int id, string urltoken)
        {
            LaneCBE laneData = new LaneCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {
                    List<HardwareCBE> hardwareDataList = HardwareBLL.GetAll().Cast<HardwareCBE>().ToList();
                    #region ANPR Dropdown
                    List<SelectListItem> anprList = new List<SelectListItem>();
                    List<HardwareCBE> ANPRfiltered = hardwareDataList.FindAll(x => x.HardwareType == 1);
                    anprList.Add(new SelectListItem() { Text = "--Select ANPR--", Value = "0" });
                    foreach (HardwareCBE h in ANPRfiltered)
                    {
                        anprList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
                    }

                    ViewBag.ANPR = anprList;

                    #endregion

                    #region RFID Dropdown
                    List<SelectListItem> rfidList = new List<SelectListItem>();
                    List<HardwareCBE> RFIDfiltered = hardwareDataList.FindAll(x => x.HardwareType == 2);
                    rfidList.Add(new SelectListItem() { Text = "--Select RFID--", Value = "0" });
                    foreach (HardwareCBE h in RFIDfiltered)
                    {
                        rfidList.Add(new SelectListItem() { Text = h.HardwareName, Value = System.Convert.ToString(h.HardwareId) });
                    }


                    ViewBag.RFID = rfidList;

                    #endregion

                    #region Gantry Class Dropdown
                    List<SelectListItem> gantryList = new List<SelectListItem>();
                    List<PlazaCBE> plaza = PlazaBLL.GetAllAsList();

                    gantryList.Add(new SelectListItem() { Text = "--Select Gantry--", Value = "0" });
                    foreach (PlazaCBE cr in plaza)
                    {
                        gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                    }

                    ViewBag.Gantry = gantryList;

                    #endregion

                    #region Lane Type
                    ViewBag.LaneType = HelperClass.GetLaneType();
                    #endregion

                    laneData.LaneId = id;
                    laneData = LaneBLL.GetLaneById(laneData);

                    #region ViewBag for DDL Values
                    ViewBag.hfPlazaId = laneData.PlazaId;
                    ViewBag.hfAntennaIdFront = laneData.AntennaIdFront;
                    ViewBag.hfAntennaIdRear = laneData.AntennaIdRear;
                    ViewBag.hfCameraIdFront = laneData.CameraIdFront;
                    ViewBag.hfCameraIdRear = laneData.CameraIdRear;
                    ViewBag.hfEtcAntennaNameFront = laneData.EtcAntennaNameFront;
                    ViewBag.hfEtcAntennaNameRear = laneData.EtcAntennaNameRear;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetLane " + ex);
            }
            return View("LaneListPopUp", laneData);
        }
        #endregion

        #region Vehicle Class Section
        public ActionResult ClassificationList()
        {
            List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");

            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "VehicleClass");
                ViewData["vehicleclassDataList"] = VehicleClassBLL.GetAll();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load Classification List " + ex.Message.ToString());
            }
            return View();
        }

        public JsonResult ClassificationReload()
        {
            List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                vehicleclassDataList = VehicleClassBLL.GetAll();
                result.Data = vehicleclassDataList;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Classification List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewClassification()
        {
            return View("ClassificationPopUp");
        }

        [HttpPost]
        public JsonResult AddClassification(VehicleClassCBE vehicleClass)
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
                    List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
                    vehicleclassDataList = VehicleClassBLL.GetAll();
                    List<VehicleClassCBE> NameFilter = vehicleclassDataList.FindAll(x => x.Name.ToLower() == vehicleClass.Name.ToString().ToLower());
                    if (NameFilter.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Classification name already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        #region Insert Into Customer Queue
                        vehicleClass.TMSId = 1;
                        vehicleClass.CreationDate = DateTime.Now;

                        int customerEntryId = VehicleClassBLL.Insert(vehicleClass);
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
        public JsonResult UpdateClassification(VehicleClassCBE vehicleClass)
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
                    List<VehicleClassCBE> vehicleclassDataList = new List<VehicleClassCBE>();
                    vehicleclassDataList = VehicleClassBLL.GetAll();
                    List<VehicleClassCBE> NameFilter = vehicleclassDataList.FindAll(x => x.Name.ToLower() == vehicleClass.Name.ToString().ToLower() && x.Id != vehicleClass.Id);
                    if (NameFilter.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Classification name already exists";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        #region Insert Into Customer Queue
                        vehicleClass.TMSId = 1;
                        vehicleClass.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        vehicleClass.ModificationDate = DateTime.Now;

                        VehicleClassBLL.Update(vehicleClass);
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
        public ActionResult GetClassification(int id)
        {
            VehicleClassCBE vehicleclassDataList = new VehicleClassCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {

                    vehicleclassDataList.Id = id;
                    vehicleclassDataList = VehicleClassBLL.GetVehicleClassId(vehicleclassDataList);


                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetClassification " + ex);
            }
            return View("ClassificationPopUp", vehicleclassDataList);
        }
        #endregion

        #region Fare Section
        public ActionResult FareList()
        {

            List<HardwareCBE> hardwareDataList = new List<HardwareCBE>();

            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("Logout", "Login");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Hardware");
                hardwareDataList = HardwareBLL.GetAll().Cast<HardwareCBE>().ToList();


            }
            catch (Exception ex)
            {

                HelperClass.LogMessage("Failed To Load Hardware List " + ex.Message.ToString());
            }
            return View(hardwareDataList);
        }
        #endregion

        #region Hardware Section
        public ActionResult HardwareList()
        {

            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");

            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Hardware");
                ViewData["HardwareDataList"] = HardwareBLL.GetAll();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load Hardware List " + ex.Message.ToString());
            }
            return View();
        }

        [HttpGet]
        public JsonResult HardwareReload()
        {
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                result.Data = HardwareBLL.GetAll(); ;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Classification List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HardwareNew()
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
            return View("HardwarePopUp");
        }

        [HttpPost]
        public JsonResult HardwareAdd(HardwareCBE hardware)
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
                    #region Insert Into Hardware Data
                    hardware.TMSId = 1;
                    hardware.TransferStatus = 1;
                    hardware.CreationDate = DateTime.Now;
                    int userId = HardwareBLL.Insert(hardware);
                    if (userId > 0)
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
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Hardware in Registration Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HardwareUpdate(HardwareCBE hardware)
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
                    hardware.TMSId = 1;
                    hardware.TransferStatus = 1;
                    hardware.ModificationDate = DateTime.Now;
                    hardware.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                    HardwareBLL.Update(hardware);
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
        public ActionResult GetHardware(int id)
        {
            HardwareCBE hardwareData = new HardwareCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {

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

                    hardwareData.HardwareId = id;
                    hardwareData = HardwareBLL.GetHardwareById(hardwareData);
                    #region ViewBag for DDL Values
                    ViewBag.hfHardwarePosition = hardwareData.HardwarePosition;
                    ViewBag.hfHardwareType = hardwareData.HardwareType;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetHardware " + ex);
            }
            return View("HardwarePopUp", hardwareData);
        }
        #endregion

        #region ProvinceList Section
        public ActionResult ProvinceList()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Province");
                ViewData["ProvinceList"] = ProvinceBLL.GetAll();
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load Province List " + ex.Message.ToString());
            }
            return View();
        }

        public JsonResult ProvinceReload()
        {
            List<ProvinceCBE> ProvinceDataList = new List<ProvinceCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                ProvinceDataList = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList(); ;
                result.Data = ProvinceDataList;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh Classification List " + ex.Message.ToString());
            }
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProvince(int id, string urltoken)
        {
            ProvinceCBE Province = new ProvinceCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    Province.ProvinceId = id;
                    Province = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ProvinceBLL.GetProvinceById(Province);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetProvince " + ex);
            }
            return View("ProvinceListPopUp", Province);
        }

        [HttpGet]
        public ActionResult ProvinceNew()
        {
            return View("ProvinceListPopUp");
        }

        [HttpPost]
        public JsonResult ProvinceAdd(ProvinceCBE Province)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<ProvinceCBE> PalazaDataList = new List<ProvinceCBE>();
                    PalazaDataList = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

                    List<ProvinceCBE> PlazaNamefiltered = PalazaDataList.FindAll(x => x.ProvinceName.ToLower() == Province.ProvinceName.ToLower() && x.ProvinceId != Province.ProvinceId);
                    if (PlazaNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Gantry Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        Province.TmsId = 1;
                        Province.CreationDate = DateTime.Now;
                        Province.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        string id = ProvinceBLL.Insert(Province);
                        if (id != "")
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Province in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProvinceUpdate(ProvinceCBE Province)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<ProvinceCBE> ProvinceDataList = new List<ProvinceCBE>();
                    ProvinceDataList = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

                    List<ProvinceCBE> ProvinceNamefiltered = ProvinceDataList.FindAll(x => x.ProvinceName.ToLower() == Province.ProvinceName.ToLower() && x.ProvinceId != Province.ProvinceId);
                    if (ProvinceNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "Province Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    
                    if (objResponseMessage.Count == 0)
                    {
                        Province.TmsId = 1;
                        Province.ModificationDate = DateTime.Now;
                        Province.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        ProvinceBLL.Update(Province);

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Province Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Kabupaten/Kota Section--CITY
        public ActionResult CityList()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Kabupaten/Kota");
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.CityBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                ViewData["CityListData"] = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load City List " + ex.Message.ToString());
            }
            return View();
        }

        public object CityReload()
        {
            List<CityCBE> DistrictDataList = new List<CityCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                //DistrictDataList = CityBLL.GetAll().Cast<CityCBE>().ToList(); ;
                //result.Data = DistrictDataList;
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.CityBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                result.Data = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh City List " + ex.Message.ToString());
            }
            return result.Data;
            //return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCity(int id, string urltoken)
        {
            CityCBE city = new CityCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    city.CityId = id;
                    city = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CityBLL.GetCityById(city);

                    List<SelectListItem> provincelist = new List<SelectListItem>();
                    List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

                    provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
                    {
                        provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
                    }
                    ViewBag.Province = provincelist;
                    ViewBag.OProvinceId = city.ProvinceId;
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetCity " + ex);
            }
            return View("CityListPopUp", city);
        }

        [HttpGet]
        public ActionResult CityNew()
        {
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }
            ViewBag.Province = provincelist;
            return View("CityListPopUp");
        }

        [HttpPost]
        public JsonResult CityAdd(CityCBE city)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<CityCBE> CityDataList = new List<CityCBE>();
                    CityDataList = CityBLL.GetAll().Cast<CityCBE>().ToList();

                    List<CityCBE> CityNamefiltered = CityDataList.FindAll(x => x.CityName.ToLower() == city.CityName.ToLower() && x.CityId != city.CityId);
                    if (CityNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "City Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        city.TmsId = 1;
                        city.CreationDate = DateTime.Now;
                        city.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        string id = CityBLL.Insert(city);
                        if (id != "")
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert City in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CityUpdate(CityCBE city)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<CityCBE> DistrictDataList = new List<CityCBE>();
                    DistrictDataList = CityBLL.GetAll().Cast<CityCBE>().ToList();

                    List<CityCBE> CityNamefiltered = DistrictDataList.FindAll(x => x.CityName.ToLower() == city.CityName.ToLower() && x.CityId != city.CityId);
                    if (CityNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "City Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        city.TmsId = 1;
                        city.ModificationDate = DateTime.Now;
                        city.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        CityBLL.Update(city);

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("City Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Kecamatan Section --District
        public ActionResult DistrictList()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Kabupaten/Kota");
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.DistrictBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                ViewData["District"] = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load DistrictList List " + ex.Message.ToString());
            }
            return View();
        }

        public object DistrictReload()
        {
            List<DistrictCBE> DistrictDataList = new List<DistrictCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                // DistrictDataList = DistrictBLL.GetAll().Cast<DistrictCBE>().ToList(); ;
                // result.Data = DistrictDataList;
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.DistrictBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                result.Data = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh District List " + ex.Message.ToString());
            }
            return result.Data;
           // return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDistrict(int id, string urltoken)
        {
            DistrictCBE District = new DistrictCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    List<SelectListItem> provincelist = new List<SelectListItem>();
                    List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

                    provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
                    {
                        provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
                    }
                    ViewBag.Province = provincelist;
                    
                    District.DistrictId = id;
                    //ViewBag.OCity = District.CityId;
                    District = VaaaN.MLFF.Libraries.CommonLibrary.BLL.DistrictBLL.GetDistrictById(District);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetDisrict " + ex);
            }
            return View("DistrictListPopUp", District);
        }

        [HttpGet]
        public ActionResult DistrictNew()
        {
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }
            ViewBag.Province = provincelist;

            return View("DistrictListPopUp");
        }

        [HttpPost]
        public JsonResult DistrictAdd(DistrictCBE District)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<DistrictCBE> DistrictDataList = new List<DistrictCBE>();
                    DistrictDataList = DistrictBLL.GetAll().Cast<DistrictCBE>().ToList();

                    List<DistrictCBE> DistrictNamefiltered = DistrictDataList.FindAll(x => x.DistrictName.ToLower() == District.DistrictName.ToLower() && x.DistrictId != District.DistrictId);
                    if (DistrictNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "District Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        District.TmsId = 1;
                        District.CreationDate = DateTime.Now;
                        District.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        string id = DistrictBLL.Insert(District);
                        if (id != "")
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert District in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DistrictUpdate(DistrictCBE District)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<DistrictCBE> DistrictDataList = new List<DistrictCBE>();
                    DistrictDataList = DistrictBLL.GetAll().Cast<DistrictCBE>().ToList();

                    List<DistrictCBE> ProvinceNamefiltered = DistrictDataList.FindAll(x => x.DistrictName.ToLower() == District.DistrictName.ToLower() && x.DistrictId != District.DistrictId);
                    if (ProvinceNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "District Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        District.TmsId = 1;
                        District.ModificationDate = DateTime.Now;
                        District.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        DistrictBLL.Update(District);

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Province Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Kelurahan/Desa Section--SubDistrict
        public ActionResult SubDistrictList()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            try
            {
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "Kabupaten/Kota");
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubDistrictBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                ViewData["SubDistrict"] = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To Load SubDistrictList List " + ex.Message.ToString());
            }
            return View();
        }

        public object SubDistrictReload()
        {
            List<SubDistrictCBE> SubDistrictDataList = new List<SubDistrictCBE>();
            JsonResult result = new JsonResult();
            if (Session["LoggedUserId"] == null)
            {
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "logout";
                objResponseMessage.Add(objModelState);
                result.Data = objResponseMessage;
            }
            try
            {
                //SubDistrictDataList = SubDistrictBLL.GetAll().Cast<SubDistrictCBE>().ToList(); ;
                //result.Data = SubDistrictDataList;
                string Det = JsonConvert.SerializeObject(VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubDistrictBLL.GetAll_DT(), Formatting.Indented);
                Det = Det.Replace("\r", "").Replace("\n", "");
                result.Data = Det;
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed To refresh SubDistrict List " + ex.Message.ToString());
            }
            return result.Data;
            //return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSubDistrict(int id, string urltoken)
        {
            SubDistrictCBE SubDistrict = new SubDistrictCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else {
                    List<SelectListItem> provincelist = new List<SelectListItem>();
                    List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

                    provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
                    {
                        provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
                    }
                    ViewBag.Province = provincelist;

                   // ViewBag.ODistrictId = SubDistrict.DistrictId;
                    SubDistrict.SubDistrictId = id;
                    SubDistrict = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubDistrictBLL.GetSubDistrictById(SubDistrict);
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetSubDisrict " + ex);
            }
            return View("SubDistrictListPopUp", SubDistrict);
        }

        [HttpGet]
        public ActionResult SubDistrictNew()
        {
            List<SelectListItem> provincelist = new List<SelectListItem>();
            List<ProvinceCBE> province = ProvinceBLL.GetAll().Cast<ProvinceCBE>().ToList();

            provincelist.Add(new SelectListItem() { Text = "", Value = "0" });
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE cr in province)
            {
                provincelist.Add(new SelectListItem() { Text = cr.ProvinceName, Value = System.Convert.ToString(cr.ProvinceId) });
            }
            ViewBag.Province = provincelist;
            return View("SubDistrictListPopUp");
        }

        [HttpPost]
        public JsonResult SubDistrictAdd(SubDistrictCBE SubDistrict)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<SubDistrictCBE> SubDistrictDataList = new List<SubDistrictCBE>();
                    SubDistrictDataList = SubDistrictBLL.GetAll().Cast<SubDistrictCBE>().ToList();

                    List<SubDistrictCBE> SubDistrictNamefiltered = SubDistrictDataList.FindAll(x => x.SubDistrictName.ToLower() == SubDistrict.SubDistrictName.ToLower() && x.SubDistrictId != SubDistrict.SubDistrictId);
                    if (SubDistrictNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "SubDistrict Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }
                    if (objResponseMessage.Count == 0)
                    {
                        SubDistrict.TmsId = 1;
                        SubDistrict.CreationDate = DateTime.Now;
                        SubDistrict.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        string id = SubDistrictBLL.Insert(SubDistrict);
                        if (id != "")
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
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert SubDistrict in SetUp Controller" + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubDistrictUpdate(SubDistrictCBE SubDistrict)
        {
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);
                }
                else
                {
                    List<SubDistrictCBE> SubDistrictDataList = new List<SubDistrictCBE>();
                    SubDistrictDataList = SubDistrictBLL.GetAll().Cast<SubDistrictCBE>().ToList();

                    List<SubDistrictCBE> SubDistrictNamefiltered = SubDistrictDataList.FindAll(x => x.SubDistrictName.ToLower() == SubDistrict.SubDistrictName.ToLower() && x.SubDistrictId != SubDistrict.SubDistrictId);
                    if (SubDistrictNamefiltered.Count > 0)
                    {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "SubDistrict Name already exists.";
                        objResponseMessage.Add(objModelState);
                    }

                    if (objResponseMessage.Count == 0)
                    {
                        SubDistrict.TmsId = 1;
                        SubDistrict.ModificationDate = DateTime.Now;
                        SubDistrict.ModifierId = Convert.ToInt16(Session["LoggedUserId"]);
                        SubDistrictBLL.Update(SubDistrict);

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "success";
                        objResponseMessage.Add(objModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("SubDistrict Update " + ex);
                ModelStateList objModelState = new ModelStateList();
                objModelState.ErrorMessage = "Something went wrong";
                objResponseMessage.Add(objModelState);
            }
            return Json(objResponseMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}