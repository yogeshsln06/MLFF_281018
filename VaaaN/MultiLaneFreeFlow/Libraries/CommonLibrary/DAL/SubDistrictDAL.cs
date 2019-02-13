using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class SubDistrictDAL
    {

        static string tableName = "TBL_SUB_DISTRICT";

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection subDistricts = new CBE.SubDistrictCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SUBDISTRICT_GETALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                subDistricts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return subDistricts;
        }

        public static DataTable GetAll_DT()
        {
            DataTable dt = new DataTable();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SUBDISTRICT_GETALL";
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
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection GetByDistrictId(Libraries.CommonLibrary.CBE.SubDistrictCBE subDistrict)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection subDistricts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SUBDISTRICT_GETBYDISTRICTID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, subDistrict.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DISTRICT_ID", DbType.Int32, subDistrict.DistrictId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                subDistricts = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return subDistricts;
        }


        public static CBE.SubDistrictCBE GetSubDistrictById(CBE.SubDistrictCBE SubDistrict)
        {
            try
            {
                CBE.SubDistrictCBECollection SubDistrictes = new CBE.SubDistrictCBECollection();
                string spName = Constants.oraclePackagePrefix + "SUBDISTRICT_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUBDISTRICT_ID", DbType.Int32, SubDistrict.SubDistrictId, ParameterDirection.Input));
                SubDistrictes = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return SubDistrictes[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Insert(CBE.SubDistrictCBE SubDistrict)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "SUBDISTRICT_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, SubDistrict.TmsId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DISTRICT_ID", DbType.Int32, SubDistrict.DistrictId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_DISTRICT_NAME", DbType.String, SubDistrict.SubDistrictName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, SubDistrict.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, SubDistrict.CreationDate, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RETURNMSG", DbType.String, "", ParameterDirection.Output, 100));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                return strmsg = (string)command.Parameters["P_RETURNMSG"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Update(CBE.SubDistrictCBE SubDistrict)
        {
            try
            {
                string strmsg = "";
                string spName = Constants.oraclePackagePrefix + "SUBDISTRICT_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_DISTRICT_ID", DbType.Int32, SubDistrict.SubDistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DISTRICT_ID", DbType.Int32, SubDistrict.DistrictId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SUB_DISTRICT_NAME", DbType.String, SubDistrict.SubDistrictName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, SubDistrict.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, SubDistrict.ModificationDate, ParameterDirection.Input, 100));
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection subdistricts = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE subDistrict = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        subDistrict.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["DISTRICT_ID"] != DBNull.Value)
                        subDistrict.DistrictId = Convert.ToInt32(dt.Rows[i]["DISTRICT_ID"]);

                    if (dt.Rows[i]["SUB_DISTRICT_ID"] != DBNull.Value)
                        subDistrict.SubDistrictId = Convert.ToInt32(dt.Rows[i]["SUB_DISTRICT_ID"]);

                    if (dt.Rows[i]["SUB_DISTRICT_NAME"] != DBNull.Value)
                        subDistrict.SubDistrictName = Convert.ToString(dt.Rows[i]["SUB_DISTRICT_NAME"]);

                    if (dt.Rows[i]["SUB_DISTRICT_CODE"] != DBNull.Value)
                        subDistrict.SubDistrictCode = Convert.ToInt32(dt.Rows[i]["SUB_DISTRICT_CODE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        subDistrict.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        subDistrict.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        subDistrict.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["ZIP_CODE"] != DBNull.Value)
                        subDistrict.ZipCode = Convert.ToInt32(dt.Rows[i]["ZIP_CODE"]);


                    subdistricts.Add(subDistrict);
                }
                return subdistricts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
