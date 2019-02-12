using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class PlazaDAL
    {
        static string tableName = "TBL_PLAZA";
        public PlazaDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            int plazaId = plaza.PlazaId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PLAZA_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                if (plazaId <= 0)
                {
                    plazaId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, plaza.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, plazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_name", DbType.String, plaza.PlazaName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_location", DbType.String, plaza.Location, ParameterDirection.Input, 20));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IPADDRESS", DbType.String, plaza.IpAddress, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LONGITUDE", DbType.Decimal, plaza.Longitude, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LATITUDE", DbType.Decimal, plaza.Latitude, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, plaza.CreationDate, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return plazaId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PLAZA_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, plaza.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, plaza.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_name", DbType.String, plaza.PlazaName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_location", DbType.String, plaza.Location, ParameterDirection.Input, 20));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ipaddress", DbType.String, plaza.IpAddress, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, plaza.ModifierId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LONGITUDE", DbType.Decimal, plaza.Longitude, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LATITUDE", DbType.Decimal, plaza.Latitude, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, plaza.ModificationDate, ParameterDirection.Input, 100));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PLAZA_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, plaza.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, plaza.PlazaId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection GetAllAsCollection()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PLAZA_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                plazas = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return plazas;
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> GetAllAsList()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE>();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "PLAZA_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                plaza = ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return plaza;
        }

        public static CBE.PlazaCBE GetPlazaById(CBE.PlazaCBE plaza)
        {
            try
            {

                CBE.PlazaCollection plazas = new CBE.PlazaCollection();
                string spName = Constants.oraclePackagePrefix + "PLAZA_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plaza.PlazaId, ParameterDirection.Input));
                plazas = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return plazas[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection objs = GetAllAsCollection();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].PlazaId;
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        plaza.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        plaza.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["PLAZA_NAME"] != DBNull.Value)
                        plaza.PlazaName = Convert.ToString(dt.Rows[i]["PLAZA_NAME"]);

                    if (dt.Rows[i]["LOCATION"] != DBNull.Value)
                        plaza.Location = Convert.ToString(dt.Rows[i]["LOCATION"]);

                    if (dt.Rows[i]["IPADDRESS"] != DBNull.Value)
                        plaza.IpAddress = Convert.ToString(dt.Rows[i]["IPADDRESS"]);

                    if (dt.Rows[i]["LONGITUDE"] != DBNull.Value)
                        plaza.Longitude = Convert.ToDecimal(dt.Rows[i]["LONGITUDE"]);

                    if (dt.Rows[i]["LATITUDE"] != DBNull.Value)
                        plaza.Latitude = Convert.ToDecimal(dt.Rows[i]["LATITUDE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        plaza.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        plaza.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        plaza.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);
                    plazas.Add(plaza);
                }
                return plazas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plazaList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza in plazas)
            {
                plazaList.Add(plaza);
            }

            return plazaList;
        }
        #endregion
    }
}
