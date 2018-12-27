using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using MLFFWebUI.Models;
using System.Threading;
using System.Globalization;
using System.Web.Security;

namespace MLFFWebUI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            UserCBE objUserLogin = new UserCBE();
            TempData["Message"] = "";
            return View(objUserLogin);
        }

        #region Login and Logout Section
        [HttpPost]
        public ActionResult Index(UserCBE objUserCBE)
        {

            if (ModelState.IsValid)
            {
                UserCBE user = new UserCBE();
                user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.ValidateUser(objUserCBE.LoginName, objUserCBE.Password);
                if (user != null)
                {
                    if (user.user_status != true)
                    {
                        TempData["Message"] = "User is inactive";
                        return View();
                    }
                    else if (user.AccountExpiryDate <= DateTime.Now)
                    {
                        TempData["Message"] = "User is expired.";
                        return View();
                    }
                    else
                    {
                        Session["LoggedUserId"] = user.UserId;
                        Session["RoleId"] = user.RoleId;
                        Session["UserName"] = user.FirstName;
                        Session["LastLogin"] = user.UserId;
                        Session["RoleName"] = user.RoleName;
                        Session["ProjectName"] = "MLFF";

                        // save the login time in to db
                        int login_Id = 0;
                        login_Id = UserBLL.User_Insert_LoginInfo(user.UserId);

                        Session["LoginId"] = login_Id;
                        //TempData["Message"] = "Ok";
                        HelperClass.LogMessage("User validated successfully.");
                        #region Cookie For Language
                        //if (LanguageAbbreviation != null)
                        //{
                        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbreviation);
                        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbreviation);
                        //}
                        //HttpCookie cookie = new HttpCookie("Language");
                        //cookie.Value = LanguageAbbreviation;
                        //Response.Cookies.Add(cookie);
                        #endregion
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    TempData["Message"] = "Invalid User Id or Password or User not found";
                    HelperClass.LogMessage("Invalid User Id or Password or User not found.");
                    return View();
                }

            }
            else {
                TempData["Message"] = "Indicates a warning that might need attention.";
                return View();
            }
        }

      

        public ActionResult Logout()
        {
            try
            {
                if (Session["LoginId"] != null && Session["LoggedUserId"] != null)
                {
                    int atmsId = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentTMSId();

                    UserBLL.User_update_LoginInfo(Convert.ToInt32(Session["LoggedUserId"]), Convert.ToInt32(Session["LoginId"]));

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
            return RedirectToAction("Index");
        }
        #endregion
    }
}