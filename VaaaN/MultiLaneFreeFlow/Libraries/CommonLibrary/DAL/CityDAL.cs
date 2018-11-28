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
                citys= ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return citys;
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
