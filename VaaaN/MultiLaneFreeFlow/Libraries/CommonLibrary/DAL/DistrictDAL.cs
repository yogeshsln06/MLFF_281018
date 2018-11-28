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
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "DISTRICT_GETALL";

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
