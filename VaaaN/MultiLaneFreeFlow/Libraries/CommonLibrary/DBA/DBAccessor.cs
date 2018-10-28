using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DBA
{
    public class DBAccessor
    {
        /// <summary>
        /// Execute the non query like insert, delete and update.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Numbers of rows affected.</returns>
        public static int ExecuteNonQuery(DbCommand command)
        {
            int rowsAffected = 0;

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                rowsAffected = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.WriteGeneralLog("Failed to execute non query." + ex.Message);
                throw ex;
            }

            return rowsAffected;
        }

        /// <summary>
        /// Get stored procedure command.
        /// </summary>
        /// <param name="spName"> Name of the stored procedure.</param>
        /// <returns>Returns the Dbcommand</returns>
        public static DbCommand GetStoredProcCommand(string spName)
        {
            DbCommand dbCommand = null;

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;

                dbCommand = db.GetStoredProcCommand(spName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dbCommand;
        }

        /// <summary>
        /// Create sql string command.
        /// </summary>
        /// <param name="query">sql query.</param>
        /// <returns>Returns the Db command.</returns>
        public static DbCommand GetSqlStringCommand(string query)
        {
            DbCommand dbCommand = null;

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                dbCommand = db.GetSqlStringCommand(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dbCommand;
        }

        /// <summary>
        /// Execute a non query returning resultset as a datatable
        /// </summary>
        /// <param name="command"></param>
        /// <returns>DataTable</returns>
        public static DataSet LoadDataSet(DbCommand command, string tableName)
        {
            DataSet ds = new DataSet();
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                db.LoadDataSet(command, ds, tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public static void LoadDataSet(DbCommand command, string tableName, ref DataSet ds)
        {
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                db.LoadDataSet(command, ds, tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns an object by executing a scalar function
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Object</returns>
        public static Object ExecuteScalar(DbCommand command)
        {
            object obj = new object();

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                obj = db.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        /// <summary>
        /// Returns an object by executing a scalar function
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns>Object</returns>
        public static Object ExecuteScalarString(string commandText)
        {
            object obj = new object();

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
                DbCommand command = db.GetSqlStringCommand(commandText);
                obj = db.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static DbParameter CreateDbParameter(ref DbCommand cmd, String paramName, DbType dbType, object value, ParameterDirection paramDirection)
        {
            DbParameter dbParam = cmd.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.DbType = dbType;
            dbParam.Value = value;
            dbParam.Direction = paramDirection;

            return dbParam;
        }
        public static DbParameter CreateDbParameter(ref DbCommand cmd, String paramName, DbType dbType, object value, ParameterDirection paramDirection, int size)
        {
            DbParameter dbParam = cmd.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.DbType = dbType;
            dbParam.Value = value;
            dbParam.Size = size;
            dbParam.Direction = paramDirection;

            return dbParam;
        }

        public static DbConnection CreateConnection()
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DBSelector.Instance.GetDataBase;
            return db.CreateConnection();
        }
    }
}
