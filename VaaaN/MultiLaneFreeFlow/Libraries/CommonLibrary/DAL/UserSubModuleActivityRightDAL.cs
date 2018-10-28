using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    class UserSubModuleActivityRightDAL
    {
        static string tableName = "TBL_USER_SUBMODULE_RIGHT";

        public static void InsertUpdateUserSubModuleActivityRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERSUBMOD_ACT_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, subModule.Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_user_id", DbType.Int32, subModule.UserId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_submodule_id", DbType.Int32, subModule.SubModuleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_module_view", DbType.Int32, subModule.SubModuleView, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_module_add", DbType.Int32, subModule.SubModuleAdd, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_module_edit", DbType.Int32, subModule.SubModuleEdit, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sub_module_delete", DbType.Int32, subModule.SubModuleDelete, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE GetSubModuleRightById(Int32 Id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> subModules;
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule = null;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USERSUBMOD_ACT_GETBYENTRYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, Id, ParameterDirection.Input));
                subModules = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                if (subModules.Count() > 0)
                {
                    subModule = subModules.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return subModule;
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> GetUserSubModuleRightByUserIdAndModuleId(Int32 user_Id, Int32 module_id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> sub_modules_right;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USERSUBMOD_ACT_BYUSERID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_ID", DbType.Int32, user_Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODULE_ID", DbType.Int32, module_id, ParameterDirection.Input));

                sub_modules_right = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
             }
            catch (Exception ex)
            {
                throw ex;
            }
            return sub_modules_right;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE GetSubmoduleRightByUserIdandSubmoduleId(Int32 user_Id, Int32 submodule_id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> subModules_right;
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule = null;
            try
            {

                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USERSUBRIGHT_BYUSERIDSUBMODID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_ID", DbType.Int32, user_Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUBMODULE_ID", DbType.Int32, submodule_id, ParameterDirection.Input));

                subModules_right = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                if (subModules_right.Count() > 0)
                {
                    subModule = subModules_right.First();
                }

                            }
            catch (Exception ex)
            {
                throw ex;
            }
            return subModule;
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }

        private static CBE.UserSubModuleActivityRightCBE CreateObjectFromDataRow(DataRow dr)
        {
            CBE.UserSubModuleActivityRightCBE subModuleRights = new CBE.UserSubModuleActivityRightCBE();

            if (dr["ENTRY_ID"] != DBNull.Value)
                subModuleRights.Id = Convert.ToInt32(dr["ENTRY_ID"]);

            if (dr["USER_ID"] != DBNull.Value)
                subModuleRights.UserId = Convert.ToInt32(dr["USER_ID"]);

            if (dr["MODULE_ID"] != DBNull.Value)
                subModuleRights.ModuleId = Convert.ToInt32(dr["MODULE_ID"]);

            if (dr["MODULE_NAME"] != DBNull.Value)
                subModuleRights.ModuleName = Convert.ToString(dr["MODULE_NAME"]);

            if (dr["SUBMODULE_ID"] != DBNull.Value)
                subModuleRights.SubModuleId = Convert.ToInt32(dr["SUBMODULE_ID"]);

            if (dr["SUBMODULE_NAME"] != DBNull.Value)
                subModuleRights.SubModuleName = Convert.ToString(dr["SUBMODULE_NAME"]);

            if (dr["SUB_MODULE_VIEW"] != DBNull.Value)
                subModuleRights.SubModuleView = Convert.ToBoolean(dr["SUB_MODULE_VIEW"]);

            if (dr["SUB_MODULE_ADD"] != DBNull.Value)
                subModuleRights.SubModuleAdd = Convert.ToBoolean(dr["SUB_MODULE_ADD"]);

            if (dr["SUB_MODULE_EDIT"] != DBNull.Value)
                subModuleRights.SubModuleEdit = Convert.ToBoolean(dr["SUB_MODULE_EDIT"]);

            if (dr["SUB_MODULE_DELETE"] != DBNull.Value)
                subModuleRights.SubModuleDelete = Convert.ToBoolean(dr["SUB_MODULE_DELETE"]);

            return subModuleRights;
        }
    }
}
