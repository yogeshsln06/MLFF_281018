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
        #endregion
    }
}
