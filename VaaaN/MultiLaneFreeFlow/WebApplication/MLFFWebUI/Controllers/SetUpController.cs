using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
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
                ViewData["UserDataList"]= UserBLL.GetUserAll().Cast<UserCBE>().ToList();
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

        //[HttpGet]
        //public ActionResult NewUserMaster()
        //{
        //    ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
        //    return View("UserPopUp");
        //}
        [HttpGet]
        public ActionResult UserNew()
        {
            ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
            return View("UserPopUp");
        }
        [HttpPost]
        public ActionResult GetUser(int id)
        {
            UserCBE hardwareData = new UserCBE();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "logout";
                    objResponseMessage.Add(objModelState);

                }
                else {

                    //#region Gantry Class Dropdown
                    //List<SelectListItem> gantryList = new List<SelectListItem>();
                    //List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

                    //gantryList.Add(new SelectListItem() { Text = "--Select Gantry--", Value = "0" });
                    //foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                    //{
                    //    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                    //}

                    //ViewBag.Gantry = gantryList;

                    //#endregion

                    //#region Hardware Type
                    //ViewBag.HardwareType = HelperClass.GetHardwareType();
                    //#endregion

                    //#region Hardware Position
                    //ViewBag.HardwarePosition = HelperClass.GetHardwarePosition();
                    //#endregion

                    //hardwareData.HardwareId = id;
                    //hardwareData = HardwareBLL.GetHardwareById(hardwareData);
                    //#region ViewBag for DDL Values
                    //ViewBag.hfHardwarePosition = hardwareData.HardwarePosition;
                    //ViewBag.hfHardwareType = hardwareData.HardwareType;
                    //#endregion

                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("GetHardware " + ex);
            }
            return View("HardwarePopUp", hardwareData);
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
                    #region Insert Into Hardware Data
                   // hardware.TMSId = 1;
                   // hardware.TransferStatus = 1;
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
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to Insert Hardware in Registration Controller" + ex);
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
            return View("LanePopUp");
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
        public ActionResult GetLane(int id)
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
            return View("LanePopUp", laneData);
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
    }
}