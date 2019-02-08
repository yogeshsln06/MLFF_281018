using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.Classes.MobileBroadCast;


namespace MobileBroadCastService
{
    public partial class MainService : ServiceBase
    {
        #region Variable
        Thread threadAccountBalanceUpdate;
        Thread threadNotificationBoradCast;
        private volatile bool stopThread = false;
        #endregion

        #region Constructor
        public MainService()
        {
            InitializeComponent();

            //OnStart(new string[] { "" });
        }

        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[]
        //    {
        //        new MainService()
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}
        #endregion

        #region OnStart/ OnStop
        protected override void OnStart(string[] args)
        {
            try
            {
               
                LogMessage("Trying to start threadAccountBalanceUpdate...");
                threadAccountBalanceUpdate = new Thread(threadAccountBalanceUpdateFunction);
                threadAccountBalanceUpdate.IsBackground = true;
                threadAccountBalanceUpdate.Name = "threadSendAccountBalance";
                threadAccountBalanceUpdate.Start();
                LogMessage("threadAccountBalanceUpdate started successfully.");

                LogMessage("Trying to start threadNotificationBoradCast...");
                threadNotificationBoradCast = new Thread(threadNotificationBoradCastFunction);
                threadNotificationBoradCast.IsBackground = true;
                threadNotificationBoradCast.Name = "threadNotificationBoradCast";
                threadNotificationBoradCast.Start();
                LogMessage("threadNotificationBoradCast started successfully.");

                LogMessage("Mobile service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start Mobile service." + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogMessage("Stopping Mobile service.");
                #region Stop Mobile Sender thread
                try
                {
                    LogMessage("Trying to stop threadSendMobile...");
                    stopThread = true;
                    Thread.Sleep(200);
                    if (threadAccountBalanceUpdate != null && threadAccountBalanceUpdate.IsAlive)
                    {
                        threadAccountBalanceUpdate.Abort();
                    }
                    threadAccountBalanceUpdate = null;
                    LogMessage("The threadAccountBalanceUpdate  and threadAccountBalanceUpdate thread has been stopped.");

                }
                catch (Exception ex)
                {
                    LogMessage("Error in stopping threadAccountBalanceUpdate and threadAccountBalanceUpdate thread function " + ex.ToString());
                }

                #endregion


                LogMessage("Mobile service stopped successfully.");
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

       

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MobileWebAPI);
        }

      


        private void threadAccountBalanceUpdateFunction()
        {
            while (!stopThread)
            {
                try
                {
                    UpdateAccountBalance();
                }
                catch (Exception ex)
                {
                    LogMessage("Failed to send message." + ex.Message);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void UpdateAccountBalance()
        {
            try
            {
                DataTable unsentBalance = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerbalanceUpdateMobile();
                foreach (DataRow dr in unsentBalance.Rows)
                {
                    string responseString = BrodcastDataMobile.BroadCastBalance(dr);
                    SendBrodcastStatus(responseString, dr["ENTRY_ID"].ToString(), "balance");
                    Thread.Sleep(1000);

                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send Mobile." + ex.Message);
            }
            finally
            {
                Thread.Sleep(2000);
            }
        }

        private void threadNotificationBoradCastFunction()
        {
            while (!stopThread)
            {
                try
                {
                    BraodCastNotification();
                }
                catch (Exception ex)
                {
                    LogMessage("Failed to send message." + ex.Message);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void BraodCastNotification()
        {
            try
            {
                DataTable unsentNotification = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.GetAllPendindNotification();
                foreach (DataRow row in unsentNotification.Rows)
                {
                    string responseString = BrodcastDataMobile.BroadCastNotification(row["RESIDENT_ID"].ToString(), row["ENTRY_ID"].ToString(), row["VEHICLEID"].ToString(), row["TRANSACTION_SUBJECT"].ToString(), row["MESSAGE_BODY"].ToString());
                    SendBrodcastStatus(responseString, row["ENTRY_ID"].ToString(), "Noti");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send Mobile." + ex.Message);
            }
            finally
            {
                Thread.Sleep(2000);
            }
        }

        public void SendBrodcastStatus(string responseString, string Id, string APIfor)
        {
            try
            {
                int SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                MobileResponce objMobileResponce = JsonConvert.DeserializeObject<MobileResponce>(responseString);
                if (objMobileResponce.Apifor.ToLower() == "balance")
                {
                    if (objMobileResponce.status.ToLower() == "success")
                        SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                    AccountHistoryBLL.UpdateBalanceStatus(objMobileResponce.trans_id, SentStatus, objMobileResponce.message);
                }
                if (objMobileResponce.Apifor.ToLower() == "notification")
                {
                    if (objMobileResponce.status.ToLower() == "success")
                        SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                    SMSCommunicationHistoryBLL.UpdateNotificationStatus(objMobileResponce.trans_id, SentStatus);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send Mobile." + ex.Message + " Account Entry Id: " + Id + " Responce: " + responseString + " API For " + APIfor);
            }
        }

        public void SendSMSStatus(SMSCommunicationHistoryCBE sms, string responseString)
        {
            try
            {
                SMSResponce objSMSResponce = JsonConvert.DeserializeObject<SMSResponce>(responseString);
                sms.TransactionId = objSMSResponce.transId;
                if (!string.IsNullOrEmpty(objSMSResponce.idsms))
                {
                    sms.OperatorResponseCode = objSMSResponce.statussms;
                    if (objSMSResponce.statussms == 5)
                    {
                        sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered;
                        sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;

                    }
                    else if (objSMSResponce.statussms == 2 || objSMSResponce.statussms == 3)
                    {
                        sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.Delivered;
                        sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;

                    }
                    else
                    {
                        sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.Delivered;
                        sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                    }
                    sms.ModificationDate = DateTime.Now;
                    sms.MessageReceiveTime = DateTime.Now;
                    sms.OperatorAttemptCount = 1;
                    sms.GatewayResponse = responseString;
                    SMSCommunicationHistoryBLL.UpdateSecondResponse(sms);
                    LogMessage("SMS sent status Second status updated successfully.");


                }
                else
                {
                    sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.Delivered;
                    sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                    sms.ModificationDate = DateTime.Now;
                    sms.MessageReceiveTime = DateTime.Now;
                    sms.OperatorAttemptCount = 0;
                    SMSCommunicationHistoryBLL.UpdateSecondResponse(sms);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to GET SMS Responce: " + responseString + " Exception Occured :" + ex.Message);
            }
        }

        public class SMSResponce
        {
            public string transId { get; set; }
            public string idsms { get; set; }
            public string sendtime { get; set; }
            public string receivetime { get; set; }
            public int statussms { get; set; }
            public string sender { get; set; }
            public string sms { get; set; }
        }

        public class MobileResponce
        {
            public string Apifor { get; set; }
            public Int32 trans_id { get; set; }
            public string status { get; set; }
            public string message { get; set; }
        }
        #endregion
    }
}
