using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CustomerQueueDAL
    {
        static string tableName = "TBL_CUSTOMER_QUEUE";
        public CustomerQueueDAL()
        {
        }

        #region Insert/Update/Delete
        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE customerQueue)
        {
           
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_QUEUE_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerQueue.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FIRST_NAME", DbType.String, customerQueue.FirstName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_NAME", DbType.String, customerQueue.LastName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MOB_NUMBER", DbType.String, customerQueue.MobNumber, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EMAIL_ID", DbType.String, customerQueue.EmailId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DESCRIPTION", DbType.String, customerQueue.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ADDRESS", DbType.String, customerQueue.Address, ParameterDirection.Input, 200));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STATUS", DbType.Int32, customerQueue.Status, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_IMAGE_PATH", DbType.String, customerQueue.CustomerImagePath, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_1", DbType.String, customerQueue.ScannedDocsPath1, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_2", DbType.String, customerQueue.ScannedDocsPath2, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_3", DbType.String, customerQueue.ScannedDocsPath3, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_4", DbType.String, customerQueue.ScannedDocsPath4, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_IMAGE_PATH_FRONT", DbType.String, customerQueue.VehicleImagePathFront, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_IMAGE_PATH_REAR", DbType.String, customerQueue.VehicleImagePathRear, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_DOC_VERIFIED", DbType.Int32, customerQueue.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, customerQueue.CreationDate, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE customerQueue)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_QUEUE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerQueue.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_QUEUE_ID", DbType.Int32, customerQueue.CustomerQueueId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FIRST_NAME", DbType.String, customerQueue.FirstName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_NAME", DbType.String, customerQueue.LastName, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MOB_NUMBER", DbType.String, customerQueue.MobNumber, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EMAIL_ID", DbType.String, customerQueue.EmailId, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DESCRIPTION", DbType.String, customerQueue.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ADDRESS", DbType.String, customerQueue.Address, ParameterDirection.Input, 200));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STATUS", DbType.Int32, customerQueue.Status, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_LOCATION", DbType.String, customerQueue.AppointmentLocation, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_DATE", DbType.DateTime, customerQueue.AppointmentDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_IMAGE_PATH", DbType.String, customerQueue.CustomerImagePath, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_1", DbType.String, customerQueue.ScannedDocsPath1, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_2", DbType.String, customerQueue.ScannedDocsPath2, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_3", DbType.String, customerQueue.ScannedDocsPath3, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_SCANNED_DOCS_PATH_4", DbType.String, customerQueue.ScannedDocsPath4, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_IMAGE_PATH_FRONT", DbType.String, customerQueue.VehicleImagePathFront, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_IMAGE_PATH_REAR", DbType.String, customerQueue.VehicleImagePathRear, ParameterDirection.Input, 150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_DOC_VERIFIED", DbType.Int32, customerQueue.IsDocVerified, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, customerQueue.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, customerQueue.ModificationDate, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection GetAllCustomerQueue()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection customerQueue = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_QUEUE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                customerQueue = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerQueue;
        }

        public static CBE.CustomerQueueCBE GetCustomerQueueById(CBE.CustomerQueueCBE customerQueue)
        {
            CBE.CustomerQueueCollection customerQueues = new CBE.CustomerQueueCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_QUEUE_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerQueue.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_QUEUE_ID", DbType.Int32, customerQueue.CustomerQueueId, ParameterDirection.Input));
                customerQueues = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return customerQueues[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection customerQueues = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE customerQueue = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        customerQueue.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["CUSTOMER_QUEUE_ID"] != DBNull.Value)
                        customerQueue.CustomerQueueId = Convert.ToInt32(dt.Rows[i]["CUSTOMER_QUEUE_ID"]);

                    if (dt.Rows[i]["FIRST_NAME"] != DBNull.Value)
                        customerQueue.FirstName = Convert.ToString(dt.Rows[i]["FIRST_NAME"]);

                    if (dt.Rows[i]["LAST_NAME"] != DBNull.Value)
                        customerQueue.LastName = Convert.ToString(dt.Rows[i]["LAST_NAME"]);

                    if (dt.Rows[i]["MOB_NUMBER"] != DBNull.Value)
                        customerQueue.MobNumber = Convert.ToString(dt.Rows[i]["MOB_NUMBER"]);

                    if (dt.Rows[i]["EMAIL_ID"] != DBNull.Value)
                        customerQueue.EmailId = Convert.ToString(dt.Rows[i]["EMAIL_ID"]);

                    if (dt.Rows[i]["DESCRIPTION"] != DBNull.Value)
                        customerQueue.Description = Convert.ToString(dt.Rows[i]["DESCRIPTION"]);

                    if (dt.Rows[i]["ADDRESS"] != DBNull.Value)
                        customerQueue.Address = Convert.ToString(dt.Rows[i]["ADDRESS"]);

                    if (dt.Rows[i]["STATUS"] != DBNull.Value)
                        customerQueue.Status = Convert.ToInt32(dt.Rows[i]["STATUS"]);

                    if (dt.Rows[i]["APPOINTMENT_LOCATION"] != DBNull.Value)
                        customerQueue.AppointmentLocation = Convert.ToString(dt.Rows[i]["APPOINTMENT_LOCATION"]);

                    if (dt.Rows[i]["APPOINTMENT_DATE"] != DBNull.Value)
                        customerQueue.AppointmentDate = Convert.ToDateTime(dt.Rows[i]["APPOINTMENT_DATE"]);

                    if (dt.Rows[i]["CUSTOMER_IMAGE_PATH"] != DBNull.Value)
                        customerQueue.CustomerImagePath = Convert.ToString(dt.Rows[i]["CUSTOMER_IMAGE_PATH"]);

                    if (dt.Rows[i]["SCANNED_DOCS_PATH_1"] != DBNull.Value)
                        customerQueue.ScannedDocsPath1 = Convert.ToString(dt.Rows[i]["SCANNED_DOCS_PATH_1"]);

                    if (dt.Rows[i]["SCANNED_DOCS_PATH_2"] != DBNull.Value)
                        customerQueue.ScannedDocsPath2 = Convert.ToString(dt.Rows[i]["SCANNED_DOCS_PATH_2"]);

                    if (dt.Rows[i]["SCANNED_DOCS_PATH_3"] != DBNull.Value)
                        customerQueue.ScannedDocsPath3 = Convert.ToString(dt.Rows[i]["SCANNED_DOCS_PATH_3"]);

                    if (dt.Rows[i]["SCANNED_DOCS_PATH_4"] != DBNull.Value)
                        customerQueue.ScannedDocsPath4 = Convert.ToString(dt.Rows[i]["SCANNED_DOCS_PATH_4"]);

                    if (dt.Rows[i]["VEHICLE_IMAGE_PATH_FRONT"] != DBNull.Value)
                        customerQueue.VehicleImagePathFront = Convert.ToString(dt.Rows[i]["VEHICLE_IMAGE_PATH_FRONT"]);

                    if (dt.Rows[i]["VEHICLE_IMAGE_PATH_REAR"] != DBNull.Value)
                        customerQueue.VehicleImagePathRear = Convert.ToString(dt.Rows[i]["VEHICLE_IMAGE_PATH_REAR"]);

                    if (dt.Rows[i]["IS_DOC_VERIFIED"] != DBNull.Value)
                        customerQueue.IsDocVerified = Convert.ToInt32(dt.Rows[i]["IS_DOC_VERIFIED"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        customerQueue.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        customerQueue.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        customerQueue.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    customerQueues.Add(customerQueue);
                }
                return customerQueues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
