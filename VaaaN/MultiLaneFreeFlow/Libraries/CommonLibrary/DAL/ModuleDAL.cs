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
    /// Summary description for ModuleDAL
    /// </summary>


    public class ModuleDAL
    {
        static string tableName = "tbl_module";
        public ModuleDAL()
        {
        }

        #region insert/update/delete
        public static void Insert(CBE.ModuleCBE module)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix +"Module_Insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_name", DbType.String, module.ModuleName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_gui_visible", DbType.Int32, module.IsGuiVisible, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(CBE.ModuleCBE module)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix +"Module_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, module.ModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_name", DbType.String, module.ModuleName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_gui_visible", DbType.Int32, module.IsGuiVisible, ParameterDirection.Input));
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
        public static CBE.ModuleCBE GetModuleById(CBE.ModuleCBE module)
        {
            try
            {
                CBE.ModuleCollection modules = new CBE.ModuleCollection();

                string spName = Constants.oraclePackagePrefix +"Module_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, module.ModuleId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                modules = ConvertDataTableToCollection(dt);

                return modules[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This will returns all modules which has view rights for specific user.
        /// </summary>
        /// <returns></returns>
        public static CBE.ModuleCollection GetByUserId(int userId)
        {
            CBE.ModuleCollection modules = new CBE.ModuleCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "MODULE_GETBYUSER";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, userId, ParameterDirection.Input));
                  DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                modules = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return modules;
        }

        public static CBE.ModuleCollection GetAll()
        {
            CBE.ModuleCollection modules = new CBE.ModuleCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.

                string spName = Constants.oraclePackagePrefix +"Module_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                  DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                modules = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return modules;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE GetModuleById(int moduelId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE();
            try
            {
                CBE.ModuleCollection modules = new CBE.ModuleCollection();

                string spName = Constants.oraclePackagePrefix + "Module_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_id", DbType.Int32, module.ModuleId, ParameterDirection.Input));
                 DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];

                if (dt != null && dt.Rows.Count>0)
                {
                    module.ModuleId = moduelId;
                    module.ModuleName = Convert.ToString(dt.Rows[0]["MODULE_NAME"]);
                    module.IsGuiVisible = Convert.ToInt32(dt.Rows[0]["IS_GUI_VISIBLE"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return module;
        }

        #endregion

        #region Helper Methods
        private static CBE.ModuleCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.ModuleCollection modules = new CBE.ModuleCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.ModuleCBE module = new CBE.ModuleCBE();

                    if (dt.Rows[i]["MODULE_ID"] != DBNull.Value)
                        module.ModuleId = Convert.ToInt32(dt.Rows[i]["MODULE_ID"]);

                 

                    if (dt.Rows[i]["MODULE_NAME"] != DBNull.Value)
                        module.ModuleName = Convert.ToString(dt.Rows[i]["MODULE_NAME"]);

                    if (dt.Rows[i]["IS_GUI_VISIBLE"] != DBNull.Value)
                        module.IsGuiVisible = Convert.ToInt32(dt.Rows[i]["IS_GUI_VISIBLE"]);

                    if (dt.Rows[i]["MODULE_URL"] != DBNull.Value)
                        module. ModuleUrl = Convert.ToString(dt.Rows[i]["MODULE_URL"]);


                    modules.Add(module);

                }
                return modules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
