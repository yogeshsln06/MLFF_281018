using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses
{
    public class CSVFileConfiguration
    {
        #region Variable
        string toEmailId = String.Empty;
        string fromEmailId = String.Empty;
        string mailPassword = String.Empty;
        string mailPort = String.Empty;
        string mailHostName = String.Empty;

        #endregion

        #region Constructor
        public CSVFileConfiguration()
        {
            this.toEmailId = String.Empty;
            this.fromEmailId = String.Empty;
            this.mailPassword = String.Empty;
            this.mailPort = String.Empty;
            this.mailHostName = String.Empty;
        }
        #endregion

        #region Property
        public String ToEmailId
        {
            get { return this.toEmailId; }
            set { this.toEmailId = value; }
        }
        public String FromEmailId
        {
            get { return this.fromEmailId; }
            set { this.fromEmailId = value; }
        }
        public String MailPassword
        {
            get { return this.mailPassword; }
            set { this.mailPassword = value; }
        }
        public String MailPort
        {
            get { return this.mailPort; }
            set { this.mailPort = value; }
        }
        public String MailHostName
        {
            get { return this.mailHostName; }
            set { this.mailHostName = value; }
        }
        #endregion

        #region Helper
        public static CSVFileConfiguration Deserialize()
        {
            CSVFileConfiguration config = (VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.CSVFileConfiguration)VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Deserialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.CSVFileConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "CSVFileConfiguration.xml");
            return config;
        }

        public static void Serialize(CSVFileConfiguration config)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Serialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.CSVFileConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "CSVFileConfiguration.xml", config);
        }
        #endregion
    }
}
