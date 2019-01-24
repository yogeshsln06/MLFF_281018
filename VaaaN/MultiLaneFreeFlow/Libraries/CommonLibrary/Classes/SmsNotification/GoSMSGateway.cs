using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification
{
    class GoSMSGateway : SMSGatewayBase
    {
        public override SMSCommunicationHistoryCBE SendSMS(SMSCommunicationHistoryCBE sms)
        {
            LogMessage("Using Go SMS gateway for sending message.");
            #region Variables
            string createdURL = "https://secure.gosmsgateway.com/api/send.php?";
            string PostedURL = "https://secure.gosmsgateway.com/api/send.php?";
            string mobileNumber = sms.MobileNumber;
            string messageBody = sms.MessageBody;
            string userName = "balitower";
            string password = "gosms37297";
            string dr_url = "http://103.119.145.130:5555/VaaaN/IndonesiaMLFFApi/ResponseSMS";
            var responseString = "";
            HttpWebResponse response = null;
            #endregion

            #region Send SMS to Mobile phone
            try
            {
                LogMessage("Trying to send message to customer mobile number. Mobile No.:" + mobileNumber);
                var postData = "username=" + userName + "";
                postData += "&mobile=" + mobileNumber + "";
                postData += "&message=" + messageBody + "";
                postData += "&password=" + password + "";
                PostedURL = createdURL + postData;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostedURL);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                //sms.ResponseCode = (int)response.StatusCode;
                //sms.GatewayResponse = responseString;
              
                //if (e.Status == WebExceptionStatus.ProtocolError)
                //{
                //    sms.ResponseCode = (int)response.StatusCode;
                //    sms.GatewayResponse = responseString;
                //    response = (HttpWebResponse)e.Response;
                //}
                //else
                //{
                //    LogMessage("Failed to send message to customer. WebException:" + e.Message + " Error: " + e.Status + "");

                //}
            }
            catch (Exception ex)
            {
                //if sending request or getting response is not successful the SMS Gateway Server may do not run
                LogMessage("Failed to send message to customer." + ex.Message);
            }
            #endregion
            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (responseString.Contains("1701"))
                {
                    sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                }
                sms.TransactionId = string.Empty;
                sms.ResponseCode = (int)response.StatusCode;
                sms.GatewayResponse = responseString;
            }
            else
            {
                LogMessage("Failed to send message to customer. due to no response found");
               
            }
            return sms;
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
