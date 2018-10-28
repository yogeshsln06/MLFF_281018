using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses
{
    public class ECBConfiguration
    {
        #region Variable
        string voiceLoggerCOMPort;
        string ecbStatusCheckLineNumber;
        string ecbType;
        #endregion

        #region Constructor
        public ECBConfiguration()
        {
            this.voiceLoggerCOMPort = String.Empty;
            this.ecbStatusCheckLineNumber = "3";
            this.ecbType = "OFC";
        }
        #endregion

        #region Property
        public String VoiceLoggerCOMPort
        {
            get { return this.voiceLoggerCOMPort; }
            set { this.voiceLoggerCOMPort = value; }
        }
        public String ECBStatusCheckLineNumber
        {
            get { return this.ecbStatusCheckLineNumber; }
            set { this.ecbStatusCheckLineNumber = value; }
        }
        public String ECBType
        {
            get { return this.ecbType; }
            set { this.ecbType = value; }
        }
        #endregion

        #region Helper
        public static ECBConfiguration Deserialize()
        {
            ECBConfiguration config = (VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.ECBConfiguration)VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Deserialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.ECBConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "ECBConfiguration.xml");

            return config;
        }

        public static void Serialize(ECBConfiguration config)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Serialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.ECBConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "ECBConfiguration.xml", config);
        }
        #endregion
    }
}
