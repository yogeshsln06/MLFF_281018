using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.MobileBroadCast
{
    public static class BrodcastDataMobile
    {
        #region Variables
        static string BaseURL = "https://erp-dev.molecool.id/";
        static string Authorization = "1adbb3178591fd5bb0c248518f39bf6d";
        static HttpWebResponse response = null;
        #endregion

        public static string BroadCastBalance(DataRow row)
        {
            string TransId = row["ENTRY_ID"].ToString();
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/vehicles/account");
                request.Headers.Add("Authorization", "1adbb3178591fd5bb0c248518f39bf6d");
                request.Headers.Add("Accept-Language", "en");
                request.Accept = "application/json";
                var postData = "residentId=" + row["RESIDENT_ID"].ToString() + "";
                postData = "&vehicleId=" + row["ENTRY_ID"].ToString() + "";
                postData += "&amount=" + row["ACCOUNT_BALANCE"].ToString() + "";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                }
                else
                {
                    LogMessage("Transcation Id = " + TransId + " WebException " + e.Message + ".");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Transcation Id = " + TransId + " Exception " + ex.Message + ".");
            }

            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.Replace("\"status\"", "\"Apifor\": \"balance\",\"trans_id\": \"" + TransId + "\", \"status\" ");
            }
            else
            {
                LogMessage("Transcation Id = " + TransId + " response is null.");
            }
            return responseString;
        }


        public static string BroadCastNotification(DataRow row)
        {
            string TransId = row["ENTRY_ID"].ToString(); //objSMSCommunicationHistoryCBE.EntryId.ToString();
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/vehicles/notification");
                request.Headers.Add("Authorization", "1adbb3178591fd5bb0c248518f39bf6d");
                request.Headers.Add("Accept-Language", "en");
                request.Accept = "application/json";
                var postData = "residentId=" + row["RESIDENT_ID"].ToString() + "";
                postData = "&vehicleId=" + row["VehicleId"].ToString() + "";
                postData += "&title=" + row["TRANSACTION_SUBJECT"].ToString() + "";
                postData += "&body=" + row["MESSAGE_BODY"].ToString() + "";

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                }
                else
                {
                    LogMessage("Transcation Id = " + TransId + " WebException " + e.Message + ".");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Transcation Id = " + TransId + " Exception " + ex.Message + ".");
            }

            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.Replace("\"status\"", "\"Apifor\": \"notification\",\"trans_id\": \"" + TransId + "\", \"status\" ");
            }
            else
            {
                LogMessage("Transcation Id = " + TransId + " response is null.");
            }
            return responseString;
        }

        public static string SignUp(CustomerAccountCBE objCustomerAccount)
        {
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/sign-up");
                request.Accept = "application/json";
                var postData = "resident_id=" + objCustomerAccount.ResidentId + "";
                postData += "&email=" + objCustomerAccount.EmailId + "";
                postData += "&name=" + objCustomerAccount.FirstName + "";
                postData += "&address=" + objCustomerAccount.Address + "";
                postData += "&phone=" + objCustomerAccount.MobileNo + "";
                //postData += "&password=" + Constants.SMSTranscationId(objCustomerAccount.AccountId) + "";
                postData += "&source=1";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                }
                else
                {
                    LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " WebException " + e.Message + ".");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " Exception " + ex.Message + ".");
            }

            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.Replace("\"status\"", "\"Apifor\": \"SignUp\",\"trans_id\": \"" + objCustomerAccount.AccountId + "\", \"status\" ");
            }
            else
            {
                LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " response is null.");
            }
            return responseString;
        }

        public static string UpdateUser(CustomerAccountCBE objCustomerAccount)
        {
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/users/update");
                request.Headers.Add("Authorization", Authorization);
                request.Accept = "application/json";
                request.Headers.Add("Accept-Language", "id");
                var postData = "resident_id=" + objCustomerAccount.ResidentId + "";
                postData += "&email=" + objCustomerAccount.EmailId + "";
                postData += "&name=" + objCustomerAccount.FirstName + "";
                postData += "&address=" + objCustomerAccount.Address + "";
                postData += "&phone=" + objCustomerAccount.MobileNo + "";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                }
                else
                {
                    LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " WebException " + e.Message + ".");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " Exception " + ex.Message + ".");
            }

            if (response != null)
            {
                int code = (int)response.StatusCode;
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString = responseString.Replace("\"status\"", "\"Apifor\": \"UpdateUser\",\"trans_id\": \"" + objCustomerAccount.AccountId + "\", \"status\" ");
            }
            else
            {
                LogMessage("Transcation Id = " + objCustomerAccount.AccountId + " response is null.");
            }
            return responseString;
        }

        public static bool SendEmail(StringBuilder body, string SendTo, string Subject)
        {
            bool isSuccess = false;

            try
            {
                string fromMail = "mlff@balitower.co.id";
                string mailPassword = "SXA$9c#*!";
                int port = 587;
                string hostName = "mail.balitower.co.id";

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.To.Add(SendTo);

                    SmtpClient SmtpServer = new SmtpClient();
                    SmtpServer.Credentials = new System.Net.NetworkCredential(fromMail, mailPassword);
                    SmtpServer.Port = port;
                    SmtpServer.Host = hostName;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(fromMail);
                    mail.Subject = Subject;
                    mail.Body = body.ToString();
                    mail.To.Add(SendTo);
                    mail.IsBodyHtml = false;

                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    // Attachment

                    SmtpServer.Send(mail);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send email." + ex.Message);
            }

            return isSuccess;
        }

        private static void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MobileWebAPI);
        }


    }
}
