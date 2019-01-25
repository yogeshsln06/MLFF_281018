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
            string createdURL = "https://secure.gosmsgateway.com/api_balitowerlong/sendsms.php?";
            string mobileNumber = sms.MobileNumber;
            string messageBody = sms.MessageBody;
            string userName = "balitower";
            string password = "gosms37297";
            var responseString = "";
            HttpWebResponse response = null;
            #endregion

            #region Send SMS to Mobile phone
            try
            {
                sms.ReferenceNo = Constants.SMSTranscationId(sms.EntryId);
                var postData = "username=balitower";
                postData += "&mobile=" + mobileNumber + "";
                postData += "&message=" + messageBody + "";
                postData += "&auth=" + Constants.MD5Hash(userName + password + mobileNumber) + "";
                postData += "&trxid=" + sms.ReferenceNo + "";
                postData += "&type=0";
                LogMessage("trying to sending : " + postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createdURL + postData);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                LogMessage("Failed to send message to customer. WebException:" + e.Message + " SMS Id: " + sms.EntryId + "");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send message to customer. Exception:" + ex.Message + " SMS Id: " + sms.EntryId + "");
            }
            #endregion
            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                string[] responcelist = Constants.SubStringSMSResponce(responseString).Split(',');
                sms.ResponseCode = Convert.ToInt32(responcelist[0].ToString());
                if (sms.ResponseCode == 1701)
                {
                    sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Sent;
                }
                sms.TransactionId = responcelist[1].ToString();
                sms.GatewayResponse = responseString;
            }
            else
            {
                LogMessage("Failed to send message to customer. due to no response found");

            }
            return sms;
        }

        public override string SMSStatus(SMSCommunicationHistoryCBE sms)
        {
            LogMessage("Using Go SMS gateway for Delivery Status message.");
            #region Variables
            string createdURL = "https://secure.gosmsgateway.com/api_balitowerlong/statusmsg.php?";
            string key = sms.TransactionId;
            string userName = "balitower";
            string password = "gosms37297";
            var responseString = "";
            HttpWebResponse response = null;
            #endregion

            #region Send SMS to Mobile phone
            try
            {
                var postData = "username=" + userName + "";
                postData += "&auth=" + Constants.MD5Hash(userName + password) + "";
                postData += "&key=" + key + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createdURL + postData);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                LogMessage("Failed to SMS Status message to customer. WebException:" + e.Message + " SMS Id: " + sms.EntryId + "");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to SMS Status message to customer. Exception:" + ex.Message + " SMS Id: " + sms.EntryId + "");
            }
            #endregion
            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.ToLower().Replace("\"idsms\"", "\"transId\": \"" + key + "\", \"idsms\" ");
            }
            else
            {
                LogMessage("Failed to SMS Status message to customer. due to no response found");

            }
            return responseString;
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
