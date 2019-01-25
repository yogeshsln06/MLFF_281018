using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification
{
    class MobisuiteSMSGateway : SMSGatewayBase
    {
        public override SMSCommunicationHistoryCBE SendSMS(SMSCommunicationHistoryCBE sms)
        {
            LogMessage("Using Mobiduite SMS gateway for sending message.");
            #region Variables
            string createdURL = "";
            string mobileNumber = sms.MobileNumber;
            string messageBody = sms.MessageBody;

            string userName = "demouser";
            string password = "e10adc3949ba59abbe56e057f20f883e";
            string sender = "IMS";
            string dr_url = "http://103.119.145.130:5555/VaaaN/IndonesiaMLFFApi/ResponseSMS";
            // Andrew Tan mobile number 082246492815 (Indonesia)

            #endregion

            #region Send SMS to Mobile phone
            try
            {
                sms.ReferenceNo = Constants.SMSTranscationId(sms.EntryId);
                LogMessage("Trying to send message to customer mobile number. Mobile No.:" + mobileNumber);
                createdURL = "http://webapps.imitra.com:29003/sms_applications/smsb/api_mt_send_message.php?";
                string data = "data=<bulk_sending>" +
  "<username>" + userName + "</username>" +
  "<password>" + password + "</password>" +
  "<priority>high</priority>" +
  "<sender>" + sender + "</sender>" +
  "<dr_url>" + dr_url + "</dr_url>" +
  "<allowduplicate>1</allowduplicate>" +
  "<data_packet>" +
  "<packet>" +
  "<msisdn>" + mobileNumber + "</msisdn>" +
  "<sms>" + messageBody + "</sms>" +
  "<is_long_sms>N</is_long_sms>" +
  "</packet>" +
  "</data_packet>" +
  "</bulk_sending>";

                // Create request
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";//important

                // Send data with request
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                myReq.ContentLength = bytes.Length;

                using (Stream putStream = myReq.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);
                }

                // Get response from the SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();

                // Check response value
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseString));
                System.Xml.XmlReader reader = System.Xml.XmlReader.Create(ms);

                string statusCode = "";

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "status_code")
                        {
                            reader.Read();
                            statusCode = Convert.ToString(reader.Value);
                            try
                            {
                                sms.ResponseCode = Convert.ToInt32(statusCode);
                                LogMessage("Response status code is " + statusCode + ".");
                            }
                            catch (Exception ex)
                            {
                                LogMessage("error in convert status code is " + statusCode + ".");
                            }

                        }
                        if (reader.Name == "transaction_id")
                        {
                            reader.Read();
                            sms.TransactionId = Convert.ToString(reader.Value);
                            LogMessage("Response transactionId is " + sms.TransactionId + ".");
                        }
                    }
                }

                if (statusCode == "2200")
                {
                    sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                }

                sms.GatewayResponse = responseString;

                LogMessage("Response:" + responseString);
                LogMessage("Message sent successfully to mobile.");
            }
            catch (Exception ex)
            {
                //if sending request or getting response is not successful the SMS Gateway Server may do not run
                LogMessage("Failed to send message to customer." + ex.Message);
            }
            #endregion

            return sms;
        }

        public override string SMSStatus(SMSCommunicationHistoryCBE sms)
        {
            throw new NotImplementedException();
        }
        public override List<SMSDetail> ReadSMS()
        {
            throw new NotImplementedException();
        }

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.InboundSMS);
        }
    }
}
