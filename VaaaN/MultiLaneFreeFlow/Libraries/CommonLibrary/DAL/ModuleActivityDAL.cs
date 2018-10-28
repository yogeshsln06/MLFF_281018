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
    /// Summary description for ModuleActivityDAL
    /// </summary>

    public class ModuleActivityDAL
    {
        public ModuleActivityDAL()
        {
        }

        static string tableName = "tbl_module_activity";

        #region Insert/Update/Delete
        public static void Insert(CBE.ModuleActivityCBE moduleActivity)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix +"Module_Activity_Insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
        //command.Parameters.Add(VaaaN.ATMS.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, moduleActivity.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, moduleActivity.ModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_activity_id", DbType.Int32, moduleActivity.ActivityId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(CBE.ModuleActivityCBE moduleActivity)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix +"Module_Activity_Delete";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, moduleActivity.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, moduleActivity.ModuleId, ParameterDirection.Input));
             
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static CBE.ModuleActivityCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                CBE.ModuleActivityCollection moduleActivities = new CBE.ModuleActivityCollection();
                string spName = Constants.oraclePackagePrefix + "Module_Activity_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                 DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                moduleActivities = ConvertDataTableToCollection(dt);

                return moduleActivities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        private static CBE.ModuleActivityCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.ModuleActivityCollection moduleActivities = new CBE.ModuleActivityCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.ModuleActivityCBE moduleActivity = new CBE.ModuleActivityCBE();
                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        moduleActivity.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);
                    if (dt.Rows[i]["MODULE_ID"] != DBNull.Value)
                        moduleActivity.ModuleId = Convert.ToInt32(dt.Rows[i]["MODULE_ID"]);

                    if (dt.Rows[i]["ACTIVITY_ID"] != DBNull.Value)
                        moduleActivity.ActivityId = Convert.ToInt32(dt.Rows[i]["ACTIVITY_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        moduleActivity.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    moduleActivities.Add(moduleActivity);
                }
                return moduleActivities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
