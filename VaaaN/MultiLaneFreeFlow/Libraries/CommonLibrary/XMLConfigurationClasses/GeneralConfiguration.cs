using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses
{
    public class GeneralConfiguration
    {
        #region Variable
        string databaseServer;
        string databaseUser;
        string databasePassword;
        int distance;
        #endregion

        #region Constructor
        public GeneralConfiguration()
        {
            this.databaseServer = String.Empty;
            this.databaseUser = String.Empty;
            this.databasePassword = String.Empty;
            this.distance = 0;
        }
        #endregion

        #region Property
        public String DatabaseServer
        {
            get { return this.databaseServer; }
            set { this.databaseServer = value; }
        }
        public String DatabaseUser
        {
            get { return this.databaseUser; }
            set { this.databaseUser = value; }
        }
        public String DatabasePassword
        {
            get { return this.databasePassword; }
            set { this.databasePassword = value; }
        }
        public int Distance
        {
            get { return this.distance; }
            set { this.distance = value; }
        }
        
        #endregion

        #region Helper
        public static GeneralConfiguration Deserialize()
        {
            GeneralConfiguration config = (VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration)VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Deserialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "GeneralConfiguration.xml");
            return config;
        }

        public static void Serialize(GeneralConfiguration config)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Serialization.Serialization.Serialize(typeof(VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration), VaaaN.MLFF.Libraries.CommonLibrary.Constants.atmsConfigDirectory + "GeneralConfiguration.xml", config);
        }
        #endregion
    }
}
