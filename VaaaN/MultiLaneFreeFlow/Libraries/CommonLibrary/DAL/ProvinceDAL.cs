using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class ProvinceDAL
    {
        static string tableName = "TBL_PROVINCE";

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection provinces = new CBE.ProvinceCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PROVINCE_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                provinces = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return provinces;
        }

        public static CBE.ProvinceCBE GetProvinceById(CBE.ProvinceCBE Province)
        {
            try
            {

                CBE.ProvinceCBECollection Provinces = new CBE.ProvinceCBECollection();
                string spName = Constants.oraclePackagePrefix + "PROVINCE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, Province.ProvinceId, ParameterDirection.Input));
                Provinces = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return Provinces[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Insert(CBE.ProvinceCBE Province)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "PROVINCE_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //int ProvinceId = 0;
                //ProvinceId = GetNextValue();
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, Province.TmsId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_NAME", DbType.String, Province.ProvinceName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, Province.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, Province.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, Province.ModificationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Update(CBE.ProvinceCBE Province)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "Province_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, Province.ProvinceId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_NAME", DbType.String, Province.ProvinceName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, Province.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, Province.ModificationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));



                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region HelperMethods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection provinces = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE province = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        province.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PROVINCE_ID"] != DBNull.Value)
                        province.ProvinceId = Convert.ToInt32(dt.Rows[i]["PROVINCE_ID"]);

                    if (dt.Rows[i]["PROVINCE_NAME"] != DBNull.Value)
                        province.ProvinceName = Convert.ToString(dt.Rows[i]["PROVINCE_NAME"]);

                    if (dt.Rows[i]["PROVINCE_CODE"] != DBNull.Value)
                        province.ProvinceCode = Convert.ToInt32(dt.Rows[i]["PROVINCE_CODE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        province.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        province.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        province.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                   
                    provinces.Add(province);
                }
                return provinces;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                CBE.ProvinceCBECollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].ProvinceId;
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
    }
}
