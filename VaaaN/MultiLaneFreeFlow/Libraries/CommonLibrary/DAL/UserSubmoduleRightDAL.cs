
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

    public class UserSubmoduleRightDAL
    {
        static string tableName = "tbl_user_submodule_right";
        public UserSubmoduleRightDAL()
        {
        }

        #region Insert/Update/Delete
        public static void Insert(CBE.UserSubmoduleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERSUBMODRIGHT_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userRight.UserId, ParameterDirection.Input));
               command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_activity_entry_id", DbType.Int32, userRight.SubModuleActivityEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(CBE.UserSubmoduleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERSUBMODRIGHT_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userRight.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_activity_entry_id", DbType.Int32, userRight.SubModuleActivityEntryId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteUserSubmoduleRightByUserId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubmoduleRightCBE userRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERSUBMODRIGHT_DELETEBYUSERID";
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
        private static CBE.UserSubmoduleRightCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.UserSubmoduleRightCollection userRights = new CBE.UserSubmoduleRightCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.UserSubmoduleRightCBE userRight = new CBE.UserSubmoduleRightCBE();

                    if (dt.Rows[i]["USER_ID"] != DBNull.Value)
                        userRight.UserId = Convert.ToInt32(dt.Rows[i]["USER_ID"]);

           
                    if (dt.Rows[i]["SUBMODULE_ACTIVITY_ENTRY_ID"] != DBNull.Value)
                        userRight.SubModuleActivityEntryId = Convert.ToInt32(dt.Rows[i]["SUBMODULE_ACTIVITY_ENTRY_ID"]);

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
        public static CBE.UserSubmoduleRightCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USER_SUBMODULE_RIGHT_GETALL";
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
