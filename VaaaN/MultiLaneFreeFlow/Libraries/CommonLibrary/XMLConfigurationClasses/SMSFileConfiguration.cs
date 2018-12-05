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
        #endregion

        #region Constructor
        public SMSFileConfiguration()
        {
            this.topup = String.Empty;
            this.saldo = String.Empty;
            this.afterdeduction = String.Empty;
            this.notification = String.Empty;
        }
        #endregion

        #region Property
        public String Topup
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
