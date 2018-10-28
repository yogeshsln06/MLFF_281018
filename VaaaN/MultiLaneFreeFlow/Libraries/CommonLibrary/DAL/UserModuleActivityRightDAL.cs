using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class UserModuleActivityRightDAL
    {
        static string tableName = "TBL_user_module_right";

        #region Insert/Update

        /// <summary>
        /// Insert or update Module rights by user Id and Module Id
        /// </summary>
        /// <param name="module"></param>
        public static void InsertUpdateUserModuleActivityRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE module)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERMODULE_ACT_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, module.Id, ParameterDirection.Input));
               command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_ID", DbType.Int32, module.UserId, ParameterDirection.Input));
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
        #endregion

        /// <summary>
        /// Read module right by User Id
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> GetUserModuleRightByUserId(Int32 user_Id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> rights;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "USERMOD_ACT_GETBYUSERID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_USER_ID", DbType.Int32, user_Id, ParameterDirection.Input));
                
                rights = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                
              }
            catch (Exception ex)
            {
                throw ex;
            }
            return rights;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE GetModuleRightById(Int32 Id)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> modules;
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE module = null;
            try
            {
                string spName = Constants.oraclePackagePrefix + "USERMOD_ACT_GETBYENTRYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, Id, ParameterDirection.Input));
                modules = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                
                if (modules.Count() > 0)
                {
                    module = modules.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return module;
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }

        private static CBE.UserModuleActivityRightCBE CreateObjectFromDataRow(DataRow dr)
        {
            CBE.UserModuleActivityRightCBE right = new CBE.UserModuleActivityRightCBE();

            if (dr["ENTRY_ID"] != DBNull.Value)
                right.Id = Convert.ToInt32(dr["ENTRY_ID"]);

            if (dr["USER_ID"] != DBNull.Value)
                right.UserId = Convert.ToInt32(dr["USER_ID"]);

            if (dr["MODULE_ID"] != DBNull.Value)
                right.ModuleId = Convert.ToInt32(dr["MODULE_ID"]);

            if (dr["MODULE_NAME"] != DBNull.Value)
                right.ModuleName = Convert.ToString(dr["MODULE_NAME"]);

            if (dr["MODULE_VIEW"] != DBNull.Value)
                right.ModuleView = Convert.ToBoolean(dr["MODULE_VIEW"]);

            if (dr["MODULE_ADD"] != DBNull.Value)
                right.ModuleAdd = Convert.ToBoolean(dr["MODULE_ADD"]);

            if (dr["MODULE_EDIT"] != DBNull.Value)
                right.ModuleEdit = Convert.ToBoolean(dr["MODULE_EDIT"]);

            if (dr["MODULE_DELETE"] != DBNull.Value)
                right.ModuleDelete = Convert.ToBoolean(dr["MODULE_DELETE"]);

            return right;
        }
    }
}
