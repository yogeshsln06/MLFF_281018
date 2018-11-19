using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class VehicleClassMappingDAL
    {
        static string tableName = "TBL_VEHICLE_CLASS_MAPPING";
        public VehicleClassMappingDAL()
        {
        }

        #region Insert/Update
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE  vehicle)
        {
            int entryId = 0;

            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                if (entryId <= 0)
                {
                    entryId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MAPPING_VEHICLE_CLASS_ID", DbType.Int32, entryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MLFF_VEHICLE_CLASS_ID", DbType.Int32, vehicle.MLFFVehicleClassId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_ID", DbType.Int32, vehicle.ANPRVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_NAME", DbType.String, vehicle.ANPRVehicleClassName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, vehicle.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE vehicle)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MAPPING_VEHICLE_CLASS_ID", DbType.Int32, vehicle.MappingVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MLFF_VEHICLE_CLASS_ID", DbType.Int32, vehicle.MLFFVehicleClassId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_ID", DbType.Int32, vehicle.ANPRVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_NAME", DbType.String, vehicle.ANPRVehicleClassName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, vehicle.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIED_BY", DbType.Int32, vehicle.ModifiedBy, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Methods

        public static CBE.VehicleClassMappingCBE GetVehicleClassMappingById(CBE.VehicleClassMappingCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.VehicleClassMappingCollection vehicles = new CBE.VehicleClassMappingCollection();

                string spName = Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MAPPING_VEHICLE_CLASS_ID", DbType.Int32, vehicle.MappingVehicleClassId, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.VehicleClassMappingCollection GetVehicleClassMappingByIdCollection(CBE.VehicleClassMappingCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.VehicleClassMappingCollection vehicles = new CBE.VehicleClassMappingCollection();

                string spName = Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MAPPING_VEHICLE_CLASS_ID", DbType.Int32, vehicle.MappingVehicleClassId, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection GetAllAsCollection()
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection vehicles = new CBE.VehicleClassMappingCollection();

                string spName = Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> GetAllAsList()
        {
            try
            {

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> vehicles = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE>();

                string spName = Constants.oraclePackagePrefix + "VEHICLE_CLASS_MAPPING_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                vehicles = ConvertDataTableToList(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public static CBE.VehicleClassMappingCBE GetVehicleClassByANPRName(CBE.VehicleClassMappingCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.VehicleClassMappingCollection vehicles = new CBE.VehicleClassMappingCollection();

                string spName = Constants.oraclePackagePrefix + "VEHICLECLASS_MAPPING_GETBYNAME";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_NAME", DbType.String, vehicle.ANPRVehicleClassName, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.VehicleClassMappingCollection GetVehicleClassByANPRNameCollection(CBE.VehicleClassMappingCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.VehicleClassMappingCollection vehicles = new CBE.VehicleClassMappingCollection();

                string spName = Constants.oraclePackagePrefix + "VEHICLECLASS_MAPPING_GETBYNAME";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPR_VEHICLE_CLASS_NAME", DbType.Int32, vehicle.ANPRVehicleClassName, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Helper Methods
        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> objs = GetAllAsList();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].MappingVehicleClassId;
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection vehicles = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE vehicle = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        vehicle.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["MAPPING_VEHICLE_CLASS_ID"] != DBNull.Value)
                        vehicle.MappingVehicleClassId = Convert.ToInt32(dt.Rows[i]["MAPPING_VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["MLFF_VEHICLE_CLASS_ID"] != DBNull.Value)
                        vehicle.MLFFVehicleClassId = Convert.ToInt32(dt.Rows[i]["MLFF_VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["ANPR_VEHICLE_CLASS_ID"] != DBNull.Value)
                        vehicle.ANPRVehicleClassId = Convert.ToInt32(dt.Rows[i]["ANPR_VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["ANPR_VEHICLE_CLASS_NAME"] != DBNull.Value)
                        vehicle.ANPRVehicleClassName = Convert.ToString(dt.Rows[i]["ANPR_VEHICLE_CLASS_NAME"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        vehicle.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        vehicle.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        vehicle.ModifiedBy = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        vehicle.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    if (dt.Rows[i]["MLFF_VEHICLE_CLASS_NAME"] != DBNull.Value)
                        vehicle.MLFFVehicleClassName = Convert.ToString(dt.Rows[i]["MLFF_VEHICLE_CLASS_NAME"]);

                    vehicles.Add(vehicle);
                }
                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection customerVehicles = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> customerVehicleList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE lane in customerVehicles)
            {
                customerVehicleList.Add(lane);
            }

            return customerVehicleList;
        }
        #endregion
    }
}
