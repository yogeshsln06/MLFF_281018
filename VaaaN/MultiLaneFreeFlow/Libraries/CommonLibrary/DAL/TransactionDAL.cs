﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class TransactionDAL
    {
        static string tableName = "TBL_TRANSACTION";
        public TransactionDAL()
        {
        }

        #region Insert/Update/Delete
        public static Int64 InsertByCTP(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_INSERT_BY_CTP";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Output));
                //transaction time of CTP
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_DATETIME", DbType.DateTime, transaction.TransactionDateTime, ParameterDirection.Input));
                //ctp entry id or nfp entry id
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CT_ENTRY_ID", DbType.Int32, transaction.CrosstalkEntryId, ParameterDirection.Input));
                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, transaction.CreationDate, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }

        public static Int64 InsertByNFPFront(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_INSERT_BY_NFP_FRONT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Output));
                //transaction time of CTP
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_DATETIME", DbType.DateTime, transaction.TransactionDateTime, ParameterDirection.Input));
                //ctp entry id or nfp entry id
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_FRONT", DbType.Int32, transaction.NodefluxEntryIdFront, ParameterDirection.Input));
                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, transaction.CreationDate, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }


        public static Int64 InsertByNFPRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_INSERT_BY_NFP_REAR";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Output));
                //transaction time of CTP
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_DATETIME", DbType.DateTime, transaction.TransactionDateTime, ParameterDirection.Input));
                //ctp entry id or nfp entry id
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_REAR", DbType.Int32, transaction.NodefluxEntryIdRear, ParameterDirection.Input));
                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, transaction.CreationDate, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }

        //==================================================================

        public static Int64 UpdateByNFPFront(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, int nfpEntryIdFront)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_BY_NFP_FRONT";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_FRONT", DbType.Int32, nfpEntryIdFront, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }


        public static Int64 UpdateByNFPRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, int nfpEntryIdRear)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_BY_NFP_REAR";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Input));
                
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_REAR", DbType.Int32, nfpEntryIdRear, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }

        //==================================================================

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_tms_id", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_plaza_id", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_lane_id", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transaction_id", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_transaction_DateTime", DbType.DateTime, transaction.TransactionDateTime, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ct_entry_id", DbType.Int32, transaction.CrosstalkEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nf_entry_id_front", DbType.Int32, transaction.NodefluxEntryIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nf_entry_id_rear", DbType.Int32, transaction.NodefluxEntryIdRear, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_balance_updated", DbType.Int32, transaction.IsBalanceUpdated, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_transfered", DbType.Int32, transaction.IsTransfered, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_violation", DbType.Int32, transaction.IsViolation, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audit_status", DbType.Int32, transaction.AuditStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_auditor_id", DbType.Int32, transaction.AuditorId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audit_datetime", DbType.DateTime, transaction.AuditDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audited_vehicle_class_id", DbType.Int32, transaction.AuditedVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audited_vrn", DbType.String, transaction.AuditedVRN, ParameterDirection.Input, 20));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, transaction.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, transaction.ModificationDate, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateAuditSection(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_AUDIT_SECTION";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDIT_STATUS", DbType.Int32, transaction.AuditStatus, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITOR_ID", DbType.Int32, transaction.AuditorId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDIT_DATE", DbType.DateTime, transaction.AuditDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VEHICLE_CLASS_ID", DbType.Int32, transaction.AuditedVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VRN", DbType.String, transaction.AuditedVRN, ParameterDirection.Input, 20));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_DELETE";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Methods
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetAll()
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETALL";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                trans = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trans;
        }

        #region for CSV files
        public static StringBuilder GetNormalTransactions(DateTime startTime, DateTime endTime)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_CSV_GETNORMALTRAN";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_TIME", DbType.DateTime, startTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_TIME", DbType.DateTime, endTime, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];

                sb = CreateCSVDataFromDataTable(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sb;
        }

        public static StringBuilder GetViolationTransactions(DateTime startTime, DateTime endTime)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_CSV_GETVIOTRAN";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_TIME", DbType.DateTime, startTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_TIME", DbType.DateTime, endTime, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];

                sb = CreateCSVViolationDataFromDataTable(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sb;
        }

        public static StringBuilder GetWithoutTagTransactions(DateTime startTime, DateTime endTime)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_CSV_GETTRANWITHOUTTAG";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_START_TIME", DbType.DateTime, startTime, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_END_TIME", DbType.DateTime, endTime, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];

                sb = CreateCSVViolationDataFromDataTable(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sb;
        }

        private static StringBuilder CreateCSVDataFromDataTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            //TIME_STAMP,ike_plate_number,FRONT_plate_number,REAR_plate_number,ike_vehicle_class,anpr_vehicle_class,LANE_ID,IKE_ID,
            //OWNER_ID,VEHICLE_OWNER,AMOUNT_CHARGED,BALANCE,SMS_NOTIFICATION_status
            // Add header line
            if (dt.Rows.Count > 0)
            {
                sb.Append("TIME_STAMP,");
                sb.Append("IKE_PLATE_NUMBER,");
                sb.Append("FRONT_PLATE_NUMBER,");
                sb.Append("REAR_PLATE_NUMBER,");
                sb.Append("IKE_VEHICLE_CLASS,");
                sb.Append("ANPR_VEHICLE_CLASS,");
                sb.Append("LANE_ID,");
                sb.Append("IKE_ID,");
                sb.Append("OWNER_ID,");
                sb.Append("VEHICLE_OWNER,");
                sb.Append("AMOUNT_CHARGED,");
                sb.Append("BALANCE,");
                sb.Append("SMS_NOTIFICATION_STATUS");
            }

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine();
                //TIME_STAMP
                if (dr["TIME_STAMP"] != DBNull.Value)
                {
                    sb.Append(Convert.ToDateTime(dr["TIME_STAMP"]).ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //EVI_VEH_NO
                if (dr["EVI_VEH_NO"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_VEH_NO"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //FRONT_VRN
                if (dr["FRONT_VRN"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_VRN"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //REAR_VRN
                if (dr["REAR_VRN"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_VRN"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //VEH_NAME_EVI
                if (dr["VEH_NAME_EVI"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["VEH_NAME_EVI"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //VEH_NAME_NODEFLUX
                if (dr["VEH_NAME_NODEFLUX"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["VEH_NAME_NODEFLUX"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //LANE_ID
                if (dr["LANE_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["LANE_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //EVI_ID
                if (dr["EVI_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //OWNER_ID
                if (dr["OWNER_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["OWNER_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //VEHICLE_OWNER
                if (dr["VEHICLE_OWNER"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["VEHICLE_OWNER"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //AMOUNT_CHARGED
                if (dr["AMOUNT_CHARGED"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["AMOUNT_CHARGED"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //BALANCE
                if (dr["BALANCE"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["BALANCE"]) + ",");
                }

                //SMS_NOTIFICATION
                if (dr["SMS_NOTIFICATION"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["SMS_NOTIFICATION"]));
                }
            }

            return sb;
        }

        private static StringBuilder CreateCSVViolationDataFromDataTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            // Add header line
            if (dt.Rows.Count > 0)
            {
                sb.Append("TIME_STAMP,");
                //sb.Append("EVI_VEH_NO,");
                sb.Append("FRONT_PLATE_NUMBER,");
                sb.Append("REAR_PLATE_NUMBER,");
                sb.Append("ANPR_VEHICLE_CLASS,");
                sb.Append("LANE_ID");
            }

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine();
                //TIME_STAMP
                if (dr["TIME_STAMP"] != DBNull.Value)
                {
                    sb.Append(Convert.ToDateTime(dr["TIME_STAMP"]).ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HOracleQuery) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                ////EVI_VEH_NO
                //if (dr["EVI_VEH_NO"] != DBNull.Value)
                //{
                //    sb.Append(Convert.ToString(dr["EVI_VEH_NO"]) + ",");
                //}
                //else
                //{
                //    sb.Append(",");
                //}

                //FRONT_VRN
                if (dr["FRONT_VRN"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_VRN"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //REAR_VRN
                if (dr["REAR_VRN"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_VRN"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //VEH_NAME_NODEFLUX
                if (dr["VEH_NAME_NODEFLUX"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["VEH_NAME_NODEFLUX"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //LANE_ID
                if (dr["LANE_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["LANE_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }
            }

            return sb;
        }
        #endregion

        public static DataTable Transaction_LiveData()
        {
            DataTable dt;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_LIVEDATA";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static DataTable Transaction_GetById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            DataTable dt;
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETBYID";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// From the nodeflux cameraId get the lane id, plazaId is already hardcoded. Together with nodeflux vrn find out the transactions in the transaction table where transaction time lies between
        /// nodeflux timestamp + 30 sec and nodeflux timestamp - 30 sec ORIGINAL
        /// </summary>
        /// <param name="nfp"></param>
        /// <returns></returns>
        //public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetCorrespondingTransaction(VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp)
        //{
        //    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
        //    try
        //    {
        //        //Stored procedure must have cur_out parameter.
        //        //There is no need to add ref cursor for oracle in code.
        //        string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GET_CORRESPONDING";
        //        DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, nfp.TMSId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, nfp.GantryId, ParameterDirection.Input));

        //        //conversion of nfp.TimeStamp to DateTime
        //        DateTime nfpDateTime = Convert.ToDateTime(nfp.TimeStamp); //<==== nfp timestamp may be wrong in JSON packet CJS
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_TIMESTAMP", DbType.DateTime, nfpDateTime, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_VRN", DbType.String, nfp.PlateNumber, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_LANE_ID", DbType.Int32, nfp.LaneId, ParameterDirection.Input));
        //        DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
        //        DataTable dt = ds.Tables[tableName];
        //        trans = ConvertDataTableToCollection(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return trans;
        //}

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetCorrespondingTransactionInNodeFlux(Int32 tmsId, Int32 plazaId, DateTime timestamp, string ctpVrn)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GET_CORR_IN_NF";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plazaId, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TIMESTAMP", DbType.DateTime, timestamp, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VRN", DbType.String, ctpVrn, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                trans = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trans;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetCorrespondingTransactionInCossTalk(Int32 tmsId, Int32 plazaId, DateTime timestamp, string nfpVrn)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GET_CORR_IN_CT"; //sp to write
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plazaId, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TIMESTAMP", DbType.DateTime, timestamp, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VRN", DbType.String, nfpVrn, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                trans = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trans;
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetCorrespondingTransactionInAll(Int32 tmsId, Int32 plazaId, DateTime timestamp, string nfpVrn)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GET_CORR_IN_ALL"; //sp to write
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, tmsId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, plazaId, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TIMESTAMP", DbType.DateTime, timestamp, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VRN", DbType.String, nfpVrn, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                trans = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trans;
        }

        public static void UpdateCrossTalkSection(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ctpEntryId)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_CTP";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
                //update part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CT_ENTRY_ID", DbType.Int32, ctpEntryId, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateNodefluxSectionFront(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ntpEntryIdFront)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_NF_FRONT";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
                //update part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_FRONT", DbType.Int32, ntpEntryIdFront, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateNodefluxSectionRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ntpEntryIdRear)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_NF_REAR";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
                //update part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_REAR", DbType.Int32, ntpEntryIdRear, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MarkAsViolation(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_MARK_AS_VIOLATION";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));


                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MarkAsBalanceUpdated(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_MARK_AS_BAL_UPD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));


                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetFilteredRecords(string filter)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETFILTERED";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FILTER", DbType.String, filter, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                DataTable dt = ds.Tables[tableName];
                trans = ConvertDataTableToCollection(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trans;
        }


        public static DataTable GetDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETFILTERED";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FILTER", DbType.String, filter, ParameterDirection.Input, 2000));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static CBE.TransactionCollection FilteredTransactions(string filter)
        {
            DataTable dt = new DataTable();
            CBE.TransactionCollection transactions = new CBE.TransactionCollection();

            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETFILTERED";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_FILTER", DbType.String, filter, ParameterDirection.Input));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
                transactions = ConvertDataTableToCollection(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactions;
        }

        #endregion

        #region Helper Methods
        private static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection ConvertDataTableToCollection(DataTable dt)
        {
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection transactions = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE();

                    if (dt.Rows[i]["TMS_ID"] != DBNull.Value)
                        transaction.TMSId = Convert.ToInt32(dt.Rows[i]["TMS_ID"]);

                    if (dt.Rows[i]["PLAZA_ID"] != DBNull.Value)
                        transaction.PlazaId = Convert.ToInt32(dt.Rows[i]["PLAZA_ID"]);

                    if (dt.Rows[i]["LANE_ID"] != DBNull.Value)
                        transaction.LaneId = Convert.ToInt32(dt.Rows[i]["LANE_ID"]);

                    if (dt.Rows[i]["TRANSACTION_ID"] != DBNull.Value)
                        transaction.TransactionId = Convert.ToInt32(dt.Rows[i]["TRANSACTION_ID"]);

                    if (dt.Rows[i]["TRANSACTION_DATETIME"] != DBNull.Value)
                        transaction.TransactionDateTime = Convert.ToDateTime(dt.Rows[i]["TRANSACTION_DATETIME"]);

                    //if (dt.Rows[i]["CROSSTALK_TAG_ID"] != DBNull.Value)
                    //    transaction.CrosstalkTagId = Convert.ToString(dt.Rows[i]["CROSSTALK_TAG_ID"]);

                    //if (dt.Rows[i]["CROSSTALK_VEHICLE_CLASS_ID"] != DBNull.Value)
                    //    transaction.CrosstalkVehicleClassId = Convert.ToString(dt.Rows[i]["CROSSTALK_VEHICLE_CLASS_ID"]);

                    //if (dt.Rows[i]["CROSSTALK_VRN"] != DBNull.Value)
                    //    transaction.CrosstalkVRN = Convert.ToString(dt.Rows[i]["CROSSTALK_VRN"]);

                    //if (dt.Rows[i]["CROSSTALK_TIMESTAMP"] != DBNull.Value)
                    //    transaction.CrosstalkTimestamp = Convert.ToString(dt.Rows[i]["CROSSTALK_TIMESTAMP"]);

                    //if (dt.Rows[i]["NODEFLUX_VRN_FRONT"] != DBNull.Value)
                    //    transaction.NodefluxVRNFront = Convert.ToString(dt.Rows[i]["NODEFLUX_VRN_FRONT"]);

                    //if (dt.Rows[i]["NODEFLUX_VEHICLE_CLASS_ID_FRONT"] != DBNull.Value)
                    //    transaction.NodefluxVehicleClassIdFront = Convert.ToString(dt.Rows[i]["NODEFLUX_VEHICLE_CLASS_ID_FRONT"]);

                    //if (dt.Rows[i]["NODEFLUX_TIMESTAMP_FRONT"] != DBNull.Value)
                    //    transaction.NodefluxTimestampFront = Convert.ToString(dt.Rows[i]["NODEFLUX_TIMESTAMP_FRONT"]);

                    if (dt.Rows[i]["CT_ENTRY_ID"] != DBNull.Value)
                        transaction.CrosstalkEntryId = Convert.ToInt32(dt.Rows[i]["CT_ENTRY_ID"]);

                    if (dt.Rows[i]["NF_ENTRY_ID_FRONT"] != DBNull.Value)
                        transaction.NodefluxEntryIdFront = Convert.ToInt32(dt.Rows[i]["NF_ENTRY_ID_FRONT"]);

                    if (dt.Rows[i]["NF_ENTRY_ID_REAR"] != DBNull.Value)
                        transaction.NodefluxEntryIdRear = Convert.ToInt32(dt.Rows[i]["NF_ENTRY_ID_REAR"]);


                    if (dt.Rows[i]["IS_BALANCE_UPDATED"] != DBNull.Value)
                        transaction.IsBalanceUpdated = Convert.ToInt32(dt.Rows[i]["IS_BALANCE_UPDATED"]);

                    if (dt.Rows[i]["IS_TRANSFERED"] != DBNull.Value)
                        transaction.IsTransfered = Convert.ToInt32(dt.Rows[i]["IS_TRANSFERED"]);

                    if (dt.Rows[i]["IS_VIOLATION"] != DBNull.Value)
                        transaction.IsViolation = Convert.ToInt32(dt.Rows[i]["IS_VIOLATION"]);

                    if (dt.Rows[i]["MODIFIER_ID"] != DBNull.Value)
                        transaction.ModifierId = Convert.ToInt32(dt.Rows[i]["MODIFIER_ID"]);

                    if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                        transaction.CreationDate = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"]);

                    if (dt.Rows[i]["MODIFICATION_DATE"] != DBNull.Value)
                        transaction.ModificationDate = Convert.ToDateTime(dt.Rows[i]["MODIFICATION_DATE"]);


                    if (dt.Rows[i]["AUDIT_STATUS"] != DBNull.Value)
                        transaction.AuditStatus = Convert.ToInt32(dt.Rows[i]["AUDIT_STATUS"]);

                    if (dt.Rows[i]["AUDITOR_ID"] != DBNull.Value)
                        transaction.AuditorId = Convert.ToInt32(dt.Rows[i]["AUDITOR_ID"]);

                    if (dt.Rows[i]["AUDIT_DATE"] != DBNull.Value)
                        transaction.AuditDate = Convert.ToDateTime(dt.Rows[i]["AUDIT_DATE"]);

                    if (dt.Rows[i]["AUDITED_VEHICLE_CLASS_ID"] != DBNull.Value)
                        transaction.AuditedVehicleClassId = Convert.ToInt32(dt.Rows[i]["AUDITED_VEHICLE_CLASS_ID"]);

                    if (dt.Rows[i]["AUDITED_VRN"] != DBNull.Value)
                        transaction.AuditedVRN = Convert.ToString(dt.Rows[i]["AUDITED_VRN"]);


                    transactions.Add(transaction);
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
