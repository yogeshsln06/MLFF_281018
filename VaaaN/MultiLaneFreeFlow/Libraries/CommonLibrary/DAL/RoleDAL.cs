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
    /// Summary description for RoleDAL
    /// </summary>

    public class RoleDAL
    {
        static string tableName = "TBL_ROLE";
        public RoleDAL()
        {
        }

        #region Insert/Update/Delete
        public static string Insert(CBE.RoleCBE role)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix +"Role_Insert";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                int roleId = 0;
                roleId = GetNextValue();
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_name", DbType.String, role.RoleName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, role.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, role.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, role.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, role.ModificationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_ACTIVE", DbType.Int32, role.ISActive, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output,100));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Update(CBE.RoleCBE role, string old_rolename)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix +"Role_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, role.RoleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_name", DbType.String, role.RoleName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_description", DbType.String, role.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, role.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, role.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, role.ModificationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_active", DbType.Int32, role.ISActive, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_OLD_ROLE_NAME", DbType.String, old_rolename, ParameterDirection.Input, 100));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(CBE.RoleCBE role)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix +"Role_Delete";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, role.RoleId, ParameterDirection.Input));
               
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static DataTable GetRoleById(CBE.RoleCBE role)
        {
            try
            {
                CBE.RoleCollection roles = new CBE.RoleCollection();

                string spName = Constants.oraclePackagePrefix +"Role_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, role.RoleId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                //roles = ConvertDataTableToCollection(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE GetRoleByIdCollection(CBE.RoleCBE role)
        {
            try
            {
                CBE.RoleCollection roles = new CBE.RoleCollection();

                string spName = Constants.oraclePackagePrefix + "Role_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_id", DbType.Int32, role.RoleId, ParameterDirection.Input));
                //DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                // DataTable dt = ds.Tables[tableName];
                //roles = ConvertDataTableToCollection(dt);
                roles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return roles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.RoleCollection GetRoleByName(CBE.RoleCBE role)
        {
            try
            {
                CBE.RoleCollection roles = new CBE.RoleCollection();

                string spName = Constants.oraclePackagePrefix +"Role_GetByName";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_role_name", DbType.String, role.RoleName, ParameterDirection.Input, 100));
               
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                roles = ConvertDataTableToCollection(dt);

                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.RoleCollection GetRoleAll()
        {
            CBE.RoleCollection roles = new CBE.RoleCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "Role_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                roles = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public static CBE.RoleCollection PagedGetAll(int startRowIndex, int endRowIndex, ref int totalRows)
        {
            CBE.RoleCollection roles = new CBE.RoleCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix +"Role_PagedGetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_start_row_index", DbType.Int32, startRowIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_end_row_index", DbType.Int32, endRowIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_total_rows", DbType.Int32, startRowIndex, ParameterDirection.Output));
                 DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                totalRows = (int)command.Parameters["p_total_rows"].Value;

                roles = ConvertDataTableToCollection(ds.Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> GetRoleList(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, ref Int32 RecordCount)
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> roles;
            try
            {
                string spName = Constants.oraclePackagePrefix + "Role_GetAll_Paging";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.Int32, info.CurrentPageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.Int32, info.PageSize, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SEARCH_TEXT", DbType.String, info.SearchText, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RECORD_COUNT", DbType.Int32, info.RecordCount, ParameterDirection.Output));

                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];

                roles = ConvertTableToIEnurable(dt);
                RecordCount = dt.Rows.Count;//Convert.ToInt32(command.Parameters["P_RECORD_COUNT"].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> GetAll()
        {
            IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> roles;
            try
            {
                string spName = Constants.oraclePackagePrefix + "Role_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                roles = ConvertTableToIEnurable(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
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
                CBE.RoleCollection objs = GetRoleAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].RoleId;
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
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> ConvertTableToIEnurable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return CreateObjectFromDataRow(row);
            }
        }
        private static CBE.RoleCBE CreateObjectFromDataRow(DataRow dr)
        {
            CBE.RoleCBE role = new CBE.RoleCBE();

            if (dr["ROLE_ID"] != DBNull.Value)
                role.RoleId = Convert.ToInt32(dr["ROLE_ID"]);

            if (dr["ROLE_NAME"] != DBNull.Value)
                role.RoleName = Convert.ToString(dr["ROLE_NAME"]);

            if (dr["IS_ACTIVE"] != DBNull.Value)
                role.ISActive = Convert.ToInt32(dr["IS_ACTIVE"]);
            return role;
        }
        private static CBE.RoleCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                CBE.RoleCollection roles = new CBE.RoleCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CBE.RoleCBE role = new CBE.RoleCBE();

                    if (dt.Rows[i]["ROLE_ID"] != DBNull.Value)
                        role.RoleId = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]);

                  

                    if (dt.Rows[i]["ROLE_NAME"] != DBNull.Value)
                        role.RoleName = Convert.ToString(dt.Rows[i]["ROLE_NAME"]);

                    if (dt.Rows[i]["DESCRIPTION"] != DBNull.Value)
                        role.Description = Convert.ToString(dt.Rows[i]["DESCRIPTION"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        role.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        role.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        role.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["IS_ACTIVE"] != DBNull.Value)
                    role.ISActive = Convert.ToInt32(dt.Rows[i]["IS_ACTIVE"]);

                    roles.Add(role);
                }
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
