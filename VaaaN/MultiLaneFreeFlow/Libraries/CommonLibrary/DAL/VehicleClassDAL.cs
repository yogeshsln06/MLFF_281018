using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class VehicleClassDAL
    {
        static string tableName = "TBL_VEHICLE_CLASS";
        public VehicleClassDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            int id = vehicleClass.Id;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                if (id <= 0)
                {
                    id = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, vehicleClass.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_name", DbType.String, vehicleClass.Name, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, vehicleClass.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, vehicleClass.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, vehicleClass.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicleClass.Id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_name", DbType.String, vehicleClass.Name, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIED_BY", DbType.Int32, vehicleClass.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, vehicleClass.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, vehicleClass.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicleClass.Id, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> GetAll()
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleClasses = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE>();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                vehicleClasses = ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vehicleClasses;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection GetAllAsCollection()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                vehicleClasses = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vehicleClasses;
        }



        public static Int32 GetVehicleByName(string VehicleName)
        {
            Int32 VehicleId = 0;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_GETBYNAME";
                
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_NAME", DbType.String, VehicleName, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                if (dt.Rows.Count > 0)
                    VehicleId = Convert.ToInt32(dt.Rows[0]["VEHICLE_CLASS_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VehicleId;
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
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].Id;
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE GetVehicleClassId(CBE.VehicleClassCBE vehicleClass)
        {
            
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection();
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, vehicleClass.Id, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                vehicleClasses = ConvertDataTableToCollection(dt);
                return vehicleClasses[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE();
                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        vehicleClass.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_ID"] != DBNull.Value)
                        vehicleClass.Id = Convert.ToInt32(dt.Rows[i]["VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_NAME"] != DBNull.Value)
                        vehicleClass.Name = Convert.ToString(dt.Rows[i]["VEHICLE_CLASS_NAME"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        vehicleClass.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        vehicleClass.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        vehicleClass.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);
                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        vehicleClass.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    vehicleClasses.Add(vehicleClass);
                }

                return vehicleClasses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> vehicleClassList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
            {
                vehicleClassList.Add(vc);
            }

            return vehicleClassList;
        }

        #endregion
    }
}
