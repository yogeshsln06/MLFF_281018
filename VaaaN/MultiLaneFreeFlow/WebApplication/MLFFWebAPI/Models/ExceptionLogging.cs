using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MLFFWebAPI.Models
{
    public static class ExceptionLogging
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
        private static string rootpath = HttpContext.Current.Server.MapPath("~/events/" + DateTime.Now.ToString("dd-MMM-yyyy") + "/");
        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = HttpContext.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                string filepath = rootpath + "Log/";
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                // string filepath = HttpContext.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path

                //if (!Directory.Exists(filepath))
                //{
                //    Directory.CreateDirectory(filepath);

                //}
                filepath = filepath + "log.txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }
    }
}