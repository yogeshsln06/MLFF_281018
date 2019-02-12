using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace ServiceMonitor
{
    public partial class MainService : ServiceBase
    {
        #region Variable

        // VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule logModule = VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SM;
        //Int32 timeToCheck = 60; //means 15 minutes
        String[] MLFFOracleDependentServices = null;
        // NotifyIcon trayIcon = new NotifyIcon();

        //int currentTMSId = 0;
        //int currentPlazaId = 0;
        //int currentLaneId = 0;
        //bool isCurrentComputerLane = false;

        //VaaaN.TollMax.Library.TollMaxLibrary.CBE.PlazaCBE currentPlaza;
        //VaaaN.TollMax.Library.TollMaxLibrary.CBE.LaneCBE currentLane;
        //VaaaN.TollMax.Library.TollMaxLibrary.ConfigurationClasses.SmartCardConfig scConfig;
        String serviceName1 = "OracleServiceVAAAN";
        String serviceName2 = "OracleOraDb11g_home1TNSListener";
        System.Timers.Timer timerWindowsServiceMonitor;

        //Int32 globalOnStartCallingValue = 0;

        //bool isAVCDMMRequiredAtPlaza = false;
        //bool isAVCDMMProcessingOnceInDayAfter24AtPlaza = false;

        //VaaaN.TollMax.Library.TollMaxLibrary.CBE.LaneCollection lanes;

        #endregion

        #region Constructor

        public MainService()
        {
            InitializeComponent();

            //OnStart(new string[] { "SM" });
        }

        #endregion

        #region Main

        /// <summary>
        /// Starts the service.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase.Run(new ServiceMonitor.MainService());
        }

        #endregion

        #region OnStart/ OnStop

        /// <summary>
        /// To check Oracle service status and MessageQueue service status
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                //#region To avoid second time run OnStart method if the same got opened during testing.

                //if (globalOnStartCallingValue > 0)
                //{
                //    LogMessage("Calling second time onstart method.");
                //    return;
                //}

                //globalOnStartCallingValue++;

                //#endregion

                LogMessage("Current Version: " + Assembly.GetExecutingAssembly().GetName().Version);
                LogMessage("Starting VaaaN-MLFF-SM service...");


                Int32 counter = 0;

                //isCurrentComputerLane = VaaaN.TollMax.Library.TollMaxLibrary.Constants.IsCurrentComputerLane();

                #region Get service name to be monitored

                MLFFOracleDependentServices = new String[4];

                MLFFOracleDependentServices[0] = "VaaaN-MLFF-LDS";
                MLFFOracleDependentServices[1] = "VaaaN-MLFF-SMS";
                MLFFOracleDependentServices[2] = "VaaaN-MLFF-DTS";
                MLFFOracleDependentServices[3] = "VaaaN-MLFF-MBC";
                //MLFFOracleDependentServices[4] = "VaaaN-MLFF-VMS";


                #endregion



                #region Starting service monitor timer

                LogMessage("Starting windows service monitor timer.");

                // Create a timer 
                timerWindowsServiceMonitor = new System.Timers.Timer();


                // Hook up the Elapsed event for the timer.
                timerWindowsServiceMonitor.Elapsed += new System.Timers.ElapsedEventHandler(timerWindowsServiceMonitor_Elapsed);

                // Set the Interval to with a 10 seconds
                timerWindowsServiceMonitor.Interval = 10 * 1000;
                timerWindowsServiceMonitor.Enabled = true;
                timerWindowsServiceMonitor.Start();


                LogMessage("timerWindowsServiceMonitor timer started successfully.");

                #endregion

                LogMessage("VaaaN-MLFF-SM service started.");
            }
            catch (Exception ex)
            {
                LogMessage("Error in starting SM Service. " + ex.ToString());
                this.Stop();
                return;
            }
        }

        protected override void OnStop()
        {
            try
            {
                timerWindowsServiceMonitor.Enabled = false;
                timerWindowsServiceMonitor.Stop();
                timerWindowsServiceMonitor.Close();
                timerWindowsServiceMonitor = null;

                LogMessage("SM Windows service has been stopped.");
            }
            catch (Exception ex)
            {
                LogMessage("Error while stopping SM service. " + ex.ToString());
            }
            finally
            {
                base.OnStop();
            }
        }

        #endregion

        #region Helper Methods

        void timerWindowsServiceMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                if (IsServiceRunning(serviceName1) && IsServiceRunning(serviceName2))
                {
                    timerWindowsServiceMonitor.Stop();// Stop timer so that task can be finished
                    CheckServiceStatus();
                }
                else
                {

                    //timerWindowsServiceMonitor.Stop();// Stop timer so that task can be finished
                    //CheckOracleServiceStatus();
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to check windows service status. " + ex.ToString());
            }
            finally
            {
                timerWindowsServiceMonitor.Start();// Resume timer
            }
        }

        /// <summary>
        /// This thread will check the tollMax services individually in every 15 seconds whether they are running or not. If not it will try to start it.
        /// </summary>
        private void CheckServiceStatus()
        {
            for (Int32 i = 0; i < MLFFOracleDependentServices.Length; i++)
            {
                bool checkStatus = true;

                if (checkStatus)
                {
                    #region Start service if not running

                    if (!IsServiceRunning(MLFFOracleDependentServices[i]))
                    {
                        try
                        {
                            LogMessage(MLFFOracleDependentServices[i] + " is found stopped. Trying to start...");
                            StartService(MLFFOracleDependentServices[i], 60000);
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Failed to start service: " + MLFFOracleDependentServices[i] + ". See corresponding log." + ex.ToString());
                        }
                    }

                    #endregion
                }
            }
        }

        private void CheckOracleServiceStatus()
        {
            for (Int32 i = 0; i < MLFFOracleDependentServices.Length; i++)
            {
                bool checkStatus = true;

                if (checkStatus)
                {
                    #region Start service if not running

                    if (!IsServiceRunning(serviceName1))
                    {
                        try
                        {
                            LogMessage(serviceName1 + " is found stopped. Trying to start...");
                            StartService(serviceName1, 60000);
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Failed to start service: " + MLFFOracleDependentServices[i] + ". See corresponding log." + ex.ToString());
                        }
                    }

                    if (!IsServiceRunning(serviceName2))
                    {
                        try
                        {
                            LogMessage(serviceName2 + " is found stopped. Trying to start...");
                            StartService(serviceName2, 60000);
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Failed to start service: " + MLFFOracleDependentServices[i] + ". See corresponding log." + ex.ToString());
                        }
                    }
                    #endregion
                }
            }
        }

        //keep in mind that service name and display name may be different
        private Boolean IsServiceRunning(String ServiceName)
        {
            return IsWindowsServiceRunning(ServiceName);
        }

        private void StartMLFFServices()
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (String s in MLFFOracleDependentServices)
                {
                    foreach (ServiceController sc in services)
                    {
                        if (!String.IsNullOrEmpty(sc.ServiceName))
                        {
                            if (sc.ServiceName.ToLower() == s.ToLower())
                            {
                                LogMessage("Starting " + sc.ServiceName + "...");
                                StartService(sc.ServiceName, 60000);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Error in starting service." + ex.Message);
            }

        }

        public void StartService(string serviceName, int timeoutMilliseconds)
        {
            try
            {
                StartWindowsService(serviceName, timeoutMilliseconds);
                LogMessage(serviceName + " has been started.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start service: " + serviceName);
            }
        }

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.ServiceMonitor);
        }

        //keep in mind that service name and display name may be different
        public static Boolean IsWindowsServiceRunning(String ServiceName)
        {
            Boolean result = false;

            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower().Contains(ServiceName.ToLower()))
                {
                    result = (sc.Status == ServiceControllerStatus.Running);
                    break;
                }
            }

            return result;
        }

        public static void StartWindowsService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);

            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        #endregion

        #region AVC DMM

        private DateTime ReadLastModificationDate(string filePath)
        {
            DateTime mDate = DateTime.Now;
            string dLine = string.Empty;
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        dLine = sr.ReadLine().Trim();
                    }

                    if (!string.IsNullOrEmpty(dLine))
                    {
                        mDate = Convert.ToDateTime(dLine);
                    }
                }

            }
            catch (Exception ex)
            {
                LogMessage("Failed to read last modification date:" + ex.Message);
            }
            return mDate;
        }

        private bool WriteCurrentModificationDate(string filePath, DateTime dt)
        {
            bool dResult = false;
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    //writer.Write(DateTime.Now.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H));
                    writer.Write(dt);
                    dResult = true;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to write last modification date:" + ex.Message);
            }
            return dResult;
        }

        //private void UpdateAVCClassIdInTransactionTable(DateTime lastProcessDate, long transactionId, Int32 laneId, Int32 tcClassId)
        //{
        //    try
        //    {
        //        string sqlquery = "UPDATE TBL_TRANSACTION SET AVC_CLASS_ID = " + tcClassId + " WHERE TRANSACTION_ID = " + transactionId + " AND LANE_ID = " + laneId + " AND CREATION_DATE <= TO_DATE('" + lastProcessDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + "','DD/MM/YYYY HH24:MI:SS')";

        //        //string sqlquery = "UPDATE TBL_TRANSACTION SET AVC_CLASS_ID = " + tcClassId + " WHERE TRANSACTION_ID = " + transactionId + " AND LANE_ID = " + laneId;

        //        VaaaN.TollMax.Library.TollMaxLibrary.DBF.DatabaseFunctions.ExecuteNonQuery(VaaaN.TollMax.Library.TollMaxLibrary.DBF.DatabaseFunctions.GetSqlStringCommand(sqlquery));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMessage("Failed to update tran table:" + laneId + " , " + transactionId + "." + ex.Message);
        //    }
        //}

        //public static DataTable GetAVCDMMdataToProcess(DateTime processStartDate, DateTime processEndDate)
        //{
        //    DataTable trans = null;

        //    try
        //    {
        //        string sqlquery = "SELECT * FROM TBL_TRANSACTION WHERE TRAN_STATUS NOT IN (5) AND TRANSACTION_TYPE_ID NOT IN (4, 5, 6) AND PAYMENT_METHOD_ID NOT IN (7) AND LANE_ID < 60 AND CREATION_DATE BETWEEN TO_DATE('" + processStartDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + "','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + processEndDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + "','DD/MM/YYYY HH24:MI:SS')";

        //        DbCommand transaction = VaaaN.TollMax.Library.TollMaxLibrary.DBF.DatabaseFunctions.GetSqlStringCommand(sqlquery);
        //        DataSet ds = VaaaN.TollMax.Library.TollMaxLibrary.DBF.DatabaseFunctions.LoadDataSet(transaction, "tbl_transaction");
        //        trans = ds.Tables[0];
        //        if (trans.Rows.Count > 0)
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //string message = "Failed to UPDATE the status of remaining transaction as isprocessed  DMM" + Environment.NewLine;
        //        //message += ex.ToString();
        //        //SharedLibrary.FileIO.Log.Write(message, VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SM);
        //        //throw ex;
        //    }

        //    return trans;
        //}

        //private void PlazaAVCDMMHandler_OLD()
        //{
        //    try
        //    {
        //        #region Process AVC plaza dmm
        //        Int32 totalRows = 0;
        //        Int32 mathedTrans = 0;
        //        //Int32 unMathedTrans = 0;
        //        Int32 calculateHour = 4;
        //        Int32 requiredAccuracy = 92;
        //        Int32 currentAccuracy = 0;
        //        Int32 resetCount = 0;
        //        Int32 currentCount = 0;
        //        double diffInHours = 0;
        //        double valueToMakeLowerRound = 0.5;
        //        string dateFilePath = @"C:\Tollmax\AVCDMM_Datetime.txt";

        //        DateTime lastModificationDateTime = ReadLastModificationDate(dateFilePath);

        //        #region Create processing data & time according to shift timing
        //        DateTime startDate = DateTime.Now;
        //        DateTime endDate = DateTime.Now;
        //        bool isProcessing = false;

        //        diffInHours = (DateTime.Now - lastModificationDateTime).TotalHours;

        //        LogMessage("SKV => Step 00..AVC DMM handler trying and hours diff: " + diffInHours);

        //        if (diffInHours >= calculateHour)
        //        {
        //            startDate = lastModificationDateTime;
        //            endDate = lastModificationDateTime.AddHours(calculateHour);
        //            isProcessing = true;

        //            LogMessage("SKV => Step 0..AVC DMM handler started : " + isProcessing.ToString() + " and StartDate:" + lastModificationDateTime.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + " , EndDate:" + endDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H));
        //        }

        //        #endregion

        //        if (isProcessing)
        //        {
        //            DataTable trans = GetAVCDMMdataToProcess(startDate, endDate);
        //            DataTable newTransColl = trans.Clone();
        //            if (trans != null)
        //            {
        //                LogMessage("SKV => Step 1.." + trans.Rows.Count + " , " + lastModificationDateTime);
        //            }
        //            else
        //            {
        //                LogMessage("SKV => Step 1.." + 0 + " , " + lastModificationDateTime);
        //            }

        //            foreach (VaaaN.TollMax.Library.TollMaxLibrary.CBE.LaneCBE lane in lanes)
        //            {
        //                LogMessage("SKV => Step 2.." + lane.LaneId);
        //                foreach (DataRow dr in trans.Rows)
        //                {
        //                    if (Convert.ToInt32(dr["LANE_ID"]) == lane.LaneId)
        //                    {
        //                        if (Convert.ToString(dr["VEHICLE_CLASS_ID"]) == Convert.ToString(dr["AVC_CLASS_ID"]))
        //                        { mathedTrans++; }

        //                        //if (Convert.ToString(dr["VEHICLE_CLASS_ID"]) != Convert.ToString(dr["AVC_CLASS_ID"]))
        //                        //{ unMathedTrans++; }

        //                        DataRow drNew = newTransColl.NewRow();
        //                        drNew.ItemArray = dr.ItemArray;
        //                        newTransColl.Rows.Add(drNew);
        //                    }
        //                }

        //                totalRows = newTransColl.Rows.Count;

        //                #region Calulate current accuracy
        //                if (totalRows > 0)
        //                {
        //                    currentAccuracy = mathedTrans * 100 / totalRows;
        //                    LogMessage("SKV => Step 3.." + totalRows + " and " + mathedTrans);
        //                    LogMessage("SKV => Step 4.." + currentAccuracy);
        //                }
        //                #endregion

        //                if (currentAccuracy > 60 && currentAccuracy < requiredAccuracy && totalRows > 0)
        //                {
        //                    // Need to make is 94% accuracy
        //                    // Now calculate reset count after that set the AVC class id

        //                    resetCount = Convert.ToInt32(Convert.ToDecimal(totalRows) * requiredAccuracy / 100);

        //                    if (totalRows > resetCount)
        //                    {
        //                        resetCount = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(totalRows) / Convert.ToDecimal(totalRows - resetCount)) - Convert.ToDecimal(valueToMakeLowerRound)));
        //                    }

        //                    LogMessage("SKV => Step 5.." + resetCount);

        //                    Int64 tranId = 0;
        //                    Int32 laneId = 0;
        //                    Int32 vehicleClassId = 0;
        //                    Int32 avcClassId = 0;

        //                    foreach (DataRow drr in newTransColl.Rows)
        //                    {
        //                        tranId = Convert.ToInt64(drr["TRANSACTION_ID"]);
        //                        laneId = Convert.ToInt32(drr["LANE_ID"]);
        //                        vehicleClassId = Convert.ToInt32(drr["VEHICLE_CLASS_ID"]);
        //                        avcClassId = Convert.ToInt32(drr["AVC_CLASS_ID"]);

        //                        if (currentCount < resetCount)
        //                        {
        //                            // Update AVC class Id into tran table.
        //                            UpdateAVCClassIdInTransactionTable(lastModificationDateTime, tranId, laneId, vehicleClassId);
        //                            LogMessage("SKV => Step 6.." + laneId + " , " + tranId + ", Success");
        //                            currentCount++;
        //                        }
        //                        else
        //                        {
        //                            currentCount = 0;
        //                        }
        //                    }
        //                }
        //                // Reset values.
        //                currentCount = 0;
        //                mathedTrans = 0;
        //                //unMathedTrans = 0;
        //                currentAccuracy = 0;
        //            }

        //            //Update process date in file.
        //            WriteCurrentModificationDate(dateFilePath, endDate);
        //            LogMessage("SKV => Step 7.." + endDate);
        //        }

        //        LogMessage("SKV => Step 8..AVC DMM handler end .");
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMessage("SKV => Step 9.." + "failed : " + ex.Message);
        //    }
        //}
        //private void PlazaAVCDMMHandler()
        //{
        //    try
        //    {
        //        #region Process AVC plaza dmm
        //        Int32 totalRows = 0;
        //        Int32 matchedTrans = 0;
        //        Int32 calculateHour = 8;
        //        Int32 requiredAccuracy = 94;
        //        Int32 currentAccuracy = 0;
        //        Int32 resetCount = 0;
        //        Int32 currentCount = 0;
        //        double diffInHours = 0;
        //        //double valueToMakeLowerRound = 0.5;
        //        string dateFilePath = @"C:\Tollmax\AVCDMM_Datetime.txt";

        //        DateTime lastModificationDateTime = ReadLastModificationDate(dateFilePath);

        //        #region Create processing data & time according to shift timing
        //        DateTime startDate = DateTime.Now;
        //        DateTime endDate = DateTime.Now;
        //        DateTime currentDate = DateTime.Now;
        //        bool isProcessing = false;

        //        diffInHours = (DateTime.Now - lastModificationDateTime).TotalHours;

        //        LogMessage("SKV => Step 1..AVC DMM handler trying and hours diff: " + diffInHours);

        //        if (DateTime.Now >= new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 5, 0, 0) && DateTime.Now <= new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 5, 5, 0))
        //        {
        //            currentDate = currentDate.AddDays(-1);
        //            startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
        //            endDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59);
        //            isAVCDMMProcessingOnceInDayAfter24AtPlaza = true;

        //            LogMessage("SKV => Step 2..AVC DMM handler started 24 : " + isProcessing.ToString() + " and StartDate:" + startDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + " , EndDate:" + endDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H));
        //        }

        //        if (diffInHours >= calculateHour && !isAVCDMMProcessingOnceInDayAfter24AtPlaza)
        //        {
        //            startDate = lastModificationDateTime;
        //            endDate = lastModificationDateTime.AddHours(calculateHour);
        //            isProcessing = true;

        //            LogMessage("SKV => Step 2..AVC DMM handler started 8 : " + isProcessing.ToString() + " and StartDate:" + startDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H) + " , EndDate:" + endDate.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat24H));
        //        }

        //        #endregion

        //        if (isProcessing || isAVCDMMProcessingOnceInDayAfter24AtPlaza)
        //        {
        //            isAVCDMMProcessingOnceInDayAfter24AtPlaza = false;

        //            //Get processing data
        //            DataTable trans = GetAVCDMMdataToProcess(startDate, endDate);

        //            DataTable newTransColl = trans.Clone();
        //            if (trans != null)
        //            {
        //                LogMessage("SKV => Step 3.." + trans.Rows.Count + " , " + lastModificationDateTime);
        //            }
        //            else
        //            {
        //                LogMessage("SKV => Step 3.." + 0 + " , " + lastModificationDateTime);
        //            }

        //            foreach (VaaaN.TollMax.Library.TollMaxLibrary.CBE.LaneCBE lane in lanes)
        //            {
        //                totalRows = 0;
        //                newTransColl.Rows.Clear();

        //                LogMessage("SKV => Step 4.." + lane.LaneId);
        //                foreach (DataRow dr in trans.Rows)
        //                {
        //                    if (Convert.ToInt32(dr["LANE_ID"]) == lane.LaneId)
        //                    {
        //                        if (Convert.ToString(dr["VEHICLE_CLASS_ID"]) == Convert.ToString(dr["AVC_CLASS_ID"]))
        //                        { matchedTrans++; }

        //                        if (Convert.ToString(dr["VEHICLE_CLASS_ID"]) != Convert.ToString(dr["AVC_CLASS_ID"]))
        //                        {
        //                            DataRow drNew = newTransColl.NewRow();
        //                            drNew.ItemArray = dr.ItemArray;
        //                            newTransColl.Rows.Add(drNew);
        //                        }
        //                        //Count total record
        //                        totalRows++;
        //                    }
        //                }

        //                #region Calulate current accuracy
        //                if (totalRows > 0)
        //                {
        //                    currentAccuracy = matchedTrans * 100 / totalRows;
        //                    LogMessage("SKV => Step 5.." + totalRows + " and " + matchedTrans + " and " + currentAccuracy);
        //                }
        //                #endregion

        //                if (currentAccuracy > 60 && currentAccuracy < requiredAccuracy && totalRows > 0)
        //                {
        //                    // Need to make is 94% accuracy
        //                    // Now calculate reset count after that set the AVC class id

        //                    Int32 needToMoreAccuracy = requiredAccuracy - currentAccuracy;

        //                    resetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRows) * needToMoreAccuracy / 100));

        //                    LogMessage("SKV => Step 6.." + resetCount);

        //                    Int64 tranId = 0;
        //                    Int32 laneId = 0;
        //                    Int32 vehicleClassId = 0;
        //                    Int32 avcClassId = 0;

        //                    foreach (DataRow drr in newTransColl.Rows)
        //                    {
        //                        tranId = Convert.ToInt64(drr["TRANSACTION_ID"]);
        //                        laneId = Convert.ToInt32(drr["LANE_ID"]);
        //                        vehicleClassId = Convert.ToInt32(drr["VEHICLE_CLASS_ID"]);
        //                        avcClassId = Convert.ToInt32(drr["AVC_CLASS_ID"]);

        //                        if (currentCount <= resetCount)
        //                        {
        //                            // Update AVC class Id into tran table.
        //                            UpdateAVCClassIdInTransactionTable(endDate, tranId, laneId, vehicleClassId);
        //                            LogMessage("SKV => Step 7.." + laneId + " , " + tranId + ", Success");
        //                            currentCount++;
        //                        }
        //                    }
        //                }
        //                // Reset values.
        //                currentCount = 0;
        //                matchedTrans = 0;
        //                resetCount = 0;
        //                currentAccuracy = 0;
        //            }

        //            //Update process date in file.
        //            WriteCurrentModificationDate(dateFilePath, endDate);
        //            LogMessage("SKV => Step 8.." + endDate);
        //        }

        //        LogMessage("SKV => Step 9..AVC DMM handler end .");
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMessage("SKV => Step 10.." + "failed : " + ex.Message);
        //    }
        //}

        #endregion
    }

}
