using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// This is the control Class for login process.  It is responsible for retrieve
    /// the TollUser and shift
    /// </summary>
    public class LoginBLL
    {
        public LoginBLL()
        {

        }

        /// <summary>
        /// Check login status based on username and password provided
        /// </summary>
        /// <param name="username">user login name</param>
        /// <param name="password">user password</param>
        /// <returns>return values defined in Constants.LoginResult. </returns>
        public static VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult ValidateUser(string loginName, string password, ref VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE currentUser)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.InvalidUser;
            CBE.UserCollection users = new CBE.UserCollection();

            try
            {
                users = VaaaN.MLFF.Libraries.CommonLibrary.DAL.LoginDAL.GetUserByLoginName(loginName);

                if (users.Count == 0)
                {
                    result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.InvalidUser;
                }
                else
                {
                    foreach (CBE.UserCBE user in users)
                    {
                        if (user.LoginName.ToLower() == loginName.ToLower() && user.Password == VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(password))
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.Successful;
                            currentUser = user;

                            //Check Account expiry
                            if (currentUser.AccountExpiryDate < System.DateTime.Now)
                            {
                                result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.AccountExpired;
                            }

                            
                            break;
                        }
                        else
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.InvalidUser;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.DatabaseError;
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to validate user. " + ex.ToString(), VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.LoginModule);

                throw ex;
            }
            return result;
        }
    }
}
