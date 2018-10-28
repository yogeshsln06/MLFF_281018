using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.ReportLibrary
{
    public class Constants
    {
        public enum ReportCategory
        {
            ECB = 0
            //CCTV,
            //VMS,
            //MET,
            //ATCC
        };
        public static string[] ReportCategoryName = new string[] 
        {
            "ECB"
            //"CCTV",
            //"VMS",
            //"MET",
            //"ATCC"
        };
        public enum ATCC_VehSpeedCategory
        {
            speed_gret_20=20,
            speed_gret_30=30,
            speed_gret_40=40,
            speed_gret_50=50,
            speed_gret_60=60,
            speed_gret_70=70,
            speed_gret_80=80,
            speed_gret_100=100,
            speed_gret_120=120
        };
        public static string[] ATCC_VehSpeedCategoryName = new string[] 
        {
            ">20",
            ">30",
            ">40",
            ">50",
            ">60",
            ">70",
            ">80",
            ">100",
            ">120"
        };
        public enum ATCC_VehDirection
        {
            same_direction = 0,
            wrong_direction = 1
        };
        public static string[] ATCC_VehDirectionName = new string[] 
        {
             "Same Direction",
             "Wrong Direction"
        };
        public enum ATCC_VehAxles
        {
            axle_2 = 2,
            axle_3 = 3,
            axle_4 = 4,
            axle_5 = 5,
            axle_6 = 6,
            axle_gret_eql_7 = 7
        };
        public static string[] ATCC_VehAxlesName = new string[] 
        {
            "2 axles",
            "3 axles",
            "4 axles",
            "5 axles",
            "6 axles",
            ">=7 axles"
        };
        public enum Month
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        };
        public static string[] MonthName = new string[] 
        {
           "January",
           "February",
           "March",
           "April",
           "May",
           "June",
           "July",
           "August",
           "September",
           "October",
           "November",
           "December"
        };
        public enum DispositionCategory
        {
            Accident = 0,
            FuelFinish,
            CraneRequest,
            HoaxCall,
            AmbulanceRequest,
            Others
        }
        public static string[] DispositionCategoryName = new string[] 
        {
            "Accident",//0
            "Fuel Finish",
            "Crane Request",
            "Hoax Call",
            "Ambulance Request",
            "Others"
        };



        /// <summary>
        /// This array include report submodule ID from the database. This is done 
        /// to show report categorywise.
        /// </summary>
        public static int[] ATCC = new int[] { 25,26,27,28};
        public static int[] ECB = new int[] { 10,11,30,31 };
        public static int[] VMS = new int[] { };
        public static int[] MET = new int[] {13,15};//66
        public static int[] CCTV = new int[] {  };

        /// <summary>
        /// Select report from the list.
        /// </summary>
        public static CrystalDecisions.CrystalReports.Engine.ReportClass SelectReport(int subModuleId)
        {
            CrystalDecisions.CrystalReports.Engine.ReportClass currentReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();

            switch (subModuleId)
            {
                #region ECB report
                case 10://ECB_DetailReport
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ECB_DetailReport();
                        break;
                    }
                case 11://ECB_CallDetailReport report
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ECB_CallDetailReport();
                        break;
                    }
                case 30://ECB Audit Edtail report
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ECB_AuditDetail();
                        break;
                    }
                case 31://ECB_VendalDetail
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ECB_VendalDetail();
                        break;
                    }
                #endregion               
                #region MET report
                case 13://Consolidate revenue report
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.MET_Hourly_Data_Report();
                        break;
                    }
                case 15://Consolidate revenue report
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.MET_Daily_Data_Report();
                        break;
                    }
                #endregion
                #region ATCC report
                case 25:
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ATCC_DetailReport();
                        break;
                    }
                case 26:
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ATCC_TransactionDetailReport();
                        break;
                    }
                case 27:
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ATCC_MonthlyTrafficCount();
                        break;
                    }
                case 28:
                    {
                        currentReport = new VaaaN.MLFF.Libraries.ReportLibrary.CrystalReports.ATCC_HourlyTrafficCount();
                        break;
                    }
                #endregion
                #region vMS
                #endregion
                #region CCTV
                #endregion
                default:
                    break;
            }

            return currentReport;
        }
    }
}
