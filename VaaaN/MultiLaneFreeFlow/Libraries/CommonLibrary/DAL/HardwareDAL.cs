using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class HardwareDAL
    {
        static string tableName = "TBL_HARDWARE";
        public HardwareDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            int hardwareId = hardware.HardwareId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "HARDWARE_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                if (hardwareId <= 0)
                {
                    hardwareId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, hardware.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, hardware.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_ID", DbType.Int32, hardwareId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_NAME", DbType.String, hardware.HardwareName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_TYPE", DbType.Int32, hardware.HardwareType, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_POSITION", DbType.Int32, hardware.HardwarePosition, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MANUFACTURER_NAME", DbType.String, hardware.ManufacturerName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODEL_NAME", DbType.String, hardware.ModelName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IP_ADDRESS", DbType.String, hardware.IpAddress, ParameterDirection.Input, 20));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, hardware.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, hardware.TransferStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return hardwareId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "HARDWARE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, hardware.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, hardware.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_ID", DbType.Int32, hardware.HardwareId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_NAME", DbType.String, hardware.HardwareName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_TYPE", DbType.Int32, hardware.HardwareType, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_POSITION", DbType.Int32, hardware.HardwarePosition, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MANUFACTURER_NAME", DbType.String, hardware.ManufacturerName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODEL_NAME", DbType.String, hardware.ModelName, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IP_ADDRESS", DbType.String, hardware.IpAddress, ParameterDirection.Input, 20));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIED_BY", DbType.Int32, hardware.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, hardware.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, hardware.TransferStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "HARDWARE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                hardwares = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hardwares;
        }

        public static CBE.HardwareCBE GetHardwareById(CBE.HardwareCBE hardware)
        {
            try
            {

                CBE.HardwareCollection hardwares = new CBE.HardwareCollection();

                string spName = Constants.oraclePackagePrefix + "HARDWARE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_ID", DbType.Int32, hardware.HardwareId, ParameterDirection.Input));
                hardwares = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return hardwares[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.HardwareCBE GetHardwareByType(CBE.HardwareCBE hardware)
        {
            try
            {

                CBE.HardwareCollection hardwares = new CBE.HardwareCollection();

                string spName = Constants.oraclePackagePrefix + "HARDWARE_GETBYTYPE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_HARDWARE_TYPE", DbType.Int32, hardware.HardwareType, ParameterDirection.Input));
                hardwares = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return hardwares[0];
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
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].HardwareId;
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection GetLatestHardwares(DateTime lastCollectionUpdateTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "HARDWARE_LATEST_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_UPDATE_TIME", DbType.DateTime, lastCollectionUpdateTime, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                hardwares = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hardwares;
        }
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwareCollection = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        hardware.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        hardware.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["HARDWARE_ID"] != DBNull.Value)
                        hardware.HardwareId = Convert.ToInt32(dt.Rows[i]["HARDWARE_ID"]);

                    if (dt.Rows[i]["HARDWARE_NAME"] != DBNull.Value)
                        hardware.HardwareName = Convert.ToString(dt.Rows[i]["HARDWARE_NAME"]);

                    if (dt.Rows[i]["HARDWARE_TYPE"] != DBNull.Value)
                        hardware.HardwareType = Convert.ToInt32(dt.Rows[i]["HARDWARE_TYPE"]);
                        hardware.HardwareTypeName = GetEnumFieldName(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Constants.HardwareType), VaaaN.MLFF.Libraries.CommonLibrary.Constants.HardwareTypeName, hardware.HardwareType - 1);

                    if (dt.Rows[i]["HARDWARE_POSITION"] != DBNull.Value)
                        hardware.HardwarePosition = Convert.ToInt32(dt.Rows[i]["HARDWARE_POSITION"]);

                    if (dt.Rows[i]["MANUFACTURER_NAME"] != DBNull.Value)
                        hardware.ManufacturerName = Convert.ToString(dt.Rows[i]["MANUFACTURER_NAME"]);

                    if (dt.Rows[i]["MODEL_NAME"] != DBNull.Value)
                        hardware.ModelName = Convert.ToString(dt.Rows[i]["MODEL_NAME"]);

                    if (dt.Rows[i]["IP_ADDRESS"] != DBNull.Value)
                        hardware.IpAddress = Convert.ToString(dt.Rows[i]["IP_ADDRESS"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        hardware.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        hardware.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        hardware.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        hardware.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    hardwareCollection.Add(hardware);
                }
                return hardwareCollection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static String GetEnumFieldName(Type type, String[] arFieldName, int value)
        {
            String result = String.Empty;

            Array ar = Enum.GetValues(type);

            for (int i = 0; i < ar.Length; i++)
            {
                if (i == value)
                {
                    result = arFieldName[i];
                    break;
                }
            }

            return result;
        }
        #endregion
    }
}
