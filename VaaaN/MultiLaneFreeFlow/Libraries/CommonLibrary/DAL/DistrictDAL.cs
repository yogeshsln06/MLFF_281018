using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class DistrictDAL
    {
        static string tableName = "TBL_DISTRICT";

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection districts = new CBE.DistrictCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "District_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                districts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return districts;
        }

        public static DataTable GetAll_DT()
        {
            DataTable dt = new DataTable();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "District_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection GetByCityId(Libraries.CommonLibrary.CBE.DistrictCBE district)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection districts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "DISTRICT_GETBYCITYID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, district.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CITY_ID", DbType.Int32, district.CityId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                districts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return districts;
        }

        public static CBE.DistrictCBE GetDistrictById(CBE.DistrictCBE District)
        {
            try
            {
                CBE.DistrictCBECollection Districtes = new CBE.DistrictCBECollection();
                string spName = Constants.oraclePackagePrefix + "DISTRICT_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DISTRICT_ID", DbType.Int32, District.DistrictId, ParameterDirection.Input));
                Districtes = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return Districtes[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Insert(CBE.DistrictCBE District)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "PROVINCE_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //int ProvinceId = 0;
                //ProvinceId = GetNextValue();
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, District.TmsId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_NAME", DbType.String, District.DistrictName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, District.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, District.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, District.ModificationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Update(CBE.DistrictCBE District)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "Province_Update";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_ID", DbType.Int32, District.DistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROVINCE_NAME", DbType.String, District.DistrictName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, District.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, District.ModificationDate, ParameterDirection.Input, 100));
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection districts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE district = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        district.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["CITY_ID"] != DBNull.Value)
                        district.CityId = Convert.ToInt32(dt.Rows[i]["CITY_ID"]);

                    if (dt.Rows[i]["DISTRICT_ID"] != DBNull.Value)
                        district.DistrictId = Convert.ToInt32(dt.Rows[i]["DISTRICT_ID"]);

                    if (dt.Rows[i]["DISTRICT_NAME"] != DBNull.Value)
                        district.DistrictName = Convert.ToString(dt.Rows[i]["DISTRICT_NAME"]);

                    if (dt.Rows[i]["DISTRICT_CODE"] != DBNull.Value)
                        district.DistrictCode = Convert.ToInt32(dt.Rows[i]["DISTRICT_CODE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        district.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        district.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        district.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);


                    districts.Add(district);
                }
                return districts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
