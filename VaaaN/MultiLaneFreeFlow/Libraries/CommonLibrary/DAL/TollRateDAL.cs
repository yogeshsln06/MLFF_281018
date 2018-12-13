using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class TollRateDAL
    {
        static string tableName = "TBL_TOLL_RATE";
        public TollRateDAL()
        {
        }

        #region Insert/Update/Delete
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollRate)
        {
            int rateId = tollRate.RateId;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TOLL_RATE_INSERT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                if (rateId <= 0)
                {
                    rateId = GetNextValue();
                }

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tollRate.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, tollRate.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, tollRate.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RATE_ID", DbType.Int32, tollRate.RateId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROFILE_ID", DbType.Int32, tollRate.ProfileId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, tollRate.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_TYPE_ID", DbType.Int32, tollRate.LaneTypeId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_TIME", DbType.String, tollRate.StartTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_TIME", DbType.String, tollRate.EndTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AMOUNT", DbType.Decimal, tollRate.Amount, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DESCRIPTION", DbType.String, tollRate.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, tollRate.CreationDate, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rateId;
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollRate)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TOLL_RATE_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tollRate.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, tollRate.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSFER_STATUS", DbType.Int32, tollRate.TransferStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_RATE_ID", DbType.Int32, tollRate.RateId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PROFILE_ID", DbType.Int32, tollRate.ProfileId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, tollRate.VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_TYPE_ID", DbType.Int32, tollRate.LaneTypeId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_TIME", DbType.String, tollRate.StartTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_TIME", DbType.String, tollRate.EndTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AMOUNT", DbType.Decimal, tollRate.Amount, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_DESCRIPTION", DbType.String, tollRate.Description, ParameterDirection.Input, 100));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFIER_ID", DbType.Int32, tollRate.ModifiedBy, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_MODIFICATION_DATE", DbType.DateTime, tollRate.ModificationDate, ParameterDirection.Input, 100));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TOLL_RATE_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                tollRates = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tollRates;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE GetSpecificTollRate(int TMSId, int PlazaId, int LaneTypeId, int VehicleClassId, DateTime TransactionTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollRate = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TOLL_RATE_GET_SPECIFIC_TOLL_RATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_TYPE_ID", DbType.Int32, LaneTypeId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLE_CLASS_ID", DbType.Int32, VehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_TIME", DbType.DateTime, TransactionTime, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                tollRate = ConvertDataTableToCollection(dt)[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tollRate;
        }

        /// <summary>
        /// Get next value for the primary key
        /// </summary>
        /// <returns></returns>
        private static int GetNextValue()
        {
            //next value will be 1 if there is no row in the datatable.
            int nextValue = 1;

            try
            {
                //Get object collection
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection objs = GetAll();

                //Get all objects Id
                int[] sortedObjsId = new int[objs.Count];
                for (int i = 0; i < objs.Count; i++)
                {
                    sortedObjsId[i] = objs[i].RateId;
                }

                //Sort the object id
                Array.Sort(sortedObjsId);

                for (int j = 0; j < sortedObjsId.Length; j++)
                {
                    if (j + 1 < sortedObjsId.Length)
                    {
                        if (sortedObjsId[j] + 1 < sortedObjsId[j + 1])
                        {
                            nextValue = sortedObjsId[j] + 1;
                            break;
                        }
                    }
                    else
                    {
                        nextValue = sortedObjsId[sortedObjsId.Length - 1] + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nextValue;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection GetLatestTollRates(DateTime lastCollectionUpdateTime)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TOLLRATE_LATEST_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LAST_UPDATE_TIME", DbType.DateTime, lastCollectionUpdateTime, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                tollRates = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tollRates;
        }
        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRateCollection = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollRate = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        tollRate.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        tollRate.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["TRANSFER_STATUS"] != DBNull.Value)
                        tollRate.TransferStatus = Convert.ToInt32(dt.Rows[i]["TRANSFER_STATUS"]);

                    if (dt.Rows[i]["RATE_ID"] != DBNull.Value)
                        tollRate.RateId = Convert.ToInt32(dt.Rows[i]["RATE_ID"]);

                    if (dt.Rows[i]["PROFILE_ID"] != DBNull.Value)
                        tollRate.ProfileId = Convert.ToInt32(dt.Rows[i]["PROFILE_ID"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_ID"] != DBNull.Value)
                        tollRate.VehicleClassId = Convert.ToInt32(dt.Rows[i]["VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["VEHICLE_CLASS_NAME"] != DBNull.Value)
                        tollRate.VehicleClassName = Convert.ToString(dt.Rows[i]["VEHICLE_CLASS_NAME"]);

                    if (dt.Rows[i]["LANE_TYPE_ID"] != DBNull.Value)
                    {
                        tollRate.LaneTypeId = Convert.ToInt32(dt.Rows[i]["LANE_TYPE_ID"]);
                        if (tollRate.LaneTypeId > 0)
                        {
                            tollRate.LaneTypeName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LaneTypeName[tollRate.LaneTypeId - 1];
                        }
                    }

                    if (dt.Rows[i]["START_TIME"] != DBNull.Value)
                        tollRate.StartTime = Convert.ToString(dt.Rows[i]["START_TIME"]);

                    if (dt.Rows[i]["END_TIME"] != DBNull.Value)
                        tollRate.EndTime = Convert.ToString(dt.Rows[i]["END_TIME"]);

                    if (dt.Rows[i]["AMOUNT"] != DBNull.Value)
                        tollRate.Amount = Convert.ToDecimal(dt.Rows[i]["AMOUNT"]);

                    if (dt.Rows[i]["DESCRIPTION"] != DBNull.Value)
                        tollRate.Description = Convert.ToString(dt.Rows[i]["DESCRIPTION"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        tollRate.ModifiedBy = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        tollRate.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        tollRate.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);

                    tollRateCollection.Add(tollRate);
                }
                return tollRateCollection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
