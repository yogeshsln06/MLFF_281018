using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static string BroadCastBalance(CustomerVehicleCBE objCustomerVehicleCBE)
        {
            string TransId = objCustomerVehicleCBE.EntryId.ToString();
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/vehicles/account");
                request.Headers.Add("Authorization", Authorization);
                request.Accept = "application/json";
                var postData = "vehicle_registration_certificate_number=" + objCustomerVehicleCBE.VehicleRCNumber + "";
                postData += "&amount=" + objCustomerVehicleCBE.AccountBalance + "";
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


        public static string BroadCastNotification(SMSCommunicationHistoryCBE objSMSCommunicationHistoryCBE)
        {
            string TransId = objSMSCommunicationHistoryCBE.EntryId.ToString();
            var responseString = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/api/vehicles/account");
                request.Headers.Add("Authorization", Authorization);
                request.Accept = "application/json";
                var postData = "vehicle_registration_certificate_number=" + objSMSCommunicationHistoryCBE.VehicleRCNumber + "";
                postData += "&title=" + objSMSCommunicationHistoryCBE.Subject + "";
                postData += "&body=" + objSMSCommunicationHistoryCBE.MessageBody + "";
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

        private static void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MobileWebAPI);
        }
    }
}
