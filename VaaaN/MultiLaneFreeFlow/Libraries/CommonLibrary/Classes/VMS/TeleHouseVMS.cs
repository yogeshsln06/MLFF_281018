using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.VMS
{
    class TeleHouseVMS : VMSBase
    {
        public override bool SendTollRateMessage(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates)
        {
            #region Variables

            // Get from configuration file-TODO
            //string apiURL = "http://vms.intitekno.com/index.php/getapi";
            string apiURL = "http://10.213.0.63/node/index.php/getapi";
            string userName = "admin";
            string password = "admin132!";
            string x_api_key = "cda2c99fbf5e19f20d331299c15a4491";

            // 1=motorcycle
            // 2 = small car
            // 3 = medium car
            // 4 = big car

            // lane type id 1= normal, 2 = trans jakarta
            string startTime = "";
            string endTime = "";
            string motorCycleNormalFare = "";
            string motorCycleTrjFare = "";
            string smallcarNormalFare = "";
            string smallCarTrjFare = "";
            string mediumCarNormalFare = "";
            string mediumCarTrjFare = "";
            string bigCarNormalFare = "";
            string bigCarTrjFare = "";

            bool isSuccess = false;

            #endregion

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in tollRates)
            {
                #region Get data for json
                if (tr.VehicleClassId == 1)// Motorcycle
                {
                    if (tr.LaneTypeId == 1)// Normal
                    {
                        startTime = "\"start_time\":\"" + tr.StartTime.Substring(0, 2) + ":" + tr.StartTime.Substring(3, 2) + ":00\",";
                        endTime = "\"end_time\":\"" + tr.EndTime.Substring(0, 2) + ":" + tr.EndTime.Substring(3, 2) + ":00\",";

                        motorCycleNormalFare = "\"motorcycle_normal_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }

                    if (tr.LaneTypeId == 2)// trj
                    {
                        motorCycleTrjFare = "\"motorcycle_trj_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }
                }

                if (tr.VehicleClassId == 2)// small car
                {
                    if (tr.LaneTypeId == 1)// Normal
                    {
                        smallcarNormalFare = "\"small_car_normal_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }

                    if (tr.LaneTypeId == 2)// trj
                    {
                        smallCarTrjFare = "\"small_car_trj_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }
                }

                if (tr.VehicleClassId == 3)// medium car
                {
                    if (tr.LaneTypeId == 1)// Normal
                    {
                        mediumCarNormalFare = "\"medium_car_normal_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }

                    if (tr.LaneTypeId == 2)// trj
                    {
                        mediumCarTrjFare = "\"medium_car_trj_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }
                }

                if (tr.VehicleClassId == 4)// big car
                {
                    if (tr.LaneTypeId == 1)// Normal
                    {
                        bigCarNormalFare = "\"big_car_normal_fare\":\"" + Convert.ToInt32(tr.Amount) + "\",";
                    }

                    if (tr.LaneTypeId == 2)// trj
                    {
                        bigCarTrjFare = "\"big_car_trj_fare\":\"" + Convert.ToInt32(tr.Amount) + "\"";
                    }
                }
                #endregion
            }

            #region Create final json
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(startTime);
            json.AppendLine(endTime);

            json.AppendLine(motorCycleNormalFare);
            json.AppendLine(motorCycleTrjFare);

            json.AppendLine(smallcarNormalFare);
            json.AppendLine(smallCarTrjFare);

            json.AppendLine(mediumCarNormalFare);
            json.AppendLine(mediumCarTrjFare);

            json.AppendLine(bigCarNormalFare);
            json.AppendLine(bigCarTrjFare);

            json.Append("}");
            LogMessage("JSON:" + json.ToString());
            #endregion

            #region Send api request
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(apiURL);

            myReq.Method = "POST";
            myReq.ContentType = "application/json; charset=UTF-8";
            myReq.Headers.Add("x-api-key", x_api_key);
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(userName + ":" + password));
            myReq.Headers.Add("Authorization", "Basic " + svcCredentials);

            byte[] bytes = Encoding.UTF8.GetBytes(json.ToString());

            myReq.ContentLength = bytes.Length;
            using (Stream putStream = myReq.GetRequestStream())
            {
                putStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                //Get response from the SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();

                if (myResp.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true;
                }
                respStreamReader.Close();
                myResp.Close();
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to get response from server." + ex.Message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.VMS);
            }
            #endregion

            return isSuccess;
        }

        private void LogMessage(string msg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(msg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.VMS);
        }
    }
}
