using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SMSService
{
    public partial class MainService : ServiceBase
    {
        #region Variable
        Thread threadSendSMS;
        private volatile bool stopThread = false;
        VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSGatewayController smsGatewayController;
        private MessageQueue smsMessageQueue;
        #endregion

        #region Constructor
        public MainService()
        {
            InitializeComponent();

            //OnStart(new string[] { "" });
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

        #region OnStart/ OnStop
        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage("Trying to start SMS service...");

                try
                {
                    LogMessage("Creating SMS gateway controller.");
                    smsGatewayController = new VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSGatewayController();
                    LogMessage("SMS gateway controller created successfully.");
                }
                catch (Exception ex)
                {
                    LogMessage("Failed to create sms gateway controller." + ex.Message);
                }

                this.smsMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.smsMessageQueue);
                smsMessageQueue.PeekCompleted += new PeekCompletedEventHandler(smsMessageQueue_PeekCompleted);
                smsMessageQueue.BeginPeek();

                LogMessage("Trying to start threadSendSMS...");
                threadSendSMS = new Thread(SendSMSThreadFunction);
                threadSendSMS.IsBackground = true;
                threadSendSMS.Name = "threadSendSMS";
                threadSendSMS.Start();
                LogMessage("threadSendSMS started successfully.");

                LogMessage("SMS service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start SMS service." + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogMessage("Stopping SMS service.");
                #region Stop SMS Sender thread
                try
                {
                    LogMessage("Trying to stop threadSendSMS...");
                    stopThread = true;
                    Thread.Sleep(200);

                    if (threadSendSMS != null && threadSendSMS.IsAlive)
                    {
                        threadSendSMS.Abort();
                    }

                    threadSendSMS = null;
                    LogMessage("The threadSendSMS thread has been stopped.");
                }
                catch (Exception ex)
                {
                    LogMessage("Error in stopping threadSendSMS thread function. GoSmsService cannot be stopped. " + ex.ToString());
                }

                #endregion

                smsMessageQueue.PeekCompleted -= new PeekCompletedEventHandler(smsMessageQueue_PeekCompleted);

                LogMessage("SMS service stopped successfully.");
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

        void smsMessageQueue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            bool receiveRecord = false;
            MessageQueue mq = (MessageQueue)sender;

            Message m = (Message)mq.EndPeek(e.AsyncResult);
            m.Formatter = new BinaryMessageFormatter();

            try
            {
                if (m != null)
                {
                    m.Formatter = new BinaryMessageFormatter();

                    if (m.Body != null)
                    {
                        LogMessage("New SMS found in SMS Message queue.");
                        if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail)
                        {
                            VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = (VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail)m.Body;

                            #region Save balance notification in database
                            try
                            {
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE smsOutgoing = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                                smsOutgoing.EntryId = 0;
                                smsOutgoing.TmsId = 1;
                                smsOutgoing.CustomerAccountId = smsDetail.AccountId;
                                smsOutgoing.CustomerName = smsDetail.CustomerName;
                                smsOutgoing.MobileNumber = smsDetail.SenderMobileNumber;
                                smsOutgoing.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                                smsOutgoing.MessageBody = smsDetail.SMSMessage;
                                smsOutgoing.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                                smsOutgoing.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                                smsOutgoing.MessageSendDateTime = DateTime.Now;
                                smsOutgoing.MessageReceiveTime = DateTime.Now;
                                smsOutgoing.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                                smsOutgoing.AttemptCount = 0;
                                smsOutgoing.CreationDate = Convert.ToDateTime(DateTime.Now.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24H));
                                smsOutgoing.ModificationDate = DateTime.Now;
                                smsOutgoing.ModifiedBy = 0;

                                LogMessage("Inserting sms communication history. Detail:" + smsOutgoing.ToString());
                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(smsOutgoing);
                                LogMessage("SMS communication history updated successfully.");
                            }
                            catch (Exception ex)
                            {
                                LogMessage("Failed to insert SMS communication history." + ex.Message);
                            }
                            #endregion

                            receiveRecord = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Error in peeking inbox queue. " + ex.ToString());
                receiveRecord = false;
            }
            finally
            {
                if (receiveRecord)
                {
                    mq.Receive();
                }

                smsMessageQueue.BeginPeek();
            }
        }

        #endregion

        #region Helper Methods

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.OutboundSMS);
        }

        private void SendSMSThreadFunction()
        {
            while (!stopThread)
            {
                try
                {
                    SendSMS();
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

        private void SendSMS()
        {
            try
            {
                // Get unsent outgoing message from the database 
                string query = " WHERE NVL(ATTEMPT_COUNT,0) < 3 AND SENT_STATUS = " + (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent + " AND MESSAGE_DIRECTION = " + (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection unsentSMSes = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.GetFilteredRecords(query);
                bool DataProcess = true;
                // Send message to customer
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms in unsentSMSes)
                {
                    DataProcess = true;
                    // Send message via SMS Gateway
                    // Message will be sent if not older than 2 hours. attempt count should be less thyan equal to 5

                    if ((DateTime.Now - sms.CreationDate).TotalHours <= 2)
                    {
                        if (sms.AttemptCount < 3)
                        {
                            LogMessage("SMS sending attempt count is greater than 3 so will not be sent. SMS entry id: " + sms.EntryId + " Attempt: " + sms.AttemptCount + " ResponseCode :" + sms.ResponseCode);
                            if (sms.AttemptCount > 0)
                            {
                                if (sms.OperatorResponseCode != 3701)
                                {

                                    if ((DateTime.Now - sms.MessageReceiveTime).TotalSeconds < 60)
                                    {
                                        DataProcess = false;
                                    }
                                }
                                else {
                                    DataProcess = false;
                                }
                            }
                            if (DataProcess)
                            {
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE smsResponse = smsGatewayController.SendSMS(sms);
                                #region Update SMS sent status in database
                                try
                                {
                                    LogMessage("Trying to update sms sent status in database.");
                                    sms.AttemptCount++;
                                    // Update status in database
                                    smsResponse.AttemptCount = sms.AttemptCount;
                                    smsResponse.MessageSendDateTime = DateTime.Now;
                                    smsResponse.ModificationDate = DateTime.Now;
                                    if (smsResponse.ResponseCode == 2200)
                                    {
                                        smsResponse.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                                        LogMessage("2200 status received.");
                                    }

                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.UpdateFirstResponse(smsResponse);
                                    LogMessage("SMS sent status updated successfully.");

                                    //if (isSent)
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    LogMessage("Failed to send SMS. Updating attempt count in database.");

                                    //    sms.AttemptCount++;
                                    //    VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Update(sms);

                                    //    LogMessage("SMS attempt count updated successfully.");
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    LogMessage("Failed tp update message sent status in database." + ex.Message);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            LogMessage("SMS sending attempt count is greater than 5 so will not be sent. SMS entry id: " + sms.EntryId + " Attempt: " + sms.AttemptCount);
                        }
                    }
                    //else
                    //{
                    //    LogMessage("Message is 2 hours older so will not be sent. Message Entry Id:" + sms.EntryId);
                    //}
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send SMS." + ex.Message);
            }
            finally
            {
                Thread.Sleep(200);
            }
        }

        #endregion
    }
}
