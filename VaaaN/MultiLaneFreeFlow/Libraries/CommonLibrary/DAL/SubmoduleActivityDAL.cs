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
    /// Summary description for SubsubmoduleActivityDAL
    /// </summary>

    public class SubmoduleActivityDAL
    {
        public SubmoduleActivityDAL()
        {
        }

        static string tableName = "tbl_submodule_activity";

        #region Insert/Update/Delete
        public static void Insert(CBE.SubmoduleActivityCBE submoduleActivity)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "SUBMODULEACTIVITY_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, GetNextValue(), ParameterDirection.Input));
               command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_subModule_id", DbType.Int32, submoduleActivity.SubModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_activity_id", DbType.Int32, submoduleActivity.ActivityId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(CBE.SubmoduleActivityCBE submoduleActivity)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "SUBMODULEACTIVITY_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, submoduleActivity.EntryId, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static CBE.SubmoduleActivityCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                CBE.SubmoduleActivityCollection moduleActivities = new CBE.SubmoduleActivityCollection();
                string spName = Constants.oraclePackagePrefix + "SUBMODULE_ACTIVITY_GETALL";
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
                CBE.SubmoduleActivityCollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].EntryId;
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
        private static CBE.SubmoduleActivityCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.SubmoduleActivityCollection submoduleActivities = new CBE.SubmoduleActivityCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.SubmoduleActivityCBE submoduleActivity = new CBE.SubmoduleActivityCBE();

                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        submoduleActivity.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);

                    if (dt.Rows[i]["SUBMODULE_ID"] != DBNull.Value)
                        submoduleActivity.SubModuleId = Convert.ToInt32(dt.Rows[i]["SUBMODULE_ID"]);

                    if (dt.Rows[i]["ACTIVITY_ID"] != DBNull.Value)
                        submoduleActivity.ActivityId = Convert.ToInt32(dt.Rows[i]["ACTIVITY_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        submoduleActivity.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    submoduleActivities.Add(submoduleActivity);
                }
                return submoduleActivities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
