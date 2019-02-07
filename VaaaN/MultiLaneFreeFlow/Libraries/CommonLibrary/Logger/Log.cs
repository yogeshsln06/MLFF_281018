#region Copyright message
/*
© copyright 2011 VaaaN Infra (P) Ltd. All rights reserved.

This file contains Proprietary information of VaaaN Infra (P) Ltd.
and should be treated as proprietary and confidential.

The use, copying, disclosure or modification of the programs and/or 
source code is prohibited unless otherwise provided for in the license 
or confidential agreements.

========================================================
Author           :  VaaaN Infra                  
Company          :  VaaaN Infra     
Date of Creation :                              
========================================================
*/
#endregion

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Logger
{
    /// <summary>
    /// This class is used to log the errors in the application.
    /// </summary>
    public class Log
    {
        #region variable
        #endregion

        /// <summary>
        /// Module log folder
        /// </summary>
        public enum ErrorLogModule
        {
            MCM,
            VMS,
            LDS,
            MainDashboard,
            GeneralLog,
            LoginModule,
            Report,
            WebDashboard,
            WebAPI,
            MobileWebAPI,
            InboundSMS,
            MLFFWeb,
            POSController,
            DTSService,
            OutboundSMS,
            ServiceMonitor
        }

        /// <summary>
        /// Constructor
        /// </summary>
        static Log()
        {
        }

        public enum VerbosityLevel
        {
            Normal = 1,
            Warning,
            Information,
            Error,
            Detailed
        }

        public static string[] VerbosityLevelName = new string[]
        {
            "Normal",//0
            "Warning",
            "Information",
            "Error",
            "Detailed"
        };

        /// <summary>
        /// Write message to log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logModule"></param>
        public static void Write(string message, ErrorLogModule logModule)
        {
            string path = string.Empty;
            DateTime dt = DateTime.Now;

            try
            {
                //Create folder name
                switch (logModule)
                {
                    case ErrorLogModule.MCM:
                        {
                            path = CreateDirectory("MCM//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }

                    case ErrorLogModule.VMS:
                        {
                            path = CreateDirectory("Services/VMS//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.LDS:
                        {
                            path = CreateDirectory("Services/LDS//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.MainDashboard:
                        {
                            path = CreateDirectory("DashBoard//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.LoginModule:
                        {
                            path = CreateDirectory("Login//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }

                    case ErrorLogModule.Report:
                        {
                            path = CreateDirectory("WebReport//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.WebDashboard:
                        {
                            path = CreateDirectory("WebDashboard//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.WebAPI:
                        {
                            path = CreateDirectory("WebAPI//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.MobileWebAPI:
                        {
                            path = CreateDirectory("MobileWebAPI//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.InboundSMS:
                        {
                            path = CreateDirectory("SMS/Inbound//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.OutboundSMS:
                        {
                            path = CreateDirectory("SMS/Outbound//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.MLFFWeb:
                        {
                            path = CreateDirectory("MLFFWeb//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.GeneralLog:
                        {
                            path = CreateDirectory("GeneralLog//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.DTSService:
                        {
                            path = CreateDirectory("Services/DTS//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                    case ErrorLogModule.ServiceMonitor:
                        {
                            path = CreateDirectory("Services/ServiceMonitor//") + dt.ToString("yyyy-MM-dd") + ".log";
                            break;
                        }
                }

                //Write log to file.
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            writer.WriteLine(GetFormattedMessage(message));
                            break;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    path = CreateDirectory("GENERAL//") + dt.ToString("yyyy-MM-dd") + ".log";
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(GetFormattedMessage("Exception in log file writer. Original message was " + message + Environment.NewLine + ex.ToString()));
                    }
                }
                catch
                {
                    //Ignore exception
                }
            }
        }

        public static string CreateDirectory(string directoryName)
        {
            string directory = Constants.driveLetter + ":\\MLFF\\log\\" + directoryName;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        private static string GetFormattedMessage(string message)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(DateTime.Now.ToString("hh:mm:ss.FFFF tt") + ": " + message); //time should not be here. this should be the time when the event happeded. CJS.

            return sb.ToString();
        }

        public static void WriteGeneralLog(String msg)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(msg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.GeneralLog);
                    break;
                }
                catch
                {
                    //Ignore
                    Thread.Sleep(50);
                }
            }
        }
    }
}
