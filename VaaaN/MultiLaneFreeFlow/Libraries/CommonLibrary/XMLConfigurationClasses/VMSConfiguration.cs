using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses
{

    public class VMSConfiguration
    { 
        #region Variable

        string vmsMaxCharacterLength;

        #endregion

        #region Constructor

        public VMSConfiguration()
        {
            this.vmsMaxCharacterLength = String.Empty;
        }

        #endregion

        #region Property

        public String VMSMaxCharacterLength
        {
            get { return this.vmsMaxCharacterLength; }
            set { this.vmsMaxCharacterLength = value; }
        }

        #endregion

        #region Helper
        public static VMSConfiguration Deserialize()
        {
            VMSConfiguration config = (VMSConfiguration)Serialization.Serialization.Deserialize(typeof(VMSConfiguration), Constants.atmsConfigDirectory + "VMSConfiguration.xml");

            return config;
        }

        public static void Serialize(VMSConfiguration config)
        {
            Serialization.Serialization.Serialize(typeof(XMLConfigurationClasses.VMSConfiguration), Constants.atmsConfigDirectory + "VMSConfiguration.xml", config);
        }
        #endregion
    }
}
