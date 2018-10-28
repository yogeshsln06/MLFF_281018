
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
    /// Summary description for RoleRightDAL
    /// </summary>

    public class RoleModuleRightDAL
    {
        public RoleModuleRightDAL()
        {
        }

        static string tableName = "TBL_role_module_right";

        #region Insert/Update/Delete

        /// <summary>
        /// Insert or Update Rights for Role & module
        /// </summary>
        /// <param name="module"></param>
        public static void InsertUpdateRoleModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE module)
        {
            string spName = string.Empty;
            try
            {
                if (module.Id == 0)
                {
                    // Insert rec
                    spName = Constants.oraclePackagePrefix + "ROLEMODRIGHT_INSERT";
                   
                }
                else
                {
                    //update rec
                    spName = Constants.oraclePackagePrefix + "ROLEMODRIGHT_UPDATE";
                   
                }
                
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ROLE_ID", DbType.Int32, module.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, module.Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_ID", DbType.Int32, module.ModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_ADD", DbType.Int32, module.ModuleAdd ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_VIEW", DbType.Int32, module.ModuleView ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_EDIT", DbType.Int32, module.ModuleEdit ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_DELETE", DbType.Int32, module.ModuleDelete ? 1 : 0, ParameterDirection.Input, 100));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Insert(CBE.RoleModuleRightCBE roleModuleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolemodright_insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleModuleRight.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_activity_entry_id", DbType.Int32, roleModuleRight.ModuleActivityEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteRoleRightByRoleId(CBE.RoleModuleRightCBE roleModuleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolemodright_deletebyroleid";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleModuleRight.RoleId, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Delete(CBE.RoleModuleRightCBE roleModuleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolemodright_delete";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleModuleRight.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_module_activity_entry_id", DbType.Int32, roleModuleRight.ModuleActivityEntryId, ParameterDirection.Input));
              
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static CBE.RoleModuleRightCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                CBE.RoleModuleRightCollection roleRights = new CBE.RoleModuleRightCollection();
                string spName = Constants.oraclePackagePrefix + "rolemodright_getall";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                roleRights = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return roleRights;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE> GetRoleModuleRightByRoleId(Int32 role_Id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE> modules;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                CBE.RoleModuleRightCollection roleRights = new CBE.RoleModuleRightCollection();
                string spName = Constants.oraclePackagePrefix + "MODULEACTIVITY_BYROLEID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, role_Id, ParameterDirection.Input));
                modules = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return modules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }

        private static CBE.RoleModuleRightActivityCBE CreateObjectFromDataRow(DataRow dr)
        {
            CBE.RoleModuleRightActivityCBE module = new CBE.RoleModuleRightActivityCBE();

            if (dr["ENTRY_ID"] != DBNull.Value)
                module.Id = Convert.ToInt32(dr["ENTRY_ID"]);

            if (dr["ROLE_ID"] != DBNull.Value)
                module.RoleId = Convert.ToInt32(dr["ROLE_ID"]);

            if (dr["MODULE_ID"] != DBNull.Value)
                module.ModuleId = Convert.ToInt32(dr["MODULE_ID"]);

            if (dr["MODULE_NAME"] != DBNull.Value)
                module.ModuleName = Convert.ToString(dr["MODULE_NAME"]);

            if (dr["MODULE_VIEW"] != DBNull.Value)
                module.ModuleView = Convert.ToBoolean(dr["MODULE_VIEW"]);

            if (dr["MODULE_ADD"] != DBNull.Value)
                module.ModuleAdd = Convert.ToBoolean(dr["MODULE_ADD"]);

            if (dr["MODULE_EDIT"] != DBNull.Value)
                module.ModuleEdit = Convert.ToBoolean(dr["MODULE_EDIT"]);

            if (dr["MODULE_DELETE"] != DBNull.Value)
                module.ModuleDelete = Convert.ToBoolean(dr["MODULE_DELETE"]);

            return module;
        }
        #endregion

        #region Helper Methods
        private static CBE.RoleModuleRightCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.RoleModuleRightCollection roleModuleRights = new CBE.RoleModuleRightCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.RoleModuleRightCBE roleModuleRight = new CBE.RoleModuleRightCBE();

                    if (dt.Rows[i]["ROLE_ID"] != DBNull.Value)
                        roleModuleRight.RoleId = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]);

                 

                    if (dt.Rows[i]["MODULE_ACTIVITY_ENTRY_ID"] != DBNull.Value)
                        roleModuleRight.ModuleActivityEntryId = Convert.ToInt32(dt.Rows[i]["MODULE_ACTIVITY_ENTRY_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        roleModuleRight.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    roleModuleRights.Add(roleModuleRight);
                }
                return roleModuleRights;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
