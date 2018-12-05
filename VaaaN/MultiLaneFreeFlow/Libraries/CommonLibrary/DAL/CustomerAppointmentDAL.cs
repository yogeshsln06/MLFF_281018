using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class CustomerAppointmentDAL
    {
        static string tableName = "TBL_CUSTOMER_QUEUE";
        public CustomerAppointmentDAL()
        {
        }

        #region Insert/Update/Delete
        public static Int32 Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            int entryId = customerAppointment.CustomerAppointmentId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_APPOINTMENT_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerAppointment.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_APPOINTMENT_ID", DbType.Int32, entryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, customerAppointment.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_LOCATION", DbType.String, customerAppointment.AppointmentLocation, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_DATE", DbType.DateTime, Convert.ToDateTime(customerAppointment.AppointmentDate), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTED_BY", DbType.Int32, customerAppointment.AppointedById, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ATTENDED_BY", DbType.Int32, customerAppointment.AttendedbyId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, DateTime.Now, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, 1, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                entryId = Convert.ToInt32(command.Parameters["P_CUSTOMER_APPOINTMENT_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entryId;
        }


        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_APPOINTMENT_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerAppointment.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_APPOINTMENT_ID", DbType.Int32, customerAppointment.CustomerAppointmentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, customerAppointment.AccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_LOCATION", DbType.String, customerAppointment.AppointmentLocation, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTMENT_DATE", DbType.DateTime, Convert.ToDateTime(customerAppointment.AppointmentDate), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_APPOINTED_BY", DbType.Int32, customerAppointment.AppointedById, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ATTENDED_BY", DbType.Int32, customerAppointment.AttendedbyId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, customerAppointment.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, DateTime.Now, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, 1, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection GetAllCustomerAppointment()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection customerQueue = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "CUSTOMER_APPOINTMENT_GETALL";
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

        public static CBE.CustomerAppointmentCBE GetCustomerAppointmentById(CBE.CustomerAppointmentCBE customerQueue)
        {
            CBE.CustomerAppointmentCollection customerQueues = new CBE.CustomerAppointmentCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "CUSTOMER_APPOINTMENT_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, customerQueue.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CUSTOMER_APPOINTMENT_ID", DbType.Int32, customerQueue.CustomerAppointmentId, ParameterDirection.Input));
                customerQueues = ConvertDataTableToCollection(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName]);
                return customerQueues[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static CBE.CustomerAppointmentCollection GetCustomerAppointmentByAccountId(CBE.CustomerAppointmentCBE CustomerAppointment)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection CustomerAppointmentQueues = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection();
            try
            {
                string spName = Constants.oraclePackagePrefix + "CUSTAPPOINTMENT_GETACC_ID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, CustomerAppointment.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ACCOUNT_ID", DbType.Int32, CustomerAppointment.AccountId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
               return CustomerAppointmentQueues = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection customerAppointments = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        customerAppointment.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["CUSTOMER_APPOINTMENT_ID"] != DBNull.Value)
                        customerAppointment.CustomerAppointmentId = Convert.ToInt32(dt.Rows[i]["CUSTOMER_APPOINTMENT_ID"]);

                    if (dt.Rows[i]["ACCOUNT_ID"] != DBNull.Value)
                        customerAppointment.AccountId = Convert.ToInt32(dt.Rows[i]["ACCOUNT_ID"]);

                    if (dt.Rows[i]["APPOINTMENT_LOCATION"] != DBNull.Value)
                        customerAppointment.AppointmentLocation = Convert.ToString(dt.Rows[i]["APPOINTMENT_LOCATION"]);

                    if (dt.Rows[i]["APPOINTMENT_DATE"] != DBNull.Value)
                        customerAppointment.AppointmentDate = Convert.ToString(dt.Rows[i]["APPOINTMENT_DATE"]);

                    if (dt.Rows[i]["APPOINTED_BY"] != DBNull.Value)
                        customerAppointment.AppointedById = Convert.ToInt32(dt.Rows[i]["APPOINTED_BY"]);

                    if (dt.Rows[i]["ATTENDED_BY"] != DBNull.Value)
                        customerAppointment.AttendedbyId = Convert.ToInt32(dt.Rows[i]["ATTENDED_BY"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        customerAppointment.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        customerAppointment.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        customerAppointment.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);
                    customerAppointments.Add(customerAppointment);
                }
                return customerAppointments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
