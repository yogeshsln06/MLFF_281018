using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VMSService
{
    public partial class MainService : ServiceBase
    {
        #region Variables

        Thread threadSendVMSMessage;
        private volatile bool stopThread = false;
        VaaaN.MLFF.Libraries.CommonLibrary.Classes.VMS.VMSController vmsController;
        #endregion

        #region Main
        public MainService()
        {
            InitializeComponent();
            //OnStart(new string[] { "saad" });
        }

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MainService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        #endregion

        #region Events
        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage("Trying to start VMS service...");

                LogMessage("Creating VMS controller...");
                vmsController = new VaaaN.MLFF.Libraries.CommonLibrary.Classes.VMS.VMSController();
                LogMessage("VMS controller created successfully.");

                LogMessage("Trying to start threadReadSMS...");
                threadSendVMSMessage = new Thread(SendVMSMessageThreadFunction);
                threadSendVMSMessage.IsBackground = true;
                threadSendVMSMessage.Name = "threadReadSMS";
                threadSendVMSMessage.Start();
                LogMessage("threadReadSMS started successfully.");

                LogMessage("SMS service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start VMS service." + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                #region Stop SMS Sender thread
                try
                {
                    LogMessage("Trying to stop threadSendVMSMessage...");
                    stopThread = true;
                    Thread.Sleep(200);

                    if (threadSendVMSMessage != null && threadSendVMSMessage.IsAlive)
                    {
                        threadSendVMSMessage.Abort();
                    }

                    threadSendVMSMessage = null;
                    LogMessage("The threadSendVMSMessage thread has been stopped.");
                }
                catch (Exception ex)
                {
                    LogMessage("Error in stopping threadSendVMSMessage thread." + ex.ToString());
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogMessage("Failed to process OnStop..." + ex.Message);
            }
            finally
            {
                base.OnStop();
            }
        }
        #endregion

        #region Helper Methods

        string previousStartDate = "";

        private void SendVMSMessageThreadFunction()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
          
            while (!stopThread)
            {
                try
                {
                    #region Get current applicable toll rates
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection currentTimeTollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();
                    DateTime currentDate = DateTime.Now;
                    currentTimeTollRates = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetTollRateCollection(currentDate, tollRates);

                    if (previousStartDate != currentTimeTollRates[0].StartTime)
                    {
                        if (currentTimeTollRates.Count > 0)
                        {
                            try
                            {
                                previousStartDate = currentTimeTollRates[0].StartTime;
                                LogMessage("There is change in toll rate time slot so sending toll rate message to vms for current time slot.");
                                vmsController.SendMessage(currentTimeTollRates);
                                LogMessage("Data send successfully");
                            }
                            catch (Exception ex)
                            {
                                LogMessage("Failed to send vms message." + ex.Message);
                            }
                        }
                        else
                        {
                            LogMessage("No toll rate found.");
                        }
                    }


                    DateTime currentStartDate = new DateTime();
                    DateTime currentEndDate = new DateTime();

                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in tollRates)
                    {


                        // Get Start hour and minute
                        int startHour = Convert.ToInt32(tr.StartTime.Substring(0, 2));
                        int startMinute = Convert.ToInt32(tr.StartTime.Substring(3, 2));

                        int endHour = Convert.ToInt32(tr.EndTime.Substring(0, 2));
                        int endMinute = Convert.ToInt32(tr.EndTime.Substring(3, 2));

                        currentStartDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startHour, startMinute, 0);
                        currentEndDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endHour, endMinute, 0);

                        if (startHour > endHour)// Cross day
                        {
                            currentEndDate.AddDays(1);
                        }

                        if (currentDate > currentStartDate && currentDate < currentEndDate)
                        {
                            currentTimeTollRates.Add(tr);

                        }
                    }
                    #endregion




                }
                catch (Exception ex)
                {
                    LogMessage("Failed to send VMS message." + ex.Message);
                }
                finally
                {
                    Thread.Sleep(200);
                }
            }
        }

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.VMS);
        }
        #endregion
    }
}
