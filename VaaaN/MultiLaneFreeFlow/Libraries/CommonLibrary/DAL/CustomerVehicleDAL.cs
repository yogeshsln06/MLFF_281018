using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

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
                //if (entryId <= 0)
                //{
                //    entryId = GetNextValue();
                //}
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, vehicle.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, vehicle.AccountId, ParameterDirection.InputOutput));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEH_REG_NO", DbType.String, vehicle.VehRegNo, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TAG_ID", DbType.String, vehicle.TagId, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, vehicle.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, vehicle.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_RC_NO", DbType.String, vehicle.VehicleRCNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_OWNER_NAME", DbType.String, vehicle.OwnerName, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_OWNER_ADDRESS", DbType.String, vehicle.OwnerAddress, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_BRAND", DbType.String, vehicle.Brand, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_TYPE", DbType.String, vehicle.VehicleType, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CATEGORY", DbType.String, vehicle.VehicleCategory, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODEL_NO", DbType.String, vehicle.Model, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MANUFACTURING_YEAR", DbType.Int32, vehicle.ManufacturingYear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CYCLINDER_CAPACITY", DbType.String, vehicle.CyclinderCapacity, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FRAME_NUMBER", DbType.String, vehicle.FrameNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENGINE_NUMBER", DbType.String, vehicle.EngineNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_COLOR", DbType.String, vehicle.VehicleColor, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FUEL_TYPE", DbType.Int32, vehicle.FuelType, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LICENCE_PLATE_COLOR", DbType.Int32, vehicle.LicencePlateColor, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_REGISTRATION_YEAR", DbType.Int32, vehicle.RegistrationYear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_OWNERSHIP_NO", DbType.String, vehicle.VehicleOwnershipDocumentNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LOCATION_CODE", DbType.String, vehicle.LocationCode, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_REG_QUEUE_NO", DbType.String, vehicle.RegistrationQueueNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_FRONT", DbType.String, vehicle.VehicleImageFront, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_REAR", DbType.String, vehicle.VehicleImageRear, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_RIGHT", DbType.String, vehicle.VehicleImageRight, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_LEFT", DbType.String, vehicle.VehicleImageLeft, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_RC_NO_PATH", DbType.String, vehicle.VehicleRCNumberImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EXCEPTION_FLAG", DbType.Int16, vehicle.ExceptionFlag, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STATUS", DbType.Int16, vehicle.Status, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VALID_UNTIL", DbType.Date, vehicle.ValidUntil, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TID_Front", DbType.String, vehicle.TidFront, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TID_REAR", DbType.String, vehicle.TidRear, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_BALANCE", DbType.Decimal, vehicle.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_REGISTRATION_THROUGH", DbType.Int16, vehicle.RegistartionThrough, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_DOC_VERIFIED", DbType.Int16, vehicle.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_QUEUE_STATUS", DbType.Int16, vehicle.QueueStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                entryId = Convert.ToInt32(command.Parameters["P_ENTRY_ID"].Value);
            }
            catch (Exception ex)
            {
                entryId = 0;
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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, vehicle.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modified_by", DbType.Int32, vehicle.ModifiedBy, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transfer_status", DbType.Int32, vehicle.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_RC_NO", DbType.String, vehicle.VehicleRCNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_OWNER_NAME", DbType.String, vehicle.OwnerName, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_OWNER_ADDRESS", DbType.String, vehicle.OwnerAddress, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_BRAND", DbType.String, vehicle.Brand, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_TYPE", DbType.String, vehicle.VehicleType, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CATEGORY", DbType.String, vehicle.VehicleCategory, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODEL_NO", DbType.String, vehicle.Model, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MANUFACTURING_YEAR", DbType.Int32, vehicle.ManufacturingYear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CYCLINDER_CAPACITY", DbType.String, vehicle.CyclinderCapacity, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FRAME_NUMBER", DbType.String, vehicle.FrameNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENGINE_NUMBER", DbType.String, vehicle.EngineNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_COLOR", DbType.String, vehicle.VehicleColor, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FUEL_TYPE", DbType.Int32, vehicle.FuelType, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LICENCE_PLATE_COLOR", DbType.Int32, vehicle.LicencePlateColor, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_REGISTRATION_YEAR", DbType.Int32, vehicle.RegistrationYear, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_OWNERSHIP_NO", DbType.String, vehicle.VehicleOwnershipDocumentNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LOCATION_CODE", DbType.String, vehicle.LocationCode, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_REG_QUEUE_NO", DbType.String, vehicle.RegistrationQueueNumber, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_FRONT", DbType.String, vehicle.VehicleImageFront, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_REAR", DbType.String, vehicle.VehicleImageRear, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_RIGHT", DbType.String, vehicle.VehicleImageRight, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLEIMAGE_LEFT", DbType.String, vehicle.VehicleImageLeft, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_RC_NO_PATH", DbType.String, vehicle.VehicleRCNumberImagePath, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EXCEPTION_FLAG", DbType.Int16, vehicle.ExceptionFlag, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STATUS", DbType.Int16, vehicle.Status, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VALID_UNTIL", DbType.Date, vehicle.ValidUntil, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TID_Front", DbType.String, vehicle.TidFront, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TID_REAR", DbType.String, vehicle.TidRear, ParameterDirection.Input, 255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_BALANCE", DbType.Decimal, vehicle.AccountBalance, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_DOC_VERIFIED", DbType.Int16, vehicle.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_QUEUE_STATUS", DbType.Int16, vehicle.QueueStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Decimal UpdateVehiclebalance(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle, Decimal Amount)
        {
            Decimal UpdatedBalance = vehicle.AccountBalance + Amount;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "VEHICLE_BALANCE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, vehicle.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_reg_no", DbType.String, vehicle.VehRegNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_amount", DbType.Decimal, Amount, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_account_balance", DbType.Decimal, vehicle.AccountBalance, ParameterDirection.Output));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
                UpdatedBalance = Convert.ToDecimal(command.Parameters["p_account_balance"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UpdatedBalance;
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

        public static List<CustomerVehicleCBE> CustomerVehicleAccountLazyLoad(int PageIndex, int PageSize)
        {
            try
            {
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> vehicles = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, PageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, PageSize, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                vehicles = ConvertDataTableToList(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ValidateCustomerVehicleDetails(CBE.CustomerVehicleCBE vehicle, string ResidentId)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "VALIDATE_VEHICLE_DETAILS";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEH_REG_NO", DbType.String, vehicle.VehRegNo, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RESIDENT_ID", DbType.String, ResidentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_RC_NO", DbType.String, vehicle.VehicleRCNumber, ParameterDirection.Input));
                return VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetAllVehicleinDataTable()
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_VEHICLE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                return VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
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

        public static CBE.CustomerVehicleCBE GetByTansactionCrosstalkEntryId(int crosstalkEntryId)
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

        public static DataTable GetCustomerVehicleById_DT(CBE.CustomerVehicleCBE vehicle)
        {
            try
            {

                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "CUSTOMERVEHICLE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENTRY_ID", DbType.Int32, vehicle.EntryId, ParameterDirection.Input));
                return VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CustomerVehicleCBE> GetCustomerVehicleByAccountId(int AccountId)
        {
            try
            {

                List<CustomerVehicleCBE> vehicles = new List<CustomerVehicleCBE>();

                string spName = Constants.oraclePackagePrefix + "CUSTOMERVEHICLE_GETBYACCOUNTID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, AccountId, ParameterDirection.Input));
                vehicles = ConvertDataTableToList(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return vehicles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CBE.CustomerVehicleCBE GetCustomerVehicleByVehRegNo(CBE.CustomerVehicleCBE vehicle)
        {
            CBE.CustomerVehicleCBE result = null;
            try
            {

                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                string spName = Constants.oraclePackagePrefix + "CUSTOMERVEHICLE_GETBYVEHREGNO";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEH_REG_NO", DbType.String, vehicle.VehRegNo, ParameterDirection.Input));

                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                if (vehicles.Count > 0)
                    result = vehicles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetLatestCustomerVehicles(DateTime lastCollectionUpdateTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

            try
            {
                string spName = Constants.oraclePackagePrefix + "VEHICLE_LATEST_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_UPDATE_TIME", DbType.DateTime, lastCollectionUpdateTime, ParameterDirection.Input));
                vehicles = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vehicles;
        }

        public static List<CBE.CustomerVehicleCBE> GetCustomerVehicleFiltered(string filter)
        {
            List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> vehicles = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMERVEHICLE_FILTERED";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FILTER", DbType.String, filter, ParameterDirection.Input, 2000));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                vehicles = ConvertDataTableToList(ds.Tables[0]);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vehicles;
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
                CBE.CustomerVehicleCollection vehicles = new CBE.CustomerVehicleCollection();

                foreach (DataRow dr in dt.Rows)
                {
                    vehicles.Add(ConvertDataTableToCBE(dr));
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
            List<CBE.CustomerVehicleCBE> customerVehicleList = new List<CBE.CustomerVehicleCBE>();

            foreach (DataRow dr in dt.Rows)
            {
                customerVehicleList.Add(ConvertDataTableToCBE(dr));
            }

            return customerVehicleList;
        }

        private static CBE.CustomerVehicleCBE ConvertDataTableToCBE(DataRow row)
        {

            CBE.CustomerVehicleCBE vehicle = new CBE.CustomerVehicleCBE();

            if (row["TMS_ID"] != DBNull.Value)
                vehicle.TMSId = Convert.ToInt32(row["TMS_ID"]);

            if (row["ENTRY_ID"] != DBNull.Value)
                vehicle.EntryId = Convert.ToInt32(row["ENTRY_ID"]);

            if (row["ACCOUNT_ID"] != DBNull.Value)
                vehicle.AccountId = Convert.ToInt32(row["ACCOUNT_ID"]);

            if (row["VEH_REG_NO"] != DBNull.Value)
                vehicle.VehRegNo = Convert.ToString(row["VEH_REG_NO"]);

            if (row["TAG_ID"] != DBNull.Value)
                vehicle.TagId = Convert.ToString(row["TAG_ID"]);

            if (row["VEHICLE_CLASS_ID"] != DBNull.Value)
                vehicle.VehicleClassId = Convert.ToInt32(row["VEHICLE_CLASS_ID"]);

            if (row["VEHICLE_CLASS_NAME"] != DBNull.Value)
                vehicle.VehicleClassName = Convert.ToString(row["VEHICLE_CLASS_NAME"]);

            if (row["CREATION_DATE"] != DBNull.Value)
                vehicle.CreationDate = Convert.ToDateTime(row["CREATION_DATE"]);

            if (row["MODIFICATION_DATE"] != DBNull.Value)
                vehicle.ModificationDate = Convert.ToDateTime(row["MODIFICATION_DATE"]);

            if (row["MODIFIED_BY"] != DBNull.Value)
                vehicle.ModifiedBy = Convert.ToInt32(row["MODIFIED_BY"]);

            if (row["TRANSFER_STATUS"] != DBNull.Value)
                vehicle.TransferStatus = Convert.ToInt32(row["TRANSFER_STATUS"]);

            if (row["VEHICLE_RC_NO"] != DBNull.Value)
                vehicle.VehicleRCNumber = Convert.ToString(row["VEHICLE_RC_NO"]);

            if (row["OWNER_NAME"] != DBNull.Value)
                vehicle.OwnerName = Convert.ToString(row["OWNER_NAME"]);

            if (row["OWNER_ADDRESS"] != DBNull.Value)
                vehicle.OwnerAddress = Convert.ToString(row["OWNER_ADDRESS"]);

            if (row["BRAND"] != DBNull.Value)
                vehicle.Brand = Convert.ToString(row["BRAND"]);

            if (row["VEHICLE_TYPE"] != DBNull.Value)
                vehicle.VehicleType = Convert.ToString(row["VEHICLE_TYPE"]);

            if (row["VEHICLE_CATEGORY"] != DBNull.Value)
                vehicle.VehicleCategory = Convert.ToString(row["VEHICLE_CATEGORY"]);

            if (row["MODEL_NO"] != DBNull.Value)
                vehicle.Model = Convert.ToString(row["MODEL_NO"]);

            if (row["MANUFACTURING_YEAR"] != DBNull.Value)
                vehicle.ManufacturingYear = Convert.ToInt32(row["MANUFACTURING_YEAR"]);

            if (row["CYCLINDER_CAPACITY"] != DBNull.Value)
                vehicle.CyclinderCapacity = Convert.ToString(row["CYCLINDER_CAPACITY"]);

            if (row["FRAME_NUMBER"] != DBNull.Value)
                vehicle.FrameNumber = Convert.ToString(row["FRAME_NUMBER"]);

            if (row["ENGINE_NUMBER"] != DBNull.Value)
                vehicle.EngineNumber = Convert.ToString(row["ENGINE_NUMBER"]);

            if (row["VEHICLE_COLOR"] != DBNull.Value)
                vehicle.VehicleColor = Convert.ToString(row["VEHICLE_COLOR"]);

            if (row["FUEL_TYPE"] != DBNull.Value)
            {
                vehicle.FuelType = Convert.ToInt32(row["FUEL_TYPE"]);
                if (Convert.ToInt32(row["FUEL_TYPE"]) > 0)
                    vehicle.FuelTypeName = Constants.FuelTypeName[Convert.ToInt32(row["FUEL_TYPE"]) - 1];
            }

            if (row["LICENCE_PLATE_COLOR"] != DBNull.Value)
            {
                vehicle.LicencePlateColor = Convert.ToInt32(row["LICENCE_PLATE_COLOR"]);
                if (Convert.ToInt32(row["LICENCE_PLATE_COLOR"]) > 0)
                    vehicle.LicencePlateColorName = Constants.LicencePlateColorName[Convert.ToInt32(row["LICENCE_PLATE_COLOR"]) - 1];
            }

            if (row["REGISTRATION_YEAR"] != DBNull.Value)
                vehicle.RegistrationYear = Convert.ToInt32(row["REGISTRATION_YEAR"]);

            if (row["VEHICLE_OWNERSHIP_NO"] != DBNull.Value)
                vehicle.VehicleOwnershipDocumentNumber = Convert.ToString(row["VEHICLE_OWNERSHIP_NO"]);

            if (row["LOCATION_CODE"] != DBNull.Value)
                vehicle.LocationCode = Convert.ToString(row["LOCATION_CODE"]);

            if (row["REG_QUEUE_NO"] != DBNull.Value)
                vehicle.RegistrationQueueNumber = Convert.ToString(row["REG_QUEUE_NO"]);

            if (row["VEHICLEIMAGE_FRONT"] != DBNull.Value)
                vehicle.VehicleImageFront = Convert.ToString(row["VEHICLEIMAGE_FRONT"]);

            if (row["VEHICLEIMAGE_REAR"] != DBNull.Value)
                vehicle.VehicleImageRear = Convert.ToString(row["VEHICLEIMAGE_REAR"]);

            if (row["VEHICLEIMAGE_RIGHT"] != DBNull.Value)
                vehicle.VehicleImageRight = Convert.ToString(row["VEHICLEIMAGE_RIGHT"]);

            if (row["VEHICLEIMAGE_LEFT"] != DBNull.Value)
                vehicle.VehicleImageLeft = Convert.ToString(row["VEHICLEIMAGE_LEFT"]);

            if (row["VEHICLE_RC_NO_PATH"] != DBNull.Value)
                vehicle.VehicleRCNumberImagePath = Convert.ToString(row["VEHICLE_RC_NO_PATH"]);

            if (row["EXCEPTION_FLAG"] != DBNull.Value)
            {
                vehicle.ExceptionFlag = Convert.ToInt16(row["EXCEPTION_FLAG"]);
                if (Convert.ToInt32(row["EXCEPTION_FLAG"]) > 0)
                    vehicle.ExceptionFlagName = Constants.ExceptionFlagName[Convert.ToInt32(row["EXCEPTION_FLAG"]) - 1];
            }

            if (row["STATUS"] != DBNull.Value)
                vehicle.Status = Convert.ToInt16(row["STATUS"]);

            if (row["VALID_UNTIL"] != DBNull.Value)
                vehicle.ValidUntil = Convert.ToDateTime(row["VALID_UNTIL"]);

            if (row["TID_FRONT"] != DBNull.Value)
                vehicle.TidFront = Convert.ToString(row["TID_FRONT"]);

            if (row["TID_REAR"] != DBNull.Value)
                vehicle.TidRear = Convert.ToString(row["TID_REAR"]);

            if (row["ACCOUNT_BALANCE"] != DBNull.Value)
                vehicle.AccountBalance = Convert.ToDecimal(row["ACCOUNT_BALANCE"]);

            if (row["REGISTRATION_THROUGH"] != DBNull.Value)
                vehicle.RegistartionThrough = Convert.ToInt16(row["REGISTRATION_THROUGH"]);

            if (row["IS_DOC_VERIFIED"] != DBNull.Value)
                vehicle.IsDocVerified = Convert.ToInt16(row["IS_DOC_VERIFIED"]);

            if (row["QUEUE_STATUS"] != DBNull.Value)
            {
                vehicle.QueueStatus = Convert.ToInt16(row["QUEUE_STATUS"]);
                if (Convert.ToInt32(row["QUEUE_STATUS"]) > 0)
                    vehicle.CustomerQueueStatusName = Constants.CustomerQueueStatusName[Convert.ToInt32(row["QUEUE_STATUS"]) - 1];
            }
            return vehicle;
        }
        #endregion
    }
}
