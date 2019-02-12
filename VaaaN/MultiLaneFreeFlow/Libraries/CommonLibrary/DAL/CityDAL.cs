using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CityDAL
    {
        static string tableName = "TBL_CITY";

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection GetByProvinceId(Libraries.CommonLibrary.CBE.CityCBE city)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection citys = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CITY_GETBYPROVINCEID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, city.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, city.ProvinceId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                citys = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return citys;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection citys = new CBE.CityCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CITY_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                citys = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return citys;
        }

        public static CBE.CityCBE GetCityById(CBE.CityCBE city)
        {
            try
            {
                CBE.CityCBECollection cities = new CBE.CityCBECollection();
                string spName = Constants.oraclePackagePrefix + "CITY_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CITY_ID", DbType.Int32, city.CityId, ParameterDirection.Input));
                cities = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return cities[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Insert(CBE.CityCBE City)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "CITY_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, City.TmsId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, City.ProvinceId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CITY_NAME", DbType.String, City.CityName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, City.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, City.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Update(CBE.CityCBE city)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "CITY_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CITY_ID", DbType.Int32, city.CityId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, city.ProvinceId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CITY_NAME", DbType.String, city.CityName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, city.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, city.ModificationDate, ParameterDirection.Input, 100));
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection citys = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE city = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        city.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PROVINCE_ID"] != DBNull.Value)
                        city.ProvinceId = Convert.ToInt32(dt.Rows[i]["PROVINCE_ID"]);

                    if (dt.Rows[i]["CITY_ID"] != DBNull.Value)
                        city.CityId = Convert.ToInt32(dt.Rows[i]["CITY_ID"]);

                    if (dt.Rows[i]["CITY_NAME"] != DBNull.Value)
                        city.CityName = Convert.ToString(dt.Rows[i]["CITY_NAME"]);

                    if (dt.Rows[i]["CITY_CODE"] != DBNull.Value)
                        city.CityCode = Convert.ToInt32(dt.Rows[i]["CITY_CODE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        city.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        city.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        city.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);


                    citys.Add(city);
                }
                return citys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
