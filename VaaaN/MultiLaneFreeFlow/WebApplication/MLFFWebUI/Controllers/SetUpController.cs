﻿using MLFFWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace MLFFWebUI.Controllers
{
    public class SetUpController : Controller
    {
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
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "SetUp", "UsersList");
                int userId = Convert.ToInt32(Session["LoggedUserId"]);
                userList = UserBLL.GetUserAll().Cast<UserCBE>().ToList();
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

        [HttpGet]
        public ActionResult NewUserMaster()
        {
           
            ViewBag.RoleId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });

            return View("UserMaster");
        }

        #endregion

    }
}