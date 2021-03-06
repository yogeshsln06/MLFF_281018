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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CT_ENTRY_ID", DbType.Int32, transaction.CrosstalkEntryIdFront, ParameterDirection.Input));
                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, transaction.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_REGISTERED", DbType.Int32, transaction.IsRegistered, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);

                transactionId = Convert.ToInt64(command.Parameters["P_TRANSACTION_ID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transactionId;
        }

        public static Int64 InsertByCTPRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            Int64 transactionId = 0;
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_INSERT_BY_CTP_REAR";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int64, transaction.TransactionId, ParameterDirection.Output));
                //transaction time of CTP
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_DATETIME", DbType.DateTime, transaction.TransactionDateTime, ParameterDirection.Input));
                //ctp entry id or nfp entry id
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CT_ENTRY_ID", DbType.Int32, transaction.CrosstalkEntryIdFront, ParameterDirection.Input));
                //common parts
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CREATION_DATE", DbType.DateTime, transaction.CreationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_REGISTERED", DbType.Int32, transaction.IsRegistered, ParameterDirection.Input));

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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_REGISTERED", DbType.Int32, transaction.IsRegistered, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Int32, transaction.VehicleSpeed, ParameterDirection.Input));

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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_REGISTERED", DbType.Int32, transaction.IsRegistered, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Int32, transaction.VehicleSpeed, ParameterDirection.Input));
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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Double, transaction.VehicleSpeed, ParameterDirection.Input));
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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Double, transaction.VehicleSpeed, ParameterDirection.Input));
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

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_ct_entry_id", DbType.Int32, transaction.CrosstalkEntryIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nf_entry_id_front", DbType.Int32, transaction.NodefluxEntryIdFront, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_nf_entry_id_rear", DbType.Int32, transaction.NodefluxEntryIdRear, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_balance_updated", DbType.Int32, transaction.IsBalanceUpdated, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_transfered", DbType.Int32, transaction.IsTransfered, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_is_violation", DbType.Int32, transaction.IsViolation, ParameterDirection.Input));

                ////command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audit_status", DbType.Int32, transaction.AuditStatus, ParameterDirection.Input));
                ////command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_auditor_id", DbType.Int32, transaction.AuditorId, ParameterDirection.Input));
                ////command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audit_datetime", DbType.DateTime, transaction.AuditDate, ParameterDirection.Input));
                ////command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audited_vehicle_class_id", DbType.Int32, transaction.AuditedVehicleClassId, ParameterDirection.Input));
                ////command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_audited_vrn", DbType.String, transaction.AuditedVRN, ParameterDirection.Input, 20));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modifier_id", DbType.Int32, transaction.ModifierId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "p_modification_date", DbType.DateTime, transaction.ModificationDate, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IS_REGISTERED", DbType.Int32, transaction.IsRegistered, ParameterDirection.Input));

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
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_TRANSCATION_STATUS", DbType.Int16, transaction.AuditedTranscationStatus, ParameterDirection.Input, 20));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void JoinAuditTransaction(long parentTransactionId, long childOneTransactionId, long childTwoTransactionId, string auditedVRN, int auditedVehicleClassId, int auditorID)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "JOIN_AUDIT_TRANSACTIONS";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PARENT_TRANSACTION_ID", DbType.Int64, parentTransactionId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CHILD_1_TRANSACTION_ID", DbType.Int64, childOneTransactionId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CHILD_2_TRANSACTION_ID", DbType.Int64, childTwoTransactionId, ParameterDirection.Input, 50));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VRN", DbType.String, auditedVRN, ParameterDirection.Input));

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VEHICLE_CLASS_ID", DbType.Int32, auditedVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITOR_ID", DbType.Int32, auditorID, ParameterDirection.Input));

                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MeargedAuditTransactionOld(Int32 ParentId, Int32 IkeEntryId, Int32 ANPRFrontEntryId, Int32 ANPRRearEntryId, string AuditedVRN, int AuditedVehicleClassId, int AuditorID, int TranscationStatus)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "MEARGED_AUDIT_TRANSACTIONS";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PARENT_TRANSACTION_ID", DbType.Int32, ParentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_IKEENTRYID", DbType.Int32, IkeEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPRFRONTENTRYID", DbType.Int32, ANPRFrontEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ANPRREARENTRYID", DbType.Int32, ANPRRearEntryId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VRN", DbType.String, AuditedVRN, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITED_VEHICLE_CLASS_ID", DbType.Int32, AuditedVehicleClassId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_AUDITOR_ID", DbType.Int32, AuditorID, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANS_STATUS", DbType.Int32, TranscationStatus, ParameterDirection.Input));
                VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MeargedAuditTransaction(Int32 ParentId, Int32 ChildId, int TranscationStatus)
        {
            try
            {
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "MEARGED_AUDIT_TRANSACTIONS";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);

                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PARENT_TRANSACTION_ID", DbType.Int32, ParentId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_CHILD_TRANSACTION_ID", DbType.Int32, ChildId, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANS_STATUS", DbType.Int32, TranscationStatus, ParameterDirection.Input));
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

            // Add header line
            if (dt.Rows.Count > 0)
            {
                sb.Append("Transaction ID,");
                sb.Append("Timestamp,");
                sb.Append("VRN (Front EVI),");
                sb.Append("VRN (Rear EVI),");
                sb.Append("VRN (Front ANPR),");
                sb.Append("VRN (Rear ANPR),");
                sb.Append("Class (Front EVI),");
                sb.Append("Class (Rear EVI),");
                sb.Append("Class (Front ANPR),");
                sb.Append("Class (Rear ANPR),");
                //sb.Append("Total Record,");
                sb.Append("Lane,");
                sb.Append("EPC,");
                sb.Append("Resident ID,");
                sb.Append("Name,");
                sb.Append("Amount,");
                sb.Append("Balance,");
                sb.Append("Notification Status,");
                sb.Append("Image URL (Front ANPR),");
                sb.Append("Image URL (Rear ANPR),");
                sb.Append("Video URL (Front ANPR),");
                sb.Append("Video URL (Rear ANPR),");

            }

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine();
                //TIME_STAMP
                if (dr["TRANSACTION_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["TRANSACTION_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }
                //TIME_STAMP
                if (dr["TIME_STAMP"] != DBNull.Value)
                {
                    sb.Append(Convert.ToDateTime(dr["TIME_STAMP"]).ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HFileFormat) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //EVI_VEH_NO FRONT
                if (dr["EVI_VRN_FRONT"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_VRN_FRONT"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //EVI_VEH_NO REAR
                if (dr["EVI_VRN_REAR"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_VRN_REAR"]) + ",");
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

                //Class (Front EVI)
                if (dr["EVI_CLASS_FRONT"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_CLASS_FRONT"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Class (Rear EVI)
                if (dr["EVI_CLASS_REAR"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_CLASS_REAR"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Class (Front ANPR)
                if (dr["ANPR_CLASS_FRONT"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["ANPR_CLASS_FRONT"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Class (Rear ANPR)
                if (dr["ANPR_CLASS_REAR"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["ANPR_CLASS_REAR"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Total Record
                //if (dt.Rows.Count > 0)
                //{
                //    sb.Append(Convert.ToString(dt.Rows.Count) + ",");
                //}
                //else
                //{
                //    sb.Append(",");
                //}

                //Lane
                if (dr["LANE_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["LANE_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //EVI ID
                if (dr["EVI_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["EVI_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Resident ID
                if (dr["RESIDENT_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["RESIDENT_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Name
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
                else
                {
                    sb.Append(",");
                }
                //SMS_NOTIFICATION
                if (dr["SMS_NOTIFICATION"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["SMS_NOTIFICATION"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Image URL (Front ANPR)
                if (dr["FRONT_PLATE_IMAGE"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_PLATE_IMAGE"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Image URL (Rear ANPR)
                if (dr["REAR_PLATE_IMAGE"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_PLATE_IMAGE"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Video URL (Front ANPR)
                if (dr["FRONT_VIDEO_URL"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_VIDEO_URL"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Video URL (Rear ANPR)
                if (dr["REAR_VIDEO_URL"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_VIDEO_URL"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

            }
            if (dt.Rows.Count > 0)
            {

                DataRow[] filteredRows1 = dt.Select("EVI_CLASS_FRONT ='Two-wheeled'");
                DataRow[] filteredRows2 = dt.Select("EVI_CLASS_REAR ='Two-wheeled'");
                DataRow[] filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Two-wheeled'");
                DataRow[] filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Two-wheeled' ");

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(",,,,,,");
                sb.Append("Class (Front EVI),");
                sb.Append("Class (Rear EVI),");
                sb.Append("Class (Front ANPR),");
                sb.Append("Class (Rear ANPR),");
                sb.AppendLine();
                sb.Append("Total Two-wheeled,,,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows1.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows2.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows1 = dt.Select("EVI_CLASS_FRONT ='Small'");
                filteredRows2 = dt.Select("EVI_CLASS_REAR ='Small'");
                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Small'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Small' ");

                sb.Append("Total Small,,,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows1.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows2.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows1 = dt.Select("EVI_CLASS_FRONT ='Medium'");
                filteredRows2 = dt.Select("EVI_CLASS_REAR ='Medium'");
                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Medium'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Medium' ");

                sb.Append("Total Medium,,,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows1.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows2.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows1 = dt.Select("EVI_CLASS_FRONT ='Large'");
                filteredRows2 = dt.Select("EVI_CLASS_REAR ='Large'");
                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Large'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Large' ");

                sb.Append("Total Large,,,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows1.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows2.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();


                filteredRows1 = dt.Select("EVI_CLASS_FRONT ='blank'");
                filteredRows2 = dt.Select("EVI_CLASS_REAR ='blank'");
                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='blank'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='blank'");

                sb.Append("Total Unidentified,,,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows1.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows2.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();



                sb.Append("Total Record,,,,,,");
                sb.Append(
                    (Convert.ToInt32(dt.Rows.Count).ToString()) + "," +
                    (Convert.ToInt32(dt.Rows.Count).ToString()) + "," +
                    (Convert.ToInt32(dt.Rows.Count).ToString()) + "," +
                    (Convert.ToInt32(dt.Rows.Count).ToString()));
                sb.AppendLine();

            }

            return sb;
        }

        private static StringBuilder CreateCSVViolationDataFromDataTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            // Add header line
            if (dt.Rows.Count > 0)
            {
                sb.Append("Transaction ID,");
                sb.Append("Timestamp,");
                sb.Append("VRN (Front ANPR),");
                sb.Append("VRN (Rear ANPR),");
                sb.Append("Class (Front ANPR),");
                sb.Append("Class (Rear ANPR),");
                //sb.Append("Total Record,");
                sb.Append("Lane,");
                sb.Append("Image URL (Front ANPR),");
                sb.Append("Image URL (Rear ANPR),");
                sb.Append("Video URL (Front ANPR),");
                sb.Append("Video URL (Rear ANPR),");
            }

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine();
                if (dr["TRANSACTION_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["TRANSACTION_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }
                //TIME_STAMP
                if (dr["TIME_STAMP"] != DBNull.Value)
                {
                    sb.Append(Convert.ToDateTime(dr["TIME_STAMP"]).ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24HFileFormat) + ",");
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
                if (dr["ANPR_CLASS_FRONT"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["ANPR_CLASS_FRONT"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //VEH_NAME_NODEFLUX
                if (dr["ANPR_CLASS_REAR"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["ANPR_CLASS_REAR"]).Replace("blank", "") + ",");
                }
                else
                {
                    sb.Append(",");
                }
                //Total Record
                //if (dt.Rows.Count > 0)
                //{
                //    sb.Append(Convert.ToString(dt.Rows.Count) + ",");
                //}
                //else
                //{
                //    sb.Append(",");
                //}
                //LANE_ID
                if (dr["LANE_ID"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["LANE_ID"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }
                //Image URL (Front ANPR)
                if (dr["FRONT_PLATE_IMAGE"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_PLATE_IMAGE"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Image URL (Rear ANPR)
                if (dr["REAR_PLATE_IMAGE"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_PLATE_IMAGE"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Video URL (Front ANPR)
                if (dr["FRONT_VIDEO_URL"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["FRONT_VIDEO_URL"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

                //Video URL (Rear ANPR)
                if (dr["REAR_VIDEO_URL"] != DBNull.Value)
                {
                    sb.Append(Convert.ToString(dr["REAR_VIDEO_URL"]) + ",");
                }
                else
                {
                    sb.Append(",");
                }

            }
            if (dt.Rows.Count > 0)
            {
                //sb.AppendLine();
                //sb.AppendLine();
                //sb.AppendLine();
                //sb.Append("Total : " + (Convert.ToInt32(dt.Rows.Count)).ToString());

                DataRow[] filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Two-wheeled'");
                DataRow[] filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Two-wheeled' ");

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();

                sb.Append(",,,,");
                sb.Append("Class (Front ANPR),");
                sb.Append("Class (Rear ANPR),");
                sb.AppendLine();
                sb.Append("Total Two-wheeled,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Small'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Small' ");

                sb.Append("Total Small,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Medium'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Medium' ");

                sb.Append("Total Medium,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();

                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='Large'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='Large' ");

                sb.Append("Total Large,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();


                filteredRows3 = dt.Select("ANPR_CLASS_FRONT ='blank'");
                filteredRows4 = dt.Select("ANPR_CLASS_REAR ='blank'");

                sb.Append("Total Unidentified,,,,");
                sb.Append(
                    (Convert.ToInt32(filteredRows3.Length).ToString()) + "," +
                    (Convert.ToInt32(filteredRows4.Length).ToString()));
                sb.AppendLine();



                sb.Append("Total Record,,,,");
                sb.Append(
                    (Convert.ToInt32(dt.Rows.Count).ToString()) + "," +
                    (Convert.ToInt32(dt.Rows.Count).ToString()));
                sb.AppendLine();
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

        public static void UpdateCrossTalkSectionRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ctpEntryId)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_CTP_REAR";
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

        //public static void UpdateNodefluxSectionFront(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ntpEntryIdFront)
        //{
        //    try
        //    {
        //        //Stored procedure must have cur_out parameter.
        //        //There is no need to add ref cursor for oracle in code.
        //        string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_NF_FRONT";
        //        DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
        //        //where clause part
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
        //        //update part
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_FRONT", DbType.Int32, ntpEntryIdFront, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Int32, transaction.VehicleSpeed, ParameterDirection.Input));
        //        VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void UpdateNodefluxSectionRear(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Int32 ntpEntryIdRear)
        //{
        //    try
        //    {
        //        //Stored procedure must have cur_out parameter.
        //        //There is no need to add ref cursor for oracle in code.
        //        string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UPDATE_NF_REAR";
        //        DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
        //        //where clause part
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TMS_ID", DbType.Int32, transaction.TMSId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PLAZA_ID", DbType.Int32, transaction.PlazaId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_LANE_ID", DbType.Int32, transaction.LaneId, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, transaction.TransactionId, ParameterDirection.Input));
        //        //update part
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_NF_ENTRY_ID_REAR", DbType.Int32, ntpEntryIdRear, ParameterDirection.Input));
        //        command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_VEHICLESPEED", DbType.Int32, transaction.VehicleSpeed, ParameterDirection.Input));
        //        VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.ExecuteNonQuery(command);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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

        public static void MarkAsUnregistred(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_MARK_AS_UNREGISTRED";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                //where clause part
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

        public static DataTable GetDataTableFilteredRecordById(int TransactionId)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETFILTERED_BYID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, TransactionId, ParameterDirection.Input, 2000));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable GetUnReviewedDataTableById(int TransactionId)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UNREVIEWED_BY_ID";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_TRANSACTION_ID", DbType.Int32, TransactionId, ParameterDirection.Input, 2000));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable GetUnReviewedDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UNREVIEWED_FILTERED";

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

        public static DataTable GetUnReviewedDataTableFilteredRecordsLazyLoad(int PageIndex, int PageSize)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "UNREVIEWED_TRANS_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, PageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, PageSize, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable GetReviewedDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_REVIEWED_FILTERED";

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

        public static DataTable GetReviewedDataTableFilteredRecordsLazyLoad(int PageIndex, int PageSize)
        {
            try
            {
                string spName = Constants.oraclePackagePrefix + "TRAN_REVIEWED_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, PageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, PageSize, ParameterDirection.Input));
                DataTable dt = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName).Tables[tableName];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetChargedDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_CHARGED_FILTERED";

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

        public static DataTable GetChargedDataTableFilteredRecordsLazyLoad(int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "CHARGED_TRANS_LAZYLOAD_TEST";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, pageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, pageSize, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable GetViolationDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_VIOLATION_FILTERED";

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


        public static DataTable GetVIOLATIONDataTableFilteredRecordsLazyLoad(int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "VIOLATION_TRANS_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, pageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, pageSize, ParameterDirection.Input));
                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable GetUnIdentifiedDataTableFilteredRecords(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_UNIDENTIFIED_FILTERED";

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

        public static DataTable GetUnidentifiedDataTableFilteredRecordsLazyLoad(int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = Constants.oraclePackagePrefix + "UNIDENTIFIED_TRANS_LAZYLOAD";
                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_INDEX", DbType.String, pageIndex, ParameterDirection.Input));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_PAGE_SIZE", DbType.String, pageSize, ParameterDirection.Input));
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


        public static DataTable TransDeatils(string filter)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_DEATILS";

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

        public static DataTable TransDeatilsReport(string StartDate, string EndDate)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_DEATILSALL";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STARTDATE", DbType.String, StartDate, ParameterDirection.Input, 2000));
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENDDATE", DbType.String, EndDate, ParameterDirection.Input, 2000));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static DataTable StackChartData(string StartDate, string EndDate)
        {
            DataTable dt = new DataTable();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection();
            try
            {
                //Stored procedure must have cur_out parameter.
                //There is no need to add ref cursor for oracle in code.
                string spName = VaaaN.MLFF.Libraries.CommonLibrary.Constants.oraclePackagePrefix + "TRAN_GETSTACKCHART";

                DbCommand command = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.GetStoredProcCommand(spName);
                command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_STARTDATE", DbType.String, StartDate, ParameterDirection.Input, 2000));
                //command.Parameters.Add(VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.CreateDbParameter(ref command, "P_ENDDATE", DbType.String, EndDate, ParameterDirection.Input, 2000));

                DataSet ds = VaaaN.MLFF.Libraries.CommonLibrary.DBA.DBAccessor.LoadDataSet(command, tableName);
                dt = ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
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
                        transaction.CrosstalkEntryIdFront = Convert.ToInt32(dt.Rows[i]["CT_ENTRY_ID"]);

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

                    if (dt.Rows[i]["IS_REGISTERED"] != DBNull.Value)
                        transaction.IsRegistered = Convert.ToInt32(dt.Rows[i]["IS_REGISTERED"]);

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
