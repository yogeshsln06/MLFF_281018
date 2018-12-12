using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class SMSCommunicationHistoryDAL
    {
        static string tableName = "TBL_SMS_COMM_HISTORY";
        public SMSCommunicationHistoryDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            int entryId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SMS_HISTORY_INSERT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, sms.EntryId, ParameterDirection.Output));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, sms.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_account_id", DbType.Int32, sms.CustomerAccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_vechile_id", DbType.Int32, sms.CustomerVehicleId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_name", DbType.String, sms.CustomerName, ParameterDirection.Input,255));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mobile_number", DbType.String, sms.MobileNumber, ParameterDirection.Input,20));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_direction", DbType.Int32, sms.MessageDirection, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_body", DbType.String, sms.MessageBody, ParameterDirection.Input,150));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sent_status", DbType.Int32, sms.SentStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_received_process_status", DbType.Int32, sms.ReceivedProcessStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_send_time", DbType.DateTime, sms.MessageSendDateTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_receive_time", DbType.DateTime, sms.MessageReceiveTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_delivery_status", DbType.Int32, sms.MessageDeliveryStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_attempt_count", DbType.Int32, sms.AttemptCount, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, sms.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, sms.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modified_by", DbType.Int32, sms.ModifiedBy, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                entryId = Convert.ToInt32(command.Parameters["p_entry_id"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entryId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SMS_HISTORY_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_entry_id", DbType.Int32, sms.EntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, sms.TmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_account_id", DbType.Int32, sms.CustomerAccountId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_customer_name", DbType.String, sms.CustomerName, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_mobile_number", DbType.String, sms.MobileNumber, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_direction", DbType.Int32, sms.MessageDirection, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_body", DbType.String, sms.MessageBody, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_sent_status", DbType.Int32, sms.SentStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_received_process_status", DbType.Int32, sms.ReceivedProcessStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_send_time", DbType.DateTime, sms.MessageSendDateTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_receive_time", DbType.DateTime, sms.MessageReceiveTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_message_delivery_status", DbType.Int32, sms.MessageDeliveryStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_attempt_count", DbType.Int32, sms.AttemptCount, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_creation_date", DbType.DateTime, sms.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, sms.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modified_by", DbType.Int32, sms.ModifiedBy, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection GetFilteredRecords(string filter)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection smses = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "SMS_HISTORY_GETFILTERED";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_filter", DbType.String, filter, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                smses = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return smses;
        }

        /// <summary>
        /// Get next value for the primary key
        /// </summary>
        /// <returns></returns>

        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection smses = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();

                    if (dt.Rows[i]["ENTRY_ID"] != DBNull.Value)
                        sms.EntryId = Convert.ToInt32(dt.Rows[i]["ENTRY_ID"]);

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        sms.TmsId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["CUSTOMER_ACCOUNT_ID"] != DBNull.Value)
                        sms.CustomerAccountId = Convert.ToInt32(dt.Rows[i]["CUSTOMER_ACCOUNT_ID"]);

                    if (dt.Rows[i]["CUSTOMER_NAME"] != DBNull.Value)
                        sms.CustomerName = Convert.ToString(dt.Rows[i]["CUSTOMER_NAME"]);

                    if (dt.Rows[i]["MOBILE_NUMBER"] != DBNull.Value)
                        sms.MobileNumber = Convert.ToString(dt.Rows[i]["MOBILE_NUMBER"]);

                    if (dt.Rows[i]["MESSAGE_DIRECTION"] != DBNull.Value)
                        sms.MessageDirection = Convert.ToInt32(dt.Rows[i]["MESSAGE_DIRECTION"]);

                    if (dt.Rows[i]["MESSAGE_BODY"] != DBNull.Value)
                        sms.MessageBody = Convert.ToString(dt.Rows[i]["MESSAGE_BODY"]);

                    if (dt.Rows[i]["SENT_STATUS"] != DBNull.Value)
                        sms.SentStatus = Convert.ToInt32(dt.Rows[i]["SENT_STATUS"]);

                    if (dt.Rows[i]["RECEIVED_PROCESS_STATUS"] != DBNull.Value)
                        sms.ReceivedProcessStatus = Convert.ToInt32(dt.Rows[i]["RECEIVED_PROCESS_STATUS"]);

                    if (dt.Rows[i]["MESSAGE_SEND_TIME"] != DBNull.Value)
                        sms.MessageSendDateTime = Convert.ToDateTime(dt.Rows[i]["MESSAGE_SEND_TIME"]);

                    if (dt.Rows[i]["MESSAGE_RECEIVE_TIME"] != DBNull.Value)
                        sms.MessageReceiveTime = Convert.ToDateTime(dt.Rows[i]["MESSAGE_RECEIVE_TIME"]);

                    if (dt.Rows[i]["MESSAGE_DELIVERY_STATUS"] != DBNull.Value)
                        sms.MessageDeliveryStatus = Convert.ToInt32(dt.Rows[i]["MESSAGE_DELIVERY_STATUS"]);

                    if (dt.Rows[i]["ATTEMPT_COUNT"] != DBNull.Value)
                        sms.AttemptCount = Convert.ToInt32(dt.Rows[i]["ATTEMPT_COUNT"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        sms.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        sms.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    if (dt.Rows[i]["MODIFIED_BY"] != DBNull.Value)
                        sms.ModifiedBy = Convert.ToInt32(dt.Rows[i]["MODIFIED_BY"]);

                    smses.Add(sms);
                }
                return smses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
