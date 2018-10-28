using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CustomerVehicleDAL
    {
        static string tableName = "TBL_CUSTOMER_VEHICLE";
        public CustomerVehicleDAL()
        {
        }

        #region Insert/Update
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle)
        {
            int entryId = 0;

            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                if (entryId <= 0)
                {
                    entryId = GetNextValue();
                }
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, entryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, vehicle.AccountId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_reg_no", DbType.String, vehicle.VehRegNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tag_id", DbType.String, vehicle.TagId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicle.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, vehicle.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, vehicle.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_id", DbType.Int32, vehicle.AccountId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_reg_no", DbType.String, vehicle.VehRegNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tag_id", DbType.String, vehicle.TagId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicle.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, vehicle.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, vehicle.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modified_by", DbType.Int32, vehicle.ModifiedBy, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));


                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Methods

        public static CBE.CustomerVehicleCBE GetUserById(CBE.CustomerVehicleCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "Customer_Vehicle_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, vehicle.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicle.VehicleClassId, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.CustomerVehicleCollection GetUserByAccountId(CBE.CustomerVehicleCBE vehicle) // what user ???? CJS
        {
            try
            {

                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "Customer_Vehicle_GetById";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, vehicle.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_vehicle_class_id", DbType.Int32, vehicle.VehicleClassId, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetAllAsCollection()
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> GetAllAsList()
        {
            try
            {

                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> vehicles = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();

                string spName = Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                vehicles = ConvertDataTableToList(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.CustomerVehicleCollection GetUserAll() //what user ???? CJS
        {
            CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "Customer_Vehicle_GetAll";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vehicles;
        }

        public static CBE.CustomerVehicleCBE GetByTagId(String TagId)
        {
            CBE.CustomerVehicleCollection customerVehicles = new CBE.CustomerVehicleCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_GETBYTAGID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TAG_ID", DbType.String, TagId, ParameterDirection.Input));

                customerVehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerVehicles[0];
        }

        public static CBE.CustomerVehicleCBE GetByTansactionCrosstalkEntryId (int crosstalkEntryId)
        {
            CBE.CustomerVehicleCollection customerVehicles = new CBE.CustomerVehicleCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "CV_GET_BY_TRANCTPENTRYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRAN_CT_EN_ID", DbType.Int32, crosstalkEntryId, ParameterDirection.Input));

                customerVehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerVehicles[0];
        }

        public static CBE.CustomerVehicleCBE GetCustomerVehicleById(CBE.CustomerVehicleCBE vehicle) 
        {
            try
            {

                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "CUSTOMERVEHICLE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, vehicle.EntryId, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);

                return vehicles[0];
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
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> objs = GetAllAsList();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].EntryId;
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
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection vehicles = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        vehicle.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        vehicle.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);

                    if (dt.Rows[i]["ACCOUNT_ID"] != DBNull.Value)
                        vehicle.AccountId = Convert.ToInt32(dt.Rows[i]["ACCOUNT_ID"]);

                    if (dt.Rows[i]["VEH_REG_NO"] != DBNull.Value)
                        vehicle.VehRegNo = Convert.ToString(dt.Rows[i]["VEH_REG_NO"]);

                    if (dt.Rows[i]["TAG_ID"] != DBNull.Value)
                        vehicle.TagId = Convert.ToString(dt.Rows[i]["TAG_ID"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_ID"] != DBNull.Value)
                        vehicle.VehicleClassId = Convert.ToInt32(dt.Rows[i]["VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        vehicle.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        vehicle.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        vehicle.ModifiedBy = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        vehicle.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_NAME"] != DBNull.Value)
                        vehicle.VehicleClassName = Convert.ToString(dt.Rows[i]["VEHICLE_CLASS_NAME"]);

                    vehicles.Add(vehicle);
                }
                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> ConvertDataTableToList(DataTable dt)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection customerVehicles = ConvertDataTableToCollection(dt);

            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> customerVehicleList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE lane in customerVehicles)
            {
                customerVehicleList.Add(lane);
            }

            return customerVehicleList;
        }
        #endregion
    }
}
