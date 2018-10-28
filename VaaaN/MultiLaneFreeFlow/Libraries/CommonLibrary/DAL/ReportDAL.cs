using System;
using System.Data;
using System.Data.Common;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class ReportDAL
    {
        static string tableName = "report_data";
        #region Device Report.
        public static DataTable Device_MasterDetailReport(string device_id)
        {
            DataTable DeviceMaster = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "DEVICE_MASTER_REPORT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DEVICE_ID", DbType.String, device_id, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DeviceMaster = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DeviceMaster;
        }

        public static DataTable Device_EventDetailReport(string device_id, string event_id , string start_date , string end_date)
        {
            DataTable EventDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "Event_Detail_Report";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DEVICE_ID", DbType.String, device_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EVENT_ID", DbType.String, event_id, ParameterDirection.Input));
                 command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                EventDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EventDetail;
        }
        #endregion

        #region Device Report.

        public static DataTable VIDS_SummaryReport(string device_id, string event_id, string start_date, string end_date)
        {
            DataTable vidsSummary = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "VIDS_SUMMARY_REPORT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DEVICE_ID", DbType.String, device_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EVENT_ID", DbType.String, event_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                vidsSummary = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vidsSummary;
        }

        public static DataTable Vids_EventDetailReport(string device_id, string event_id, string start_date, string end_date)
        {
            DataTable VidsEventDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "Vids_Event_Detail_Report";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DEVICE_ID", DbType.String, device_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_EVENT_ID", DbType.String, event_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                VidsEventDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VidsEventDetail;
        }




        #endregion








        #region MET Report.
        public static DataTable MET_DailyAndHourlyData(string met_id, string met_info, string start_date, string end_date)
        {
            DataTable hourlyDatadDt = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "MET_DAILY_AND_HOURLY_DATA";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_met_id", DbType.String, met_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_met_info", DbType.String, met_info, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                hourlyDatadDt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hourlyDatadDt;
        }        

        public static DataTable MET_WeatherHistory(string met_id, string start_date, string end_date)
        {
            DataTable hourlyDatadDt = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "MET_WEATHER_HISTORY";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_met_id", DbType.String, met_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                hourlyDatadDt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hourlyDatadDt;
        }
        #endregion

        #region ATCC Report.
        public static DataTable ATCC_ATCCDetailReport()
        {
            DataTable AtccDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "Report_ATCC_Detail";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccDetail;
        }

        public static DataTable ATCC_HourlyAndMonthlyTrafficCount(string atcc_id, string atcc_class, string start_date, string end_date)
        {
            DataTable AtccDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ATCC_HOURLY_MONTHLY_TRAFFIC";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ATCC_ID", DbType.String, atcc_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CLASS_ID", DbType.String, atcc_class, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccDetail;
        }

        public static DataTable ATCC_TrafficByClassAndDate(string atcc_id, string class_id, string start_date, string end_date)
        {
            DataTable AtccTransaction = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ATCC_TRAFFIC_BY_CLASS_AND_DATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_id", DbType.String, atcc_id, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("atcc id in DAL" + atcc_id, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.Report);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_class_id", DbType.String, class_id, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("class id in DAL" + class_id, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.Report);
                //command.Parameters.Add(VaaaN.ATMS.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date.ToString(VaaaN.ATMS.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("start date in DAL" + start_date, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.Report);
      
               
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("end date in DAL" + end_date, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.Report);
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccTransaction = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccTransaction;
        }

        public static DataTable ATCC_TrafficByClassAndLocation(string atcc_id, string class_id, string start_date, string end_date)
        {
            DataTable AtccTransaction = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "TRAFFIC_BY_CLASS_AND_LOCATION";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_id", DbType.String, atcc_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_class_id", DbType.String, class_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccTransaction = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccTransaction;
        }

        public static DataTable ATCC_TransactionDetailReport(string atcc_id, string class_id, string start_date, string end_date)
        {
            DataTable AtccTransaction = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ATCC_TRANSACTION_DETAIL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_id", DbType.String, atcc_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_class_id", DbType.String, class_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccTransaction = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccTransaction;
        }

        public static DataTable ATCC_TransactionalReport(string atcc_id, string atcc_class, string veh_direction, string veh_speed, string @veh_axle, DateTime start_date, DateTime end_date)
        {
            DataTable AtccDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "Report_ATCC_TransactionDetail";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_id", DbType.String, atcc_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_class", DbType.String, atcc_class, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_direction", DbType.String, veh_direction, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_speed", DbType.String, veh_speed, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_veh_axle", DbType.String, veh_axle, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery), ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccDetail;
        }

        public static DataTable ATCC_MonthlyTransactionalReport(string atcc_id, string atcc_class, string month)
        {
            DataTable AtccDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ATCC_HOURLY_MONTHLY_TRAFFIC";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_id", DbType.String, atcc_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_atcc_class", DbType.String, atcc_class, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_month", DbType.String, month, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AtccDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AtccDetail;
        }
        #endregion

        #region  ECB Report.
        public static DataTable ECB_Call_Detail(string ecb_id, string call_type, string line_number, string disposition_category, string operator_id, string start_date, string end_date)
        {
            DataTable CallDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ECB_CALL_DETAIL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ecb_id", DbType.String, ecb_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_call_type", DbType.String, call_type, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_line_number", DbType.String, line_number, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_disposition_category", DbType.String, disposition_category, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_operator_id", DbType.String, operator_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                CallDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CallDetail;
        }

        public static DataTable ECB_DetailReport(string ecb_id, string direction)
        {
            DataTable EcbDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ECB_DETAIL_REPORT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ecb_id", DbType.String, ecb_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ECB_DIRECTION", DbType.String, direction, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                EcbDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EcbDetail;
        }

        public static DataTable ECB_AuditDetailReport(string ecb_id, string call_type, string line_number, string disposition_category, string ecb_direction, string operator_id, string start_date, string end_date)
        {
            DataTable AuditDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "ECB_AUDIT_DETAIL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ECB_ID", DbType.String, ecb_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CALL_TYPE", DbType.String, call_type, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LINE_NUMBER", DbType.String, line_number, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_disposition_category", DbType.String, disposition_category, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ecb_direction", DbType.String, ecb_direction, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_operator_id", DbType.String, operator_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                AuditDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AuditDetail;

        }
        public static DataTable ECB_VendalDetailReport(string ecb_id, string operator_id, DateTime start_date, DateTime end_date, string ecb_direction)
        {
            DataTable VendalDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "Report_ECBVendalDetail";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ecb_id", DbType.String, ecb_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_operator_id", DbType.String, operator_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery), ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ecb_direction", DbType.String, ecb_direction, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                VendalDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VendalDetail;
        }
#endregion

        #region  VMS Report.
        public static DataTable VMS_CurrentMessageDetailReport(string vms_id, string template_id, string modifier_id, string start_date, string end_date)
        {
            DataTable CurrentMessageDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "VMS_CURRENT_MESSAGE_DETAIL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VMS_ID", DbType.String, vms_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TEMPLATE_ID", DbType.String, template_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.String, modifier_id, ParameterDirection.Input));
                //command.Parameters.Add(VaaaN.ATMS.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.DateTime, start_date, ParameterDirection.Input));
                //command.Parameters.Add(VaaaN.ATMS.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.DateTime, end_date, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                CurrentMessageDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CurrentMessageDetail;
        }
        public static DataTable VMS_MessageHistoryDetailReport(string vms_id, string template_id, string start_date, string end_date)
        {
            DataTable MessageHistoryDetail = new DataTable();
            try
            {
                string spName = Constants.oraclePackageReportPrefix + "VMS_MESSAGE_HISTORY_DETAIL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VMS_ID", DbType.String, vms_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TEMPLATE_ID", DbType.String, template_id, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_DATE", DbType.String, start_date, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_DATE", DbType.String, end_date, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                MessageHistoryDetail = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MessageHistoryDetail;
        }
        #endregion
      
           
    }
     
}

