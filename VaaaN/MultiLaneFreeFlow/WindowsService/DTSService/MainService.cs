using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DTSService
{
    public partial class MainService : ServiceBase
    {
        #region Variables

        System.Timers.Timer timerGenerateTransactionCSV;
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.CSVFileConfiguration csvFileConfig;
        #endregion

        #region Constructor
        public MainService()
        {
            InitializeComponent();
            //OnStart(new string[] { "fdfd" });
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

        #region Events
        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage("Starting DTS service.");

                LogMessage("Starting threadCreateTransactionCSVFile...");

                try
                {
                    LogMessage("Reading CSV configuration file.");
                    csvFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.CSVFileConfiguration.Deserialize();
                    LogMessage("Configuration file read successfully.");
                }
                catch (Exception ex)
                {
                    LogMessage("Failed to read csv configuration file." + ex.Message);
                }

                timerGenerateTransactionCSV = new System.Timers.Timer();
                timerGenerateTransactionCSV.Elapsed += TimerGenerateCSV_Elapsed;
                timerGenerateTransactionCSV.Interval = 1000;// First time will be called after 1 second and later on after every 10 minutes
                timerGenerateTransactionCSV.Enabled = true; ;
                timerGenerateTransactionCSV.Start();

                LogMessage("threadCreateTransactionCSVFile started successfully.");

                LogMessage("DTS service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start DTS service." + ex.Message);
            }
        }

        private void TimerGenerateCSV_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerGenerateTransactionCSV.Interval = 10 * 60 * 1000;// Run after every 10 minutes
                timerGenerateTransactionCSV.Enabled = false;
                GenerateNormalTransactionCSV();
                GenerateViolationTransactionCSV();
                SendTransactionCSVFile();
            }
            catch (Exception ex)
            {
                LogMessage("Failed to process TimerGenerateCSV_Elapsed. " + ex.Message);
            }
            finally
            {
                timerGenerateTransactionCSV.Enabled = true;
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogMessage("Stopping timerGenerateCSV.");
                timerGenerateTransactionCSV.Stop();
                timerGenerateTransactionCSV = null;

                LogMessage("timerGenerateCSV stopped successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to stop DTS service." + ex.Message);
            }
            finally
            {
                base.OnStop();
            }
        }
        #endregion

        #region Helper Methods

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.DTSService);
        }

        string nameOfBidder = "BaliTower";
        string csvDirectory = "C:/MLFF/CSV/";

        #region Normal transaction
        private void GenerateNormalTransactionCSV()
        {
            try
            {
                // Check previous day
                GenerateFirstNormalTransactionCSV(DateTime.Now.AddDays(-1));
                GenerateSecondNormalTransactionCSV(DateTime.Now.AddDays(-1));
                GenerateThirdNormalTransactionCSV(DateTime.Now.AddDays(-1));

                // Check current day
                GenerateFirstNormalTransactionCSV(DateTime.Now);
                GenerateSecondNormalTransactionCSV(DateTime.Now);
                GenerateThirdNormalTransactionCSV(DateTime.Now);
            }
            catch (Exception ex)
            {
                LogMessage("Failed to generate normal transaction CSV file." + ex.Message);
            }
        }

        private void GenerateFirstNormalTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 0, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 7, 59, 59, 999);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    // Create file path
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE1.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE1_s.csv";

                    SaveNormalCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create normal transaction CSV file." + ex.Message);
            }
        }

        private void GenerateSecondNormalTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 8, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 15, 59, 59, 999);

                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    // Create file path
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE2.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE2_s.csv";

                    SaveNormalCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create normal transaction CSV file." + ex.Message);
            }
        }

        private void GenerateThirdNormalTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 16, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 23, 59, 59, 999);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    // Create file path
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE3.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-IKE3_s.csv";

                    SaveNormalCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create normal transaction CSV file." + ex.Message);
            }
        }

        private void SaveNormalCSVFile(DateTime startDateTime, DateTime endDateTime, string csvFilePath, string csvSentFilePath)
        {
            bool fileExist = false;

            // Create directory if not exist
            if (!Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            if (File.Exists(csvFilePath) || File.Exists(csvSentFilePath))
            {
                fileExist = true;
            }

            if (!fileExist)
            {
                // Fetch CSV data from database
                StringBuilder sb = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetNormalTransactions(startDateTime, endDateTime);

                if (sb.ToString().Length > 0)
                {
                    LogMessage("Creating normal transaction CSV file. File Path:" + csvFilePath);

                    // Save CSV File
                    using (StreamWriter sw = new StreamWriter(csvFilePath))
                    {
                        sw.Write(sb.ToString());
                    }

                    LogMessage("Normal transaction CSV file created successfully.");
                }
                else
                {
                    LogMessage("Normal transaction data not found.");
                }
            }
            else
            {
                //LogMessage("File already exists." + csvFilePath);
            }
        }
        #endregion

        #region Violation transaction
        private void GenerateViolationTransactionCSV()
        {
            try
            {
                // Check previous day
                GenerateFirstViolationTransactionCSV(DateTime.Now.AddDays(-1));
                GenerateSecondViolationTransactionCSV(DateTime.Now.AddDays(-1));
                GenerateThirdViolationTransactionCSV(DateTime.Now.AddDays(-1));

                // Check current day
                GenerateFirstViolationTransactionCSV(DateTime.Now);
                GenerateSecondViolationTransactionCSV(DateTime.Now);
                GenerateThirdViolationTransactionCSV(DateTime.Now);
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create transaction violation CSV file." + ex.Message);
            }
        }

        private void GenerateFirstViolationTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 0, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 7, 59, 59, 999);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    //BaliTower - yyyymmdd - IKE - less.csv
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE1.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE1_s.csv";

                    SaveViolationCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create first violation transaction CSV file." + ex.Message);
            }
        }

        private void GenerateSecondViolationTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 8, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 15, 59, 59, 999);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE2.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE2_s.csv";

                    SaveViolationCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create second violation transaction CSV file." + ex.Message);
            }
        }

        private void GenerateThirdViolationTransactionCSV(DateTime dateToCheck)
        {
            try
            {
                DateTime startDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 16, 0, 0, 0);
                DateTime endDateTime = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 23, 59, 59, 999);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > endDateTime)// Send data after completing the the imaginary 8 hrs shift
                {
                    string csvFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE3.csv";
                    string csvSentFilePath = csvDirectory + nameOfBidder + "-" + startDateTime.ToString("yyyyMMdd") + "-non-IKE3_s.csv";

                    SaveViolationCSVFile(startDateTime, endDateTime, csvFilePath, csvSentFilePath);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create third violation transaction CSV file." + ex.Message);
            }
        }

        private void SaveViolationCSVFile(DateTime startDateTime, DateTime endDateTime, string csvFilePath, string csvSentFilePath)
        {
            bool fileExist = false;

            // Create directory if not exist
            if (!Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            if (File.Exists(csvFilePath) || File.Exists(csvSentFilePath))
            {
                fileExist = true;
            }

            if (!fileExist)
            {
                // Fetch CSV data from database
                StringBuilder sb = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetViolationTransactions(startDateTime, endDateTime);

                if (sb.ToString().Length > 0)
                {
                    LogMessage("Creating violation transaction CSV file. File Path:" + csvFilePath);

                    // Save CSV File
                    using (StreamWriter sw = new StreamWriter(csvFilePath))
                    {
                        sw.Write(sb.ToString());
                    }

                    LogMessage("Violation transaction CSV file created successfully.");
                }
                else
                {
                    LogMessage("Violation transaction data not found.");
                }
            }
            else
            {
                //LogMessage("File already exists." + csvFilePath);
            }
        }
        #endregion

        private void SendTransactionCSVFile()
        {
            // Send transaction CSV file and mark it as sent (_s)
            try
            {
                IEnumerable<string> paths = from path in Directory.GetFiles(csvDirectory, "*.*")
                                            where !path.ToLower().Contains("_s")
                                            orderby path descending
                                            select path;

                foreach (string path in paths)
                {
                    System.IO.FileInfo fi = new FileInfo(path);

                    // Send email with CSV file attachment
                    bool isSuccess = SendEmail(fi);
                    if (isSuccess)
                    {
                        // Mark file as sent
                        LogMessage("Marking file as sent." + fi.Name);
                        File.Move(fi.FullName, fi.FullName.Replace(fi.Extension, "_s" + fi.Extension));
                        LogMessage("File marked successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send CSV file." + ex.Message);
            }
        }

        private bool SendEmail(FileInfo fiAttachment)
        {
            bool isSuccess = false;

            try
            {
                LogMessage("Sending Email of CSV file." + fiAttachment.FullName);

                // Get from configuration file- TODO
                //string toEmailId = "yogesh.kumar@vaaaninfra.com,mridul.buragohain @vaaaninfra.com,hemant.kumar@vaaaninfra.com,arun.yadav@vaaaninfra.com";
                //string fromMail = "yogesh.kumar@vaaaninfra.com";
                //string mailPassword = "milestogo@123";
                //int port = 587;
                //string hostName = "smtp.office365.com";

                string toEmailId = csvFileConfig.ToEmailId;
                string fromMail = csvFileConfig.FromEmailId;
                string mailPassword = csvFileConfig.MailPassword;
                int port = Convert.ToInt32(csvFileConfig.MailPort);
                string hostName = csvFileConfig.MailHostName;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(fromMail);
                    mailMessage.To.Add(toEmailId);

                    SmtpClient SmtpServer = new SmtpClient();
                    SmtpServer.Credentials = new System.Net.NetworkCredential(fromMail, mailPassword);
                    SmtpServer.Port = port;
                    SmtpServer.Host = hostName;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(fromMail);
                    mail.Subject = fiAttachment.Name;
                    mail.Body = "Please find the attached CSV file. This is automatically generated file. No need to reply.";
                    mail.To.Add(toEmailId);
                    mail.IsBodyHtml = false;

                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    // Attachment
                    Attachment attachFile = new Attachment(fiAttachment.FullName);
                    mail.Attachments.Add(attachFile);

                    SmtpServer.Send(mail);
                    attachFile.Dispose();//disposing the Attachment object

                    isSuccess = true;
                    LogMessage("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to send email." + ex.Message);
            }

            return isSuccess;
        }

        #endregion
    }
}
