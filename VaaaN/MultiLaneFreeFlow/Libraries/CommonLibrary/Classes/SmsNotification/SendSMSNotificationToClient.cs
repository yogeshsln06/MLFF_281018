using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml.Linq;

namespace VaaaN.ATMS.Libraries.CommonLibrary.Classes.SmsNotification
{
    public class SendSMSNotificationToClient
    {
        #region Variables
        static SerialPort serialPort;
        static Logger.Log.ErrorLogModule logModule = Logger.Log.ErrorLogModule.SMSNotification;
        delegate void SetTextCallback(string text);
        static int sendingMedia = 1;// Modem = 1, SMS Gate Way = 2
        #endregion

        #region Helper Methods

        public static void OpenGSMModemCOMPort()
        {
            try
            {
                if (sendingMedia == 1) // Modem
                {
                    #region Open Com Port

                    serialPort = new SerialPort();

                    string portName = string.Empty;
                    string baudRate = string.Empty;
                    string dataBits = string.Empty;
                    string readTimeout = string.Empty;
                    string writeTimeout = string.Empty;
                    string encoding = string.Empty;
                    string inputData = string.Empty;

                    #region Configuration file
                    try
                    {
                        Logger.Log.Write("Opening modem serial port.", logModule);
                        string path = "C:\\Tollmax\\config\\plaza\\GSMModemConfig.XML";

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(path);
                        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/GSMModemConfig");
                        foreach (XmlNode node in nodeList)
                        {
                            portName = node.SelectSingleNode("PortName").InnerText;
                            baudRate = node.SelectSingleNode("BaudRate").InnerText;
                            dataBits = node.SelectSingleNode("DataBits").InnerText;
                            readTimeout = node.SelectSingleNode("ReadTimeout").InnerText;
                            writeTimeout = node.SelectSingleNode("WriteTimeout").InnerText;
                            encoding = node.SelectSingleNode("Encoding").InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Write("Failed to read XmlData from GSMModemConfig ." + ex.Message, logModule);
                    }
                    #endregion

                    serialPort.PortName = portName;                          //COM1
                    serialPort.BaudRate = Convert.ToInt32(baudRate);         //9600
                    serialPort.DataBits = Convert.ToInt32(dataBits);         //8
                    serialPort.StopBits = StopBits.One;                      //1
                    serialPort.Parity = Parity.None;                         //None
                    serialPort.ReadTimeout = Convert.ToInt32(readTimeout);   //300
                    serialPort.WriteTimeout = Convert.ToInt32(writeTimeout); //300
                    serialPort.Encoding = Encoding.GetEncoding(encoding);
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;

                    serialPort.Open();

                    string cmd = "AT";
                    serialPort.WriteLine(cmd + "\r");
                    serialPort.Write(cmd + "\r");
                    serialPort.WriteLine("AT+CMGF=1");

                    Logger.Log.Write("GSM modem serial port opened successfully.", logModule);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Write("Failed to open port for GSMModem .please check GSMModemConfig ." + ex.Message, logModule);
            }
        }

        public static void CloseGSMModemCOMPort()
        {
            try
            {
                if (sendingMedia == 1) // Modem
                {
                    #region Close port

                    Logger.Log.Write("Closing modem serial port.", logModule);

                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort = null;
                    }

                    Logger.Log.Write("Modem serial port closed successfully.", logModule);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Write("Failed to close port for GSMModem .please check GSMModemConfig." + ex.Message, logModule);
            }
        }

        public static void SendMessageToCustomerFromDatabase(int tmsId, int plazaId)
        {
            bool isSuccess = false;

            try
            {
                //Get the customer to who SMS need to sent 
                CBE.CustomerSmsNotificationCollection smsNotifications = VaaaN.TollMax.Library.TollMaxLibrary.BLL.CustomerSmsNotificationBLL.GetAllUnsent(tmsId, plazaId);

                foreach (TollMax.Library.TollMaxLibrary.CBE.CustomerSmsNotificationCBE smsNotification in smsNotifications)
                {
                    #region Send Message

                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("SMS Status check for SMS Notification id :" + smsNotification.EntryId, logModule);

                    if (smsNotification.SentStatus == "0")
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("SMS Status found unsent for SMSNotification id :" + smsNotification.EntryId, logModule);
                        if (smsNotification.AttemptCount < 5)
                        {
                            if (!string.IsNullOrEmpty(smsNotification.Message))
                            {
                                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Sending customer notification. Details: " + smsNotification.ToString(), logModule);

                                if (sendingMedia == 1)// Modem
                                {
                                    isSuccess = SendMessageViaModem(smsNotification.ContactNumber, smsNotification.Message);
                                }
                                else
                                {
                                    isSuccess = SendMessageViaSmsGateway(smsNotification.ContactNumber, smsNotification.Message);
                                }

                                if (isSuccess)
                                {
                                    smsNotification.SentStatus = "1";
                                    smsNotification.SentDate = DateTime.Now;
                                    smsNotification.AttemptCount = smsNotification.AttemptCount + 1;
                                    VaaaN.TollMax.Library.TollMaxLibrary.BLL.CustomerSmsNotificationBLL.Update(smsNotification);
                                    Logger.Log.Write("Message send Sucessfully :" + smsNotification.EntryId + ".", logModule);
                                }
                            }
                            else
                            {
                                Logger.Log.Write(smsNotification.Message + "Maximum Charecters Reached.Limit is 140.", logModule);
                            }

                        }
                        else
                        {
                            Logger.Log.Write("Maximum Number of attempts reached to send Sms :" + smsNotification.EntryId + ".", logModule);
                        }
                    }

                    #endregion

                    System.Threading.Thread.Sleep(1000 * 5);
                }
            }
            catch (Exception ex)
            {
               Logger.Log.Write("Failed to send SMS." + ex.Message, logModule);
            }
        }

        private static bool SendMessageViaModem(string contactNumber, string messageBody)
        {
            bool isSent = false;

            try
            {
                if (!serialPort.IsOpen)
                {
                    Logger.Log.Write("Modem serial port found disconnected. So trying to connect again.", logModule);
                    OpenGSMModemCOMPort();
                }

                serialPort.WriteLine("AT+CMGS=\"" + contactNumber + "\"");
                serialPort.Write(messageBody + char.ConvertFromUtf32(26));

                // Check deliver status-TODO
                isSent = true;
            }
            catch (Exception ex)
            {
                Logger.Log.Write("Failed to sendMsg from GSMModemConfig ." + ex.Message, logModule);
            }

            return isSent;
        }

        private static bool ReadMessageViaModem(string contactNumber)
        {
            bool isSent = false;

            try
            {
                if (!serialPort.IsOpen)
                {
                    Logger.Log.Write("Modem serial port found disconnected. So trying to connect again.", logModule);
                    OpenGSMModemCOMPort();
                }

                serialPort.WriteLine("AT+CMGS=\"" + contactNumber + "\"");
                //serialPort.Write(messageBody + char.ConvertFromUtf32(26));

                // Check deliver status-TODO
                isSent = true;
            }
            catch (Exception ex)
            {
                Logger.Log.Write("Failed to sendMsg from GSMModemConfig ." + ex.Message, logModule);
            }

            return isSent;
        }

        public static void InsertCustomerNotification(VaaaN.TollMax.Library.TollMaxLibrary.CBE.PrepaidTransactionCBE prepaidTransaction, string eventType)
        {
            CBE.CustomerSmsNotificationCBE smsNotification = new CBE.CustomerSmsNotificationCBE();

            try
            {
                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid tran: " + prepaidTransaction.ToString(), logModule);

                smsNotification.TmsId = prepaidTransaction.TmsId;
                smsNotification.PlazaId = prepaidTransaction.PlazaId;
                smsNotification.TellerId = prepaidTransaction.TellerId;
                smsNotification.CreationDate = prepaidTransaction.StartDateTime;
                smsNotification.DeliveryDate = DateTime.MaxValue;
                smsNotification.DeliveryStatus = "0";// Delivered = 1 and Un Delivered = 0
                smsNotification.SentStatus = "0"; // Sent = 1 Unsent = 0
                smsNotification.SentDate = DateTime.Now;

                CBE.AccountCBE account = new CBE.AccountCBE();
                account.PlazaId = prepaidTransaction.PlazaId;
                account.TmsId = prepaidTransaction.TmsId;
                account.AccountId = prepaidTransaction.AccountId;
                account = BLL.AccountBLL.GetAccountById(account);

                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Account detail: " + account.ToString(), logModule);


                smsNotification.AccountId = prepaidTransaction.AccountId;
                smsNotification.SerialNo = prepaidTransaction.SerialNo;
                smsNotification.EmailId = account.EmailId;
                smsNotification.ContactNumber = account.MobNumber;
                smsNotification.CustomerName = account.FirstName;
                smsNotification.AttemptCount = 0;
                smsNotification.MessageCategory = eventType;
                smsNotification.MediaType = Convert.ToString((int)Constants.CustomerNotificationMedia.SMS);
                smsNotification.ModuleName = "POS";
                smsNotification.Message = CreateMessageForSMS(smsNotification, null, prepaidTransaction);

                VaaaN.TollMax.Library.TollMaxLibrary.BLL.CustomerSmsNotificationBLL.Insert(smsNotification);
            }
            catch (Exception ex)
            {
                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Failed to insert customer notification. Exception: " + ex.Message + smsNotification.ToString(), logModule);
                throw ex;
            }
        }

        public static void InsertExpiryDateReminder(VaaaN.TollMax.Library.TollMaxLibrary.CBE.PrepaidCardCBE prepaidCard)
        {
            CBE.CustomerSmsNotificationCBE smsNotification = new CBE.CustomerSmsNotificationCBE();

            try
            {
                smsNotification.TmsId = prepaidCard.TmsId;
                smsNotification.PlazaId = prepaidCard.PlazaId;
                smsNotification.TellerId = 0;
                smsNotification.CreationDate = DateTime.Now;
                smsNotification.DeliveryDate = DateTime.MaxValue;
                smsNotification.DeliveryStatus = "0";// Delivered = 1 and Un Delivered = 0
                smsNotification.SentStatus = "0"; // Sent = 1 Unsent = 0
                smsNotification.SentDate = DateTime.Now;

                CBE.AccountCBE account = new CBE.AccountCBE();
                account.PlazaId = prepaidCard.PlazaId;
                account.TmsId = prepaidCard.TmsId;
                account.AccountId = prepaidCard.AccountId;
                account = BLL.AccountBLL.GetAccountById(account);
                smsNotification.AccountId = prepaidCard.AccountId;
                smsNotification.SerialNo = prepaidCard.SerialNo;
                smsNotification.EmailId = account.EmailId;
                smsNotification.ContactNumber = account.MobNumber;
                smsNotification.CustomerName = account.FirstName;
                smsNotification.AttemptCount = 0;
                smsNotification.MessageCategory = "expiryreminder";
                smsNotification.MediaType = Convert.ToString((int)Constants.CustomerNotificationMedia.SMS);
                smsNotification.ModuleName = "POS";
                smsNotification.Message = CreateMessageForSMS(smsNotification, prepaidCard, null);

                // Send only one reminder to customer in a month
                VaaaN.TollMax.Library.TollMaxLibrary.CBE.CustomerSmsNotificationCollection notifications = VaaaN.TollMax.Library.TollMaxLibrary.BLL.CustomerSmsNotificationBLL.GetBySerialNo(smsNotification.TmsId, smsNotification.PlazaId, smsNotification.SerialNo);

                bool createReminder = true;
                foreach (VaaaN.TollMax.Library.TollMaxLibrary.CBE.CustomerSmsNotificationCBE notification in notifications)
                {
                    if ((DateTime.Now - notification.CreationDate).TotalDays <= 20 && notification.MessageCategory.ToLower() == smsNotification.MessageCategory.ToLower())
                    {
                        createReminder = false;
                        break;
                    }
                }

                if (createReminder)
                {
                    VaaaN.TollMax.Library.TollMaxLibrary.BLL.CustomerSmsNotificationBLL.Insert(smsNotification);
                }
                else
                {
                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Reminder not required." + smsNotification.ToString(), logModule);
                }
            }
            catch (Exception ex)
            {
                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Failed to insert customer notification." + smsNotification.ToString(), logModule);
                throw ex;
            }
        }

        private static string CreateMessageForSMS(VaaaN.TollMax.Library.TollMaxLibrary.CBE.CustomerSmsNotificationCBE smsNotification, VaaaN.TollMax.Library.TollMaxLibrary.CBE.PrepaidCardCBE prepaidCard, VaaaN.TollMax.Library.TollMaxLibrary.CBE.PrepaidTransactionCBE prepaidTransaction)
        {
            #region Message format samples

            /* 
                //Expiry and Balance Notification Message: 
                Dear Customer, Your <Vehicle Class>, <Vehicle Regn #>, Balance trips are <No. of trips>and Validity is <Expiry date> at <Toll Plaza Name>. Please recharge your Pass. Regards,  <Name of Company>


                //Expiry Notification Message for unlimited trip pass: 
                Dear Customer, Your Pass of  <Vehicle Class>, <Vehicle Regn #>, Expiring on  <Expiry date> at <Toll Plaza Name>. Please recharge your Pass. Regards,  <Name of Company>


                // Sale
                Dear Customer, Thanks for purchasing new pass of Rs. <Amount> for <Vehicle Class>, <Vehicle Regn #> on <Sale Date> at <Toll Plaza>. Regards,  <Name of Company>


                // Recharge
                Dear Customer, Thanks for recharging your pass of Rs. <Recharge Amount> for <Vehicle Class>, <Vehicle Regn #> on <Recharge Date> at <Toll Plaza>. Regards, <Name of Company>
             */

            /*
            //Expiry and Balance Notification Message: 
            Dear Customer, Your <Vehicle Class>, <Vehicle Regn #>, Balance trips are <No. of trips>and Validity is <Expiry date> at <Toll Plaza Name>. Please recharge your Smart card. Regards, PS Toll Roads

            // Sale
            Dear Customer, Thanks for purchasing new pass of Rs. <Amount> for <Vehicle Class>, <Vehicle Regn #> on <Sale Date> at <Toll Plaza>. Regards, PS Toll Roads
 
            // Recharge
            Dear Customer, Thanks for recharging your pass of Rs. <Recharge Amount> for <Vehicle Class>, <Vehicle Regn #> on <Recharge Date> at <Toll Plaza>. Regards, PS Toll Roads

            */

            #endregion

            #region Variables

            string message = "";
            string plazaName = "";
            string companyName = "";
            string vehicleClassName = "";
            string vehRegNo = "";
            string currencySymbol = "Rs.";

            try
            {
                if (plazaConfiguration == null)
                {
                    if (prepaidTransaction != null)
                    {
                        VaaaN.TollMax.Library.TollMaxLibrary.CBE.PlazaConfigurationCollection plazaConfigurations = VaaaN.TollMax.Library.TollMaxLibrary.BLL.PlazaConfigurationBLL.GetAll(prepaidTransaction.TmsId, prepaidTransaction.PlazaId);

                        if (plazaConfigurations != null && plazaConfigurations.Count > 0)
                        {
                            plazaConfiguration = plazaConfigurations[0];
                            currencySymbol = plazaConfiguration.CurrencySymbol;
                        }
                        else
                        {
                            plazaConfiguration = null;
                            VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Plaza configuration record nofound.", logModule);
                        }
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("prepaidTransaction found null.", logModule);
                    }
                }
                else
                {
                    currencySymbol = plazaConfiguration.CurrencySymbol;
                }
            }
            catch (Exception ex)
            {
                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Failed to get  plaza configurations." + ex.Message, logModule);
                plazaConfiguration = null;
            }

            #endregion

            #region Get plaza name/ company name

            VaaaN.TollMax.Library.TollMaxLibrary.CBE.PlazaCollection plazas = BLL.PlazaBLL.GetAll(smsNotification.TmsId);

            foreach (CBE.PlazaCBE plaza in plazas)
            {
                if (plaza.PlazaId == smsNotification.PlazaId)
                {
                    plazaName = plaza.PlazaName;
                    companyName = plaza.CompanyName;
                    break;
                }
            }

            #endregion

            if (masterConfig == null)
            {
                masterConfig = (VaaaN.TollMax.Library.TollMaxLibrary.ConfigurationClasses.MasterConfigProperties)
                VaaaN.TollMax.Library.SharedLibrary.Serialization.Serialization.Deserialize(
                typeof(VaaaN.TollMax.Library.TollMaxLibrary.ConfigurationClasses.MasterConfigProperties), VaaaN.TollMax.Library.TollMaxLibrary.Constants.plazaConfigDirectory + "MasterConfig.xml");
            }

            #region Reliance
            if (masterConfig.SMSFormatName.ToLower().Equals(VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormatName[(int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormat.Reliance - 1].ToLower()))
            {
                if (smsNotification.MessageCategory.ToLower() == "sale")
                {
                    #region Sale

                    if (prepaidTransaction != null)
                    {
                        vehicleClassName = GetVehicleClassNameById(prepaidTransaction.TmsId, prepaidTransaction.PlazaId, prepaidTransaction.VehicleClassId).ToUpper();
                        vehRegNo = prepaidTransaction.VehRegNum.ToUpper();
                        message = "Dear Customer, Thanks for purchasing new pass of " + currencySymbol + " " + String.Format("{0:0.00}", prepaidTransaction.PaidAmount) + " for " + vehicleClassName + ", " + vehRegNo + " on " + prepaidTransaction.DateTime.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat) + " at " + plazaName + ". Regards, " + companyName;
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid card transaction found null.", VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SMS);
                    }

                    #endregion
                }
                else if (smsNotification.MessageCategory.ToLower() == "recharge")
                {
                    #region Recharge

                    if (prepaidTransaction != null)
                    {
                        vehicleClassName = GetVehicleClassNameById(prepaidTransaction.TmsId, prepaidTransaction.PlazaId, prepaidTransaction.VehicleClassId).ToUpper();
                        vehRegNo = prepaidTransaction.VehRegNum.ToUpper();

                        message = "Dear Customer, Thanks for recharging your pass of " + currencySymbol + " " + String.Format("{0:0.00}", prepaidTransaction.PaidAmount) + " for " + vehicleClassName + ", " + vehRegNo + " on " + prepaidTransaction.DateTime.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat) + " at " + plazaName + ". Regards, " + companyName;
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid card transaction found null.", VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SMS);
                    }

                    #endregion
                }
                else if (smsNotification.MessageCategory.ToLower() == "expiryreminder")
                {
                    #region Expiry date and trip notification

                    vehicleClassName = GetVehicleClassNameById(prepaidCard.TmsId, prepaidCard.PlazaId, prepaidCard.VehicleClassId).ToUpper();
                    vehRegNo = prepaidCard.VehRegNum.ToUpper();

                    if (prepaidCard != null)
                    {
                        //Expiry and Balance Notification Message: 
                        if (prepaidCard != null && prepaidCard.CardType == (int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.CardType.Trip)
                        {
                            message = "Dear Customer, Your " + vehicleClassName + ", " + vehRegNo + ", Balance trips are " + prepaidCard.Balance + " and Validity is " + prepaidCard.ExpiryDate.ToString("dd-MMM-yyyy") + " at " + plazaName + ". Please recharge your Pass. Regards, " + companyName;
                        }

                        //Expiry Notification Message for unlimited trip pass:
                        if (prepaidCard != null && prepaidCard.CardType == (int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.CardType.Time)
                        {
                            message = "Dear Customer, Your " + vehicleClassName + ", " + vehRegNo + ", Balance trips are " + "" + " and Validity is " + prepaidCard.ExpiryDate.ToString("dd-MMM-yyyy") + " at " + plazaName + ". Please recharge your Pass. Regards, " + companyName;
                        }
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid card found null.", VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SMS);
                    }

                    #endregion
                }

                // Blacklist
                // Refund
            }
            #endregion

            #region Bangladesh Regnum
            //Purchasing:
            //Dear Customer,Thanks for purchasing new Smart Card of Tk...... for Truck/Lorry............ on Date & Time at Sitakunda Axle Load Control Station. O&M: Regnum

            //Recharging:
            //Dear Customer,Thanks for recharging new Smart Card of Tk...... for Truck/Lorry............ on Date & Time at Sitakunda Axle Load Control Station. O&M: Regnum

            if (masterConfig.SMSFormatName.ToLower().Equals(VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormatName[(int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormat.RegnumBangladesh - 1].ToLower()))
            {
                if (smsNotification.MessageCategory.ToLower() == "sale")
                {
                    #region Sale

                    if (prepaidTransaction != null)
                    {
                        vehicleClassName = GetVehicleClassNameById(prepaidTransaction.TmsId, prepaidTransaction.PlazaId, prepaidTransaction.VehicleClassId).ToUpper();
                        vehRegNo = prepaidTransaction.VehRegNum.ToUpper();
                        message = "Dear Customer,Thanks for purchasing new Smartcard of " + currencySymbol + String.Format("{0:0.00}", prepaidTransaction.PaidAmount) + " for " + vehicleClassName + " on " + prepaidTransaction.DateTime.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat) + " at Sitakunda Axle Load Control Station. O and M:Regnum";
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid card transaction found null.", VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SMS);
                    }

                    #endregion
                }
                else if (smsNotification.MessageCategory.ToLower() == "recharge")
                {
                    #region Recharge

                    if (prepaidTransaction != null)
                    {
                        vehicleClassName = GetVehicleClassNameById(prepaidTransaction.TmsId, prepaidTransaction.PlazaId, prepaidTransaction.VehicleClassId).ToUpper();
                        vehRegNo = prepaidTransaction.VehRegNum.ToUpper();

                        message = "Dear Customer,Thanks for recharging new Smartcard of " + currencySymbol + String.Format("{0:0.00}", prepaidTransaction.PaidAmount) + " for " + vehicleClassName + " on " + prepaidTransaction.DateTime.ToString(VaaaN.TollMax.Library.TollMaxLibrary.Constants.dateTimeFormat) + " at Sitakunda Axle Load Control Station. O and M:Regnum";
                    }
                    else
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Prepaid card transaction found null.", VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.ErrorLogModule.SMS);
                    }

                    #endregion
                }

                // Expiry Reminder
                // Blacklist
                // Refund
            }
            #endregion

            return message;
        }

        private static string GetVehicleClassNameById(int tmsId, int plazaId, int vehicleClassId)
        {
            string result = "";

            VaaaN.TollMax.Library.TollMaxLibrary.CBE.VehicleClassCollection vehicleClasses = VaaaN.TollMax.Library.TollMaxLibrary.BLL.VehicleClassBLL.GetAll(tmsId, plazaId);
            foreach (VaaaN.TollMax.Library.TollMaxLibrary.CBE.VehicleClassCBE veh in vehicleClasses)
            {
                if (veh.VehicleClassId == vehicleClassId)
                {
                    result = veh.VehicleClassName;
                    break;
                }
            }

            return result;
        }

        public static bool SendMessageViaSmsGateway(string contactNumber, string messageBody)
        {
            bool isSent = false;
            string createdURL = "";

            try
            {
                if (masterConfig == null)
                {
                    masterConfig = (VaaaN.TollMax.Library.TollMaxLibrary.ConfigurationClasses.MasterConfigProperties)
                    VaaaN.TollMax.Library.SharedLibrary.Serialization.Serialization.Deserialize(
                    typeof(VaaaN.TollMax.Library.TollMaxLibrary.ConfigurationClasses.MasterConfigProperties), VaaaN.TollMax.Library.TollMaxLibrary.Constants.plazaConfigDirectory + "MasterConfig.xml");
                }

                // Reliance URL
                if (masterConfig.SMSFormatName.ToLower().Equals(VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormatName[(int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormat.Reliance - 1].ToLower()))
                {
                    createdURL = "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to=" + contactNumber + "&from=BDROAD&text=" + messageBody + "&selfid=true&alert=1&dlrreq=true";
                }
                else if (masterConfig.SMSFormatName.ToLower().Equals(VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormatName[(int)VaaaN.TollMax.Library.TollMaxLibrary.Constants.SMSFormat.RegnumBangladesh - 1].ToLower()))
                {
                    // Regnum Bangladesh (Ranks telecom)- Added on 16-Oct-17
                    string userName = masterConfig.SMSGatewayUserName.Trim();
                    string password = masterConfig.SMSGatewayPassword.Trim();
                    string sender = masterConfig.SMSSenderMobileNumber.Trim();

                    createdURL = "http://websms.rankstelecom.com/api/v3/sendsms/plain?user=" + userName + "&password=" + password + "&sender=" + sender + "&SMSText=" + messageBody + "&GSM=" + contactNumber + "&type=longSMS";
                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Sending message...", logModule);
                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Message: " + messageBody + Environment.NewLine + "Mobile No.:" + contactNumber, logModule);
                }

                //Create the request and send data to the SMS Gateway Server by HTTP connection
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);

                //Get response from the SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();

                if (responseString.Length > 50)
                {
                    if (GetRanksTelecomResponseStatus(responseString) == 0)
                    {
                        isSent = true;
                    }
                }

                // Confirm delivery status-TO DO

                if (isSent)
                {
                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Message sent successfully. Response:" + responseString, logModule);
                }
                else
                {
                    VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Message not sent due to some error. Response:" + responseString, logModule);
                }
            }
            catch (Exception ex)
            {
                //if sending request or getting response is not successful the SMS Gateway Server may do not run
                isSent = false;
                VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Failed to send message to customer." + ex.Message, logModule);
            }

            return isSent;
        }

        private static Int32 GetRanksTelecomResponseStatus(string responseString)
        {
            int statusCode = -1;// Invalid value
            // Confirm status in response
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseString));
            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(ms);

            while (reader.Read())
            {
                string elementName = "status";
                if (reader.NodeType == XmlNodeType.Element)
                {
                    elementName = reader.LocalName;

                    if (elementName.ToLower() == "status")
                    {
                        statusCode = Convert.ToInt32(reader["status"]);
                        if (statusCode == 0)
                        {
                            break;
                        }
                    }
                }
                reader.MoveToElement();
            }

            VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("Response status: ", logModule);
            switch (statusCode)
            {
                case 0:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("0 Request was successful (all recipients)", logModule);
                        break;
                    }

                case -1:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-1 Error in processing the request", logModule);
                        break;
                    }

                case -2:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-2 Not enough credits on a specific account", logModule);
                        break;
                    }

                case -3:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-3 Targeted network is not covered on specific account", logModule);
                        break;
                    }

                case -5:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-5 Username or password is invalid", logModule);
                        break;
                    }

                case -6:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-6 Destination address is missing in the request", logModule);
                        break;
                    }

                case -10:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("10 Username is missing in the request", logModule);
                        break;
                    }

                case -11:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("11 Password is missing in the request", logModule);
                        break;
                    }

                case -13:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("13 Number is not recognized by the platform", logModule);
                        break;
                    }

                case -22:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-22 Incorrect format, caused by syntax error", logModule);
                        break;
                    }

                case -23:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-23 General error, reasons may vary", logModule);
                        break;
                    }

                case -26:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-26 General API error, reasons may vary", logModule);
                        break;
                    }

                case -28:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-28 Invalid PushURL in the request", logModule);
                        break;
                    }

                case -33:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-33 Duplicated MessageID in the request", logModule);
                        break;
                    }

                case -34:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-34 Sender name is not allowed", logModule);
                        break;
                    }

                case -99:
                    {
                        VaaaN.TollMax.Library.SharedLibrary.FileIO.Log.Write("-99 Error in processing request, reasons may vary", logModule);
                        break;
                    }
            }

            return statusCode;
        }

        #endregion
    }
}
