using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DBA
{
    public class DBSelector
    {
        private static DBSelector instance;


        //Database object
        private Microsoft.Practices.EnterpriseLibrary.Data.Database database;

        private DBSelector()
        {
            SelectDataBase();
        }

        /// <summary>
        /// Get property to retrieve the current database.
        /// </summary>
        public Microsoft.Practices.EnterpriseLibrary.Data.Database GetDataBase
        {
            get { return this.database; }
        }

        public static DBSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBSelector();
                }
                return instance;
            }
        }

        /// <summary>
        /// Create new database.
        /// 1. Oracle
        /// 2. Sql Server
        /// 3. DB2
        /// </summary>
        /// <returns></returns>
        public void SelectDataBase()
        {
            try
            {
                string connString = string.Empty;

                #region Oracle

                XMLConfigurationClasses.GeneralConfiguration generalConfig = XMLConfigurationClasses.GeneralConfiguration.Deserialize();

                string plainText = Cryptography.Encryption.Decrypt(
                       generalConfig.DatabasePassword,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.passPhrase,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.saltValue,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.hashAlgorithm,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.passwordIterations,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.initVector,
                       VaaaN.MLFF.Libraries.CommonLibrary.Constants.keySize);

                connString = "Data Source = " + generalConfig.DatabaseServer + ";User ID=" + generalConfig.DatabaseUser + "; Password = " + plainText + ";Unicode=True";
                this.database = new Microsoft.Practices.EnterpriseLibrary.Data.Oracle.OracleDatabase(connString);

                #endregion

                #region SQL Server

                //connString = @"Data Source=192.168.1.51\ATMS;Initial Catalog=vaaan-atms;User ID=vaaan; password=vaaan;";
                //VaaaN.ATMS.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration generalConfig = VaaaN.ATMS.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration.Deserialize();

                //string plainText = VaaaN.ATMS.Libraries.CommonLibrary.Cryptography.Encryption.Decrypt(
                //       generalConfig.DatabasePassword,
                //       VaaaN.ATMS.Libraries.CommonLibrary.Constants.passPhrase,
                //      VaaaN.ATMS.Libraries.CommonLibrary.Constants.saltValue,
                //       VaaaN.ATMS.Libraries.CommonLibrary.Constants.hashAlgorithm,
                //       VaaaN.ATMS.Libraries.CommonLibrary.Constants.passwordIterations,
                //       VaaaN.ATMS.Libraries.CommonLibrary.Constants.initVector,
                //       VaaaN.ATMS.Libraries.CommonLibrary.Constants.keySize);

                //connString = @"Data Source=" + generalConfig.DatabaseServer + ";Initial Catalog=" + generalConfig.DatabaseName + ";User ID=" + generalConfig.DatabaseUser + "; password=" + plainText + ";";
                //this.database = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(connString);

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
