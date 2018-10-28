
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

    public class RoleSubmoduleRightDAL
    {
        public RoleSubmoduleRightDAL()
        {
        }

        static string tableName = "tbl_role_submodule_right";

        #region Insert/Update/Delete

        /// <summary>
        /// Insert  or Update Role-Sub Modules Right
        /// </summary>
        /// <param name="submoduleActvity"></param>
        public static void InsertUpdateRoleSubModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE submoduleActvity)
        {
            string spName = string.Empty;
            try
            {
                if (submoduleActvity.Id == 0)
                {
                    // Insert rec
                    spName = Constants.oraclePackagePrefix + "ROLESUBMODRIGHT_INSERT";
                }
                else
                {
                    //update rec
                    spName = Constants.oraclePackagePrefix + "ROLESUBMODRIGHT_UPDATE";
                }

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ROLE_ID", DbType.Int32, submoduleActvity.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, submoduleActvity.Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUBMODULE_ID", DbType.Int32, submoduleActvity.SubModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_MODULE_ADD", DbType.Int32, submoduleActvity.SubModuleAdd ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_MODULE_VIEW", DbType.Int32, submoduleActvity.SubModuleView ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_MODULE_EDIT", DbType.Int32, submoduleActvity.SubModuleEdit ? 1 : 0, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_MODULE_DELETE", DbType.Int32, submoduleActvity.SubModuleDelete ? 1 : 0, ParameterDirection.Input, 100));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Insert(CBE.RoleSubmoduleRightCBE roleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolesubmodright_insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleRight.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_activity_entry_id", DbType.Int32, roleRight.SubModuleActivityEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void DeleteRoleRightByRoleId(CBE.RoleSubmoduleRightCBE roleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolesubmodright_deletebyroleid";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleRight.RoleId, ParameterDirection.Input));
                
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Delete(CBE.RoleSubmoduleRightCBE roleRight)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "rolesubmodright_delete";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, roleRight.RoleId, ParameterDirection.Input));
               command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_activity_entry_id", DbType.Int32, roleRight.SubModuleActivityEntryId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static CBE.RoleSubmoduleRightCollection GetAll()
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.

                string spName = Constants.oraclePackagePrefix + "rolesubmodright_getall";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
               
                return ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE> GetRoleSubModuleRightByRoleId(Int32 role_Id, Int32 moduleId)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE> subModules;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "ROLESUBMODACT_GETBYROLEID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ROLE_ID", DbType.Int32, role_Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_ID", DbType.Int32, moduleId, ParameterDirection.Input));

                subModules = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return subModules;
        }


        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }

        private static CBE.RoleSubModuleRightActivityCBE CreateObjectFromDataRow(DataRow dr)
        {
            CBE.RoleSubModuleRightActivityCBE submoduleActivity = new CBE.RoleSubModuleRightActivityCBE();

            if (dr["ENTRY_ID"] != DBNull.Value)
                submoduleActivity.Id = Convert.ToInt32(dr["ENTRY_ID"]);

            if (dr["ROLE_ID"] != DBNull.Value)
                submoduleActivity.RoleId = Convert.ToInt32(dr["ROLE_ID"]);

            if (dr["MODULE_ID"] != DBNull.Value)
                submoduleActivity.ModuleId = Convert.ToInt32(dr["MODULE_ID"]);

            if (dr["MODULE_NAME"] != DBNull.Value)
                submoduleActivity.ModuleName = Convert.ToString(dr["MODULE_NAME"]);

            if (dr["SUBMODULE_ID"] != DBNull.Value)
                submoduleActivity.SubModuleId = Convert.ToInt32(dr["SUBMODULE_ID"]);

            if (dr["SUBMODULE_NAME"] != DBNull.Value)
                submoduleActivity.SubModuleName = Convert.ToString(dr["SUBMODULE_NAME"]);

            if (dr["SUB_MODULE_VIEW"] != DBNull.Value)
                submoduleActivity.SubModuleView = Convert.ToBoolean(dr["SUB_MODULE_VIEW"]);

            if (dr["SUB_MODULE_ADD"] != DBNull.Value)
                submoduleActivity.SubModuleAdd = Convert.ToBoolean(dr["SUB_MODULE_ADD"]);

            if (dr["SUB_MODULE_EDIT"] != DBNull.Value)
                submoduleActivity.SubModuleEdit = Convert.ToBoolean(dr["SUB_MODULE_EDIT"]);

            if (dr["SUB_MODULE_DELETE"] != DBNull.Value)
                submoduleActivity.SubModuleDelete = Convert.ToBoolean(dr["SUB_MODULE_DELETE"]);

            return submoduleActivity;
        }
        #endregion

        #region Helper Methods
        private static CBE.RoleSubmoduleRightCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.RoleSubmoduleRightCollection roleRights = new CBE.RoleSubmoduleRightCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.RoleSubmoduleRightCBE roleRight = new CBE.RoleSubmoduleRightCBE();

                    if (dt.Rows[i]["ROLE_ID"] != DBNull.Value)
                        roleRight.RoleId = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]);

                 
                    if (dt.Rows[i]["SUBMODULE_ACTIVITY_ENTRY_ID"] != DBNull.Value)
                        roleRight.SubModuleActivityEntryId = Convert.ToInt32(dt.Rows[i]["SUBMODULE_ACTIVITY_ENTRY_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        roleRight.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    roleRights.Add(roleRight);
                }
                return roleRights;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
