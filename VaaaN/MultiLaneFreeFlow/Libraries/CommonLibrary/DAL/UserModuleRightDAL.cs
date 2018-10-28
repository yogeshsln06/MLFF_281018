
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
using System.Data;
using System.Data.Common;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    /// <summary>
    /// Summary description for UserRightDAL
    /// </summary>

    public class UserModuleRightDAL
    {
        static string tableName = "tbl_user_module_right";
        public UserModuleRightDAL()
        {
        }

        #region Insert/Update/Delete
        public static void Insert(CBE.UserModuleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERMODRIGHT_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userRight.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_activity_entry_id", DbType.Int32, userRight.ModuleActivityEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void Delete(CBE.UserModuleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERMODRIGHT_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userRight.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_activity_entry_id", DbType.Int32, userRight.ModuleActivityEntryId, ParameterDirection.Input));
               VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteUserModuleRightByUserId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERMODRIGHT_DELETEBYUSERID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userRight.UserId, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        private static CBE.UserModuleRightCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.UserModuleRightCollection userRights = new CBE.UserModuleRightCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.UserModuleRightCBE userRight = new CBE.UserModuleRightCBE();

                    if (dt.Rows[i]["USER_ID"] != DBNull.Value)
                        userRight.UserId = Convert.ToInt32(dt.Rows[i]["USER_ID"]);

                
                    if (dt.Rows[i]["MODULE_ACTIVITY_ENTRY_ID"] != DBNull.Value)
                        userRight.ModuleActivityEntryId = Convert.ToInt32(dt.Rows[i]["MODULE_ACTIVITY_ENTRY_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        userRight.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    userRights.Add(userRight);
                }
                return userRights;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static CBE.UserModuleRightCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USER_MODULE_RIGHT_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
               
                return ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
