using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses
{
    public class SMSFileConfiguration
    {
        #region Variable
        string topup;
        string saldo;
        string afterdeduction;
        string notification;
        string nocustomerfound;
        string invalid;
        #endregion

        #region Constructor
        public SMSFileConfiguration()
        {
            this.topup = String.Empty;
            this.saldo = String.Empty;
            this.afterdeduction = String.Empty;
            this.notification = String.Empty;
            this.nocustomerfound = String.Empty;
            this.invalid = String.Empty;
        }
        #endregion

        #region Property
        public String TOPUP
        {
            get { return this.topup; }
            set { this.topup = value; }
        }
        public String SALDO
        {
            get { return this.saldo; }
            set { this.saldo = value; }
        }
        public String AFTERDEDUCTION
        {
            get { return this.afterdeduction; }
            set { this.afterdeduction = value; }
        }
        public String NOTIFICATION
        {
            get { return this.notification; }
            set { this.notification = value; }
        }
        public String NOCUSTOMERFOUND
        {
            get { return this.nocustomerfound; }
            set { this.nocustomerfound = value; }
        }
        public String INVALID
        {
            get { return this.invalid; }
            set { this.invalid = value; }
        }

        #endregion

        #region Helper
        public static SMSFileConfiguration Deserialize()
        {
            SMSFileConfiguration config = (VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration)VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Deserialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "SMSFileConfiguration.xml");
            return config;
        }

        public static void Serialize(GeneralConfiguration config)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Serialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "SMSFileConfiguration.xml", config);
        }
        #endregion
    }
}
