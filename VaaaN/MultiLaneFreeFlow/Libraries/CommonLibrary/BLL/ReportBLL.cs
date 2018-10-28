using System.Data;
using VaaaN.MLFF.Libraries.CommonLibrary.DAL;
using System;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class ReportBLL
    {
        public ReportBLL()
        {
        }

        #region Device Reports.
        public static DataTable Device_MasterDetailReport(string device_id)
        {
            return ReportDAL.Device_MasterDetailReport(device_id);
        }
        public static DataTable Device_EventDetailReport(string device_id, string event_id, string start_date , string end_date)
        {
            return ReportDAL.Device_EventDetailReport(device_id, event_id, start_date , end_date);
        }
        #endregion

        #region MET Reports.
        public static DataTable MET_DailyAndHourlyData(string met_id, string met_info, string start_date, string end_date)
        {
            return ReportDAL.MET_DailyAndHourlyData(met_id, met_info, start_date, end_date);
        }
        public static DataTable MET_WeatherHistory(string met_id, string start_date, string end_date)
        {
            return ReportDAL.MET_WeatherHistory(met_id, start_date, end_date);
        }
        #endregion

        #region ATCC Reports.
        public static DataTable ATCC_ATCCDetailReport()
        {
            return ReportDAL.ATCC_ATCCDetailReport();
        }
        public static DataTable ATCC_HourlyAndMonthlyTrafficCount(string atcc_id, string atcc_class, string start_date, string end_date)
        {
            return ReportDAL.ATCC_HourlyAndMonthlyTrafficCount(atcc_id, atcc_class, start_date, end_date);
        }
        public static DataTable ATCC_TrafficByClassAndDate(string atcc_id, string class_id, string start_date, string end_date)
        {
            return ReportDAL.ATCC_TrafficByClassAndDate(atcc_id, class_id, start_date, end_date);
        }
        public static DataTable ATCC_TrafficByClassAndLocation(string atcc_id, string class_id, string start_date, string end_date)
        {
            return ReportDAL.ATCC_TrafficByClassAndLocation(atcc_id, class_id, start_date, end_date);
        }
        public static DataTable ATCC_TransactionDetailReport(string atcc_id, string class_id, string start_date, string end_date)
        {
            return ReportDAL.ATCC_TransactionDetailReport(atcc_id, class_id, start_date, end_date);//
        }
        public static DataTable ATCC_TransactionalReport(string atcc_id, string atcc_class, string veh_direction, string veh_speed, string @veh_axle, DateTime start_date, DateTime end_date)
        {
            return ReportDAL.ATCC_TransactionalReport(atcc_id, atcc_class, veh_direction, veh_speed, @veh_axle, start_date, end_date);
        }
        public static DataTable ATCC_MonthlyTransactionalReport(string atcc_id, string atcc_class, string month)
        {
            return ReportDAL.ATCC_MonthlyTransactionalReport(atcc_id, atcc_class, month);
        }
         #endregion

        #region ECB Reports.
        public static DataTable ECB_Call_Detail(string ecb_id, string call_type, string line_number, string disposition_category, string operator_id, string start_date, string end_date)
        {
            return ReportDAL.ECB_Call_Detail(ecb_id, call_type, line_number, disposition_category, operator_id, start_date, end_date);
        }
        public static DataTable ECB_DetailReport(string ecb_id, string direction)
        {
            return ReportDAL.ECB_DetailReport(ecb_id, direction);
        }
        public static DataTable ECB_AuditDetailReport(string ecb_id, string call_type, string line_number, string disposition_category, string ecb_direction, string operator_id, string start_date, string end_date)
        {
            return ReportDAL.ECB_AuditDetailReport(ecb_id, call_type, line_number, disposition_category, ecb_direction, operator_id, start_date, end_date);
        }
        public static DataTable ECB_VendalDetailReport(string ecb_id, string operator_id, DateTime start_date, DateTime end_date, string ecb_direction)
        {
            return ReportDAL.ECB_VendalDetailReport(ecb_id, operator_id, start_date, end_date, ecb_direction);
        }
        #endregion

        #region VMS Reports.
        public static DataTable VMS_CurrentMessageDetailReport(string vms_id, string template_id, string modifier_id, string start_date, string end_date)
        {
            return ReportDAL.VMS_CurrentMessageDetailReport(vms_id, template_id, modifier_id,  start_date, end_date);
        }
        public static DataTable VMS_MessageHistoryDetailReport(string vms_id, string template_id, string start_date, string end_date)
        {
            return ReportDAL.VMS_MessageHistoryDetailReport(vms_id, template_id, start_date, end_date);
        }
        #endregion


        #region VIDS Reports.
        public static DataTable VIDS_SummaryReport(string device_id, string event_id, string start_date , string end_date)
        {
            return ReportDAL.VIDS_SummaryReport(device_id, event_id, start_date , end_date);
        }

        public static DataTable Vids_EventDetailReport(string device_id, string event_id, string start_date, string end_date)
        {
            return ReportDAL.Vids_EventDetailReport(device_id, event_id, start_date, end_date);
        }
        #endregion
    }
}
