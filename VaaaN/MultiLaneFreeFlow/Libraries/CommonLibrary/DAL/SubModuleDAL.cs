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
using System;
using System.Data;
using System.Data.Common;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    /// <summary>
    /// Summary description for SubModuleDAL
    /// </summary>
    /// 

    class SubModuleDAL
    {
        static string tableName = "tbl_submodule";
        public SubModuleDAL()
        {
        }

        #region insert/update/delete
        public static void Insert(CBE.SubmoduleCBE submodule)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "SubModule_Insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_id", DbType.Int32, GetNextValue(), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, submodule.ModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_name", DbType.String, submodule.SubModuleName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_gui_visible", DbType.Int32, submodule.IsGuiVisible, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(CBE.SubmoduleCBE submodule)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "SubModule_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_id", DbType.Int32, submodule.SubModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, submodule.ModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_name", DbType.String, submodule.SubModuleName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_gui_visible", DbType.Int32, submodule.IsGuiVisible, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Get Methods
        public static CBE.SubmoduleCBE GetSubModuleById(CBE.SubmoduleCBE submodule)
        {
            try
            {
                CBE.SubmoduleCollection submodules = new CBE.SubmoduleCollection();

                string spName = Constants.oraclePackagePrefix + "SubModule_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_id", DbType.Int32, submodule.SubModuleId, ParameterDirection.Input));

                submodules = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return submodules[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetSubModuleBySubModuleId(string sub_module_id)
        {
            DataTable ldt = new DataTable();
            try
            {
                string spName = Constants.oraclePackagePrefix + "SubModule_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_id", DbType.Int32, sub_module_id, ParameterDirection.Input));

                ldt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
            }
            catch (Exception ex)
            {
                ldt = null;
            }
            return ldt;
        }

        public static CBE.SubmoduleCollection GetAll()
        {
            CBE.SubmoduleCollection submodules = new CBE.SubmoduleCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.

                string spName = Constants.oraclePackagePrefix + "SubModule_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                submodules = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return submodules;
        }

        /// <summary>
        /// Get Sub Modules list by User Id and Moddule Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection GetByUserId(int userId, int moduleId)
        {
            CBE.SubmoduleCollection Submodules = new CBE.SubmoduleCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "SUBMODULE_GETBYUSER";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, moduleId, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                Submodules = ConvertDataTableToCollection(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Submodules;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection GetByModuleId(int moduleId)
        {
            CBE.SubmoduleCollection Submodules = new CBE.SubmoduleCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "SUBMODULE_GETBYMODULEID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, moduleId, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                Submodules = ConvertDataTableToCollection(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Submodules;
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
                CBE.SubmoduleCollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].ModuleId;
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
        private static CBE.SubmoduleCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.SubmoduleCollection submodules = new CBE.SubmoduleCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.SubmoduleCBE submodule = new CBE.SubmoduleCBE();

                    if (dt.Rows[i]["SUBMODULE_ID"] != DBNull.Value)
                        submodule.SubModuleId = Convert.ToInt32(dt.Rows[i]["SUBMODULE_ID"]);

                    if (dt.Rows[i]["MODULE_ID"] != DBNull.Value)
                        submodule.ModuleId = Convert.ToInt32(dt.Rows[i]["MODULE_ID"]);
                    if (dt.Rows[i]["SUBMODULE_NAME"] != DBNull.Value)
                        submodule.SubModuleName = Convert.ToString(dt.Rows[i]["SUBMODULE_NAME"]);

                    if (dt.Rows[i]["IS_GUI_VISIBLE"] != DBNull.Value)
                        submodule.IsGuiVisible = Convert.ToInt32(dt.Rows[i]["IS_GUI_VISIBLE"]);
                    if (dt.Rows[i]["SUBMODULE_URL"] != DBNull.Value)
                        submodule.SubmoduleUrl = Convert.ToString(dt.Rows[i]["SUBMODULE_URL"]);

                    if (dt.Rows[i]["ICON"] != DBNull.Value)
                        submodule.Icon = Convert.ToString(dt.Rows[i]["ICON"]);

                    submodules.Add(submodule);
                }
                return submodules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
