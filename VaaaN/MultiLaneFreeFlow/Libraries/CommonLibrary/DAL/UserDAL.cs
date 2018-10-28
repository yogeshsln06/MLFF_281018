
#region Copyright message
/*
© copyright 2011 VaaaN Infra (P) Ltd. All rights reserved.

This file contains Proprietary information of VaaaN Infra (P) Ltd.
and should be treated as proprietary and confidential.

The use, copying, disclosure or modification of the programs and/or 
source code is prohibited unless otherwise provided for in the license 
or confidential agreements.

========================================================
Author           :  VaaaN Infra                  
Company          :  VaaaN Infra     
Date of Creation :                              
========================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    /// <summary>
    /// Summary description for UserDAL
    /// </summary>

    public class UserDAL
    {
        static string tableName = "TBL_USER";
        public UserDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(CBE.UserCBE user)
        {
            try
            {
                int userId = 0;
                userId = GetNextValue();
                string spName = Constants.oraclePackagePrefix + "User_Insert";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                // Will be incremented by sequence number
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, user.LoginName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, user.FirstName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, user.LastName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, user.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_password", DbType.String, user.Password, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, user.Address, ParameterDirection.Input, 200));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, user.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_acc_expiry_date", DbType.DateTime, user.AccountExpiryDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, user.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, user.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, user.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_finger_print_1", DbType.String, user.FingerPrint1, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MOBILE_NO", DbType.String, user.MobileNo, ParameterDirection.Input, 10));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EMAIL_ID", DbType.String, user.EmailId, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_STATUS", DbType.Int32, user.UserStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DOB", DbType.DateTime, user.UserDob, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                return userId = (int)command.Parameters["p_user_id"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string UpdatePassword(string oldPassword, string changedPassword, int userId, string email_id)
        {
            string result = string.Empty;
            try
            {

                byte[] saltBytes;

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[8];
                string encOldPassword = oldPassword;
                if (oldPassword != "none")
                {
                    encOldPassword = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(oldPassword);/* Common.PasswordHashSaltMD5.CreateHash(oldPassword, "MD5", saltBytes);*/
                }
                string encChangedPassword = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(changedPassword);/*.PasswordHashSaltMD5.CreateHash(changedPassword, "MD5", saltBytes);*/
                string spName = Constants.oraclePackagePrefix + "UpdatePassword";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_old_password", DbType.String, encOldPassword, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_changed_password", DbType.String, encChangedPassword, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, email_id, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_returnmsg", DbType.String, "", ParameterDirection.Output, 30));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                result = (string)command.Parameters["p_returnmsg"].Value;

            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.WriteGeneralLog("Failed to Update password" + ex);
            }

            return result;
        }
        public static void Update(CBE.UserCBE user)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "User_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atms_id", DbType.Int32, Constants.GetCurrentTMSId(), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, user.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, user.LoginName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, user.FirstName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, user.LastName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, user.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, user.Address, ParameterDirection.Input, 200));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, user.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_acc_expiry_date", DbType.DateTime, user.AccountExpiryDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, user.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, user.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, user.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_finger_print_1", DbType.String, user.FingerPrint1, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_STATUS", DbType.Int32, user.user_status, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MOBILE_NO", DbType.String, user.MobileNo, ParameterDirection.Input, 10));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EMAIL_ID", DbType.String, user.EmailId, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DOB", DbType.DateTime, user.UserDob, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateWithPassword(CBE.UserCBE user)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "User_UpdateWithPassword";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, user.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, user.LoginName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_first_name", DbType.String, user.FirstName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_last_name", DbType.String, user.LastName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, user.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_password", DbType.String, user.Password, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_address", DbType.String, user.Address, ParameterDirection.Input, 200));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, user.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_acc_expiry_date", DbType.DateTime, user.AccountExpiryDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, user.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, user.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, user.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_finger_print_1", DbType.String, user.FingerPrint1, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(CBE.UserCBE user)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "User_Delete";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, user.UserId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int User_Insert_LoginInfo(int userId)
        {
            try
            {
                int entryId = 0;
                string spName = Constants.oraclePackagePrefix + "USER_INSERT_LOGININFO";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                // Will be incremented by sequence number
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_date", DbType.DateTime, DateTime.Now, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                return entryId = (int)command.Parameters["p_entry_id"].Value;
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.WriteGeneralLog(ex.Message.ToString());
                throw ex;
            }
        }

        public static void User_update_LoginInfo(int userId, int loginId)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USER_UPDATE_LOGININFO";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                // Will be incremented by sequence number
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, loginId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_logout_date", DbType.DateTime, DateTime.Now, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static string ValidateLoginName(string loginName, string email_id)
        {
            string result = string.Empty;
            try
            {
                string spName = Constants.oraclePackagePrefix + "ValidateLoginName";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                // Will be incremented by sequence number
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, loginName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, email_id, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_returnmsg", DbType.String, "", ParameterDirection.Output, 30));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                result = (string)command.Parameters["p_returnmsg"].Value;
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.WriteGeneralLog(ex.Message.ToString());
                throw ex;
            }
            return result;
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> GetAll(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, ref Int32 RecordCount)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> users;
            try
            {
                string spName = Constants.oraclePackagePrefix + "USER_GETALL_PAGING";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.Int32, info.CurrentPageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.Int32, info.PageSize, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SEARCH_TEXT", DbType.String, info.SearchText, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RECORD_COUNT", DbType.Int32, info.RecordCount, ParameterDirection.Output));

                DataTable dt = (VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                users = ConvertTableToIEnurable(dt);
                RecordCount = dt.Rows.Count;//Convert.ToInt32(command.Parameters["P_RECORD_COUNT"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public static CBE.UserCBE GetUserById(CBE.UserCBE user)
        {
            try
            {

                CBE.UserCollection users = new CBE.UserCollection();

                string spName = Constants.oraclePackagePrefix + "User_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, user.UserId, ParameterDirection.Input));

                users = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return users[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE GetUserByEmailId(string email_id)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "GetUserByEmailId";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_email_id", DbType.String, email_id, ParameterDirection.Input, 100));

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection users = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                if (users.Count > 0)
                {
                    return users[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CBE.UserCollection GetUserByLoginName(string loginName)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USER_GETBYNAME";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, loginName, ParameterDirection.Input, 100));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[0];
                return ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE ValidateUser(string loginName, string password)
        {
            try
            {
                byte[] saltBytes;

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[8];

                string tempPw = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(password);
                string spName = Constants.oraclePackagePrefix + "USER_VALIDATEUSER";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_login_name", DbType.String, loginName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PASSWORD", DbType.String, tempPw, ParameterDirection.Input, 500));

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection users = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                if (users.Count > 0)
                {
                    return users[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.WriteGeneralLog(ex.Message.ToString());
                return null;
            }
        }
        public static CBE.UserCollection GetUserAll()
        {
            CBE.UserCollection users = new CBE.UserCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "User_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                users = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public static DataTable dtGetUserAll()
        {
            DataTable users = new DataTable();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "User_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                users = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }
        public static CBE.UserCollection PagedGetAll(int startRowIndex, int endRowIndex, ref int totalRows)
        {
            CBE.UserCollection users = new CBE.UserCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "User_PagedGetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_start_row_index", DbType.Int32, startRowIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_end_row_index", DbType.Int32, endRowIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_total_rows", DbType.Int32, startRowIndex, ParameterDirection.Output));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                totalRows = (int)command.Parameters["p_total_rows"].Value;
                users = ConvertDataTableToCollection(ds.Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        /// <summary>
        /// Get next value for the primary key
        /// </summary>
        /// <returns></returns>
        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                CBE.UserCollection objs = GetUserAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].UserId;
                }

                //Sort the object id
                Array.Sort(sortedObjsId);

                for (int j = 0; j < sortedObjsId.Length; j++)
                {
                    if (j + 1 < sortedObjsId.Length)
                    {
                        if (sortedObjsId[j] + 1 < sortedObjsId[j + 1])
                        {
                            nextValue = sortedObjsId[j] + 1;
                            break;
                        }
                    }
                    else
                    {
                        nextValue = sortedObjsId[sortedObjsId.Length - 1] + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nextValue;
        }
        #endregion

        #region Helper Methods

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }

        private static CBE.UserCBE CreateObjectFromDataRow(DataRow dr)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();

            if (dr["USER_ID"] != DBNull.Value)
                user.UserId = Convert.ToInt32(dr["USER_ID"]);

            //if (dr["organization_id"] != DBNull.Value)
            //    user.OrganizationId = Convert.ToInt32(dr["organization_id"]);

            if (dr["login_name"] != DBNull.Value)
                user.LoginName = Convert.ToString(dr["login_name"]);

            if (dr["first_name"] != DBNull.Value)
                user.FirstName = Convert.ToString(dr["first_name"]);

            if (dr["last_name"] != DBNull.Value)
                user.LastName = Convert.ToString(dr["last_name"]);
            if (dr["role_name"] != DBNull.Value)
                user.RoleName = Convert.ToString(dr["role_name"]);

            return user;
        }
        private static CBE.UserCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.UserCollection users = new CBE.UserCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.UserCBE user = new CBE.UserCBE();

                    if (dt.Rows[i]["USER_ID"] != DBNull.Value)
                        user.UserId = Convert.ToInt32(dt.Rows[i]["USER_ID"]);
                    if (dt.Rows[i]["LOGIN_NAME"] != DBNull.Value)
                        user.LoginName = Convert.ToString(dt.Rows[i]["LOGIN_NAME"]);

                    if (dt.Rows[i]["FIRST_NAME"] != DBNull.Value)
                        user.FirstName = Convert.ToString(dt.Rows[i]["FIRST_NAME"]);

                    if (dt.Rows[i]["LAST_NAME"] != DBNull.Value)
                        user.LastName = Convert.ToString(dt.Rows[i]["LAST_NAME"]);

                    user.FullName = Convert.ToString(dt.Rows[i]["FIRST_NAME"]) + " " + Convert.ToString(dt.Rows[i]["LAST_NAME"]);

                    if (dt.Rows[i]["DESCRIPTION"] != DBNull.Value)
                        user.Description = Convert.ToString(dt.Rows[i]["DESCRIPTION"]);

                    if (dt.Rows[i]["PASSWORD"] != DBNull.Value)
                        user.Password = Convert.ToString(dt.Rows[i]["PASSWORD"]);

                    if (dt.Rows[i]["ADDRESS"] != DBNull.Value)
                        user.Address = Convert.ToString(dt.Rows[i]["ADDRESS"]);

                    if (dt.Rows[i]["ROLE_ID"] != DBNull.Value)
                        user.RoleId = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]);

                    if (user.RoleId > 0)
                    {
                        foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role in VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll())
                        {
                            if (user.RoleId == role.RoleId)
                            {
                                user.RoleName = role.RoleName;
                                break;
                            }
                        }
                    }

                    if (dt.Rows[i]["ACC_EXPIRY_DATE"] != DBNull.Value)
                        user.AccountExpiryDate = Convert.ToDateTime(dt.Rows[i]["ACC_EXPIRY_DATE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        user.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        user.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        user.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);
                    if (dt.Rows[i]["USER_STATUS"] != DBNull.Value)
                        user.UserStatus = Convert.ToBoolean(dt.Rows[i]["USER_STATUS"]);
                    if (dt.Rows[i]["EMAIL_ID"] != DBNull.Value)
                        user.EmailId = Convert.ToString(dt.Rows[i]["EMAIL_ID"]);
                    if (dt.Rows[i]["MOBILE_NO"] != DBNull.Value)
                        user.MobileNo = Convert.ToString(dt.Rows[i]["MOBILE_NO"]);
                    if (dt.Rows[i]["MOBILE_NO"] != DBNull.Value)
                        user.MobileNo = Convert.ToString(dt.Rows[i]["MOBILE_NO"]);
                    if (dt.Rows[i]["DOB"] != DBNull.Value)
                        user.UserDob = Convert.ToDateTime(dt.Rows[i]["DOB"]);
                    users.Add(user);
                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
