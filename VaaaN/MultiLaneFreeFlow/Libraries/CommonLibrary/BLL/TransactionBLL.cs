#region Copyright message
/*
© copyright 2011 VaaaN Infra (P) Ltd. All rights reserved.

This file contains Proprietary information of VaaaN Infra (P) Ltd.
and should be treated as proprietary and confidential.

The use, copying, disclosure or modification of the programs and/or 
source code is prohibited unless otherwise provided for in the license 
or confidential agreements.

========================================================
Author           :  VaaaN Infra                  
Company          :  VaaaN Infra     
Date of Creation :                              
========================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using VaaaN.MLFF.Libraries.CommonLibrary.DAL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using System.Collections;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for RoleBLL
    /// </summary>
    public class TransactionBLL
    {
        public TransactionBLL()
        {
        }

        public static Int64 InsertByCTP(CrossTalkPacketCBE ctp, int ctpEntryId)//, int eviVehicleClassId, string eviVRN)
        {
            TransactionCBE transaction = new TransactionCBE();
            transaction.TMSId = 1;
            transaction.PlazaId = ctp.PlazaId;
            transaction.LaneId = ctp.LaneId;
            //transaction.TransactionId = ; //auto increment
            transaction.TransactionDateTime = Convert.ToDateTime(ctp.TimeStamp);
            //transaction.CrosstalkTagId = ctp.FirstRead;
            //transaction.CrosstalkVehicleClassId = eviVehicleClassId.ToString();
            //transaction.CrosstalkVRN = eviVRN;
            //transaction.CrosstalkTimestamp = ctp.TimeStamp;
            //transaction.NodefluxVRN = ""; //will be updated when NFP arrives
            //transaction.NodefluxVehicleClassId = ""; //will be updated when NFP arrives
            //transaction.NodefluxTimestamp = ""; //will be updated when NFP arrives
            transaction.CrosstalkEntryId = ctpEntryId;
            //transaction.NodefluxEntryId = ; // will be updated when NFP arrives

            transaction.IsBalanceUpdated = -1; //this is important
            transaction.IsTransfered = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred);
            transaction.IsViolation = -1; // this is important
            //transaction.ModifierId = 0;//will be updated later

            transaction.CreationDate = System.DateTime.Now;
            transaction.ModificationDate = System.DateTime.Now;

            transaction.IsRegistered = 1; //it will be always 1 in this case

            return TransactionDAL.InsertByCTP(transaction);
        }

        public static Int64 InsertByNFPFront(NodeFluxPacketCBE nfp, int nfpEntryId, int isRegistered)
        {
            TransactionCBE transaction = new TransactionCBE();
            transaction.TMSId = 1;
            transaction.PlazaId = nfp.GantryId;
            transaction.LaneId = nfp.LaneId;//???
            //transaction.TransactionId = ;//auto increment
            transaction.TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp);
            //transaction.CrosstalkTagId = ;
            //transaction.CrosstalkVehicleClassId = ;
            //transaction.CrosstalkVRN = ;
            //transaction.CrosstalkTimestamp = ;
            //transaction.NodefluxVRNFront = "";//will be updated later
            //transaction.NodefluxVehicleClassIdFront = "";
            //transaction.NodefluxTimestampFront = "";
            //transaction.CrosstalkEntryId = crostalkEventId;
            transaction.NodefluxEntryIdFront = nfpEntryId;

            transaction.IsBalanceUpdated = -1;//this is important
            transaction.IsTransfered = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred);
            transaction.IsViolation = -1;//this is important
            //transaction.ModifierId = 0;//will be updated later

            transaction.CreationDate = System.DateTime.Now;
            transaction.ModificationDate = System.DateTime.Now;

            transaction.IsRegistered = isRegistered;

            return TransactionDAL.InsertByNFPFront(transaction);
        }

        public static Int64 InsertByNFPRear(NodeFluxPacketCBE nfp, int nfpEntryId, int isRegistered)
        {
            TransactionCBE transaction = new TransactionCBE();
            transaction.TMSId = 1;
            transaction.PlazaId = nfp.GantryId;
            transaction.LaneId = nfp.LaneId;
            //transaction.TransactionId = ;//auto increment
            transaction.TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp);
            //transaction.CrosstalkTagId = ;
            //transaction.CrosstalkVehicleClassId = ;
            //transaction.CrosstalkVRN = ;
            //transaction.CrosstalkTimestamp = ;
            //transaction.NodefluxVRNRear = "";//will be updated later
            //transaction.NodefluxVehicleClassIdRear = "";
            //transaction.NodefluxTimestampRear = "";
            //transaction.CrosstalkEntryId = crostalkEventId;
            transaction.NodefluxEntryIdRear = nfpEntryId;

            transaction.IsBalanceUpdated = -1;//will be updated later
            transaction.IsTransfered = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred);
            transaction.IsViolation = -1;
            //transaction.ModifierId = 0;//will be updated later

            transaction.CreationDate = System.DateTime.Now;
            transaction.ModificationDate = System.DateTime.Now;

            transaction.IsRegistered = isRegistered;

            return TransactionDAL.InsertByNFPRear(transaction);
        }

        //=================================================================
        public static Int64 UpdateByNFPFront(TransactionCBE tran, int nfpEntryId)
        {
            return TransactionDAL.UpdateByNFPFront(tran, nfpEntryId);
        }

        public static Int64 UpdateByNFPRear(TransactionCBE tran, int nfpEntryId)
        {
            return TransactionDAL.UpdateByNFPRear(tran, nfpEntryId);
        }
        //=================================================================


        //public static void Update(NodeFluxPacketCBE nfp, Int32 nodefluxEventId)
        //{
        //    TransactionCBE transaction = new TransactionCBE();
        //    transaction.TMSId = 1;
        //    //transaction.PlazaId = tran.PlazaId;//already inserted;
        //    //transaction.LaneId = -1;//???
        //    transaction.TransactionId = -1;//Find by Plaza id and VRN
        //    //transaction.CrosstalkTagId = "";//already inserted;
        //    //transaction.CrosstalkVehicleClassId = "";//already inserted;
        //    //transaction.CrosstalkVRN = "";//already inserted;
        //    transaction.NodefluxVRN = nfp.DataPlate;
        //    transaction.NodefluxVehicleClassId = nfp.DataVehicleType;
        //    //transaction.CrosstalkEntryId = //already inserted;
        //    transaction.NodefluxEntryId = nodefluxEventId;
        //    //transaction.IsBalanceUpdated = 0;//will be updated later
        //    //transaction.IsTransfered = Convert.ToInt32(Libraries.CommonLibrary.Constants.TransferStatus.Transferred);//already inserted;
        //    //transaction.IsViolation = 0;//already inserted;
        //    //transaction.ModifierId = 0;//already inserted;
        //    //transaction.CreationDate = System.DateTime.Now;//already inserted;
        //    transaction.ModificationDate = System.DateTime.Now;
        //    TransactionDAL.Update(transaction);
        //}


        public static TransactionCollection GetAll()
        {
            return TransactionDAL.GetAll();
        }

        //////public static TransactionCollection GetCorrespondingTransaction(CBE.NodeFluxPacketCBE nfp)
        //////{
        //////    return TransactionDAL.GetCorrespondingTransaction(nfp);
        //////}

        public static TransactionCollection GetCorrespondingTransactionInNodeFlux(Int32 tmsId, Int32 plazaId, DateTime timestamp, string ctpVrn)
        {
            return TransactionDAL.GetCorrespondingTransactionInNodeFlux(tmsId, plazaId, timestamp, ctpVrn);
        }

        public static TransactionCollection GetCorrespondingTransactionInCrossTalk(Int32 tmsId, Int32 plazaId, DateTime timestamp, string nfpVrn)
        {
            return TransactionDAL.GetCorrespondingTransactionInCossTalk(tmsId, plazaId, timestamp, nfpVrn);
        }

        public static TransactionCollection GetCorrespondingTransactionInAll(Int32 tmsId, Int32 plazaId, DateTime timestamp, string nfpVrn)
        {
            return TransactionDAL.GetCorrespondingTransactionInAll(tmsId, plazaId, timestamp, nfpVrn);
        }

        public static void UpdateCrossTalkSection(CBE.TransactionCBE transaction, Int32 ctpEntryId)
        {
            TransactionDAL.UpdateCrossTalkSection(transaction, ctpEntryId);
        }

        public static void UpdateNodefluxSectionFront(CBE.TransactionCBE transaction, Int32 ntpEntryId)
        {
            TransactionDAL.UpdateNodefluxSectionFront(transaction, ntpEntryId);
        }

        public static void UpdateNodefluxSectionRear(CBE.TransactionCBE transaction, Int32 ntpEntryId)
        {
            TransactionDAL.UpdateNodefluxSectionRear(transaction, ntpEntryId);
        }

        public static DataTable Transaction_LiveData()
        {
            return TransactionDAL.Transaction_LiveData();
        }

        public static DataTable Transaction_GetById(CBE.TransactionCBE transaction)
        {
            return TransactionDAL.Transaction_GetById(transaction);
        }

        public static void MarkAsViolation(CBE.TransactionCBE transaction)
        {
            TransactionDAL.MarkAsViolation(transaction);
        }

        public static void MarkAsBalanceUpdated(CBE.TransactionCBE transaction)
        {
            TransactionDAL.MarkAsBalanceUpdated(transaction);
        }

        public static void UpdateAuditSection(CBE.TransactionCBE transaction)
        {
            TransactionDAL.UpdateAuditSection(transaction);
        }
        public static void UpdateTransaction(CBE.TransactionCBE transaction)
        {
            TransactionDAL.Update(transaction);
        }

        #region CSV Data
        public static StringBuilder GetNormalTransactions(DateTime startTime, DateTime endTime)
        {
            return TransactionDAL.GetNormalTransactions(startTime, endTime);
        }

        public static StringBuilder GetViolationTransactions(DateTime startTime, DateTime endTime)
        {
            return TransactionDAL.GetViolationTransactions(startTime, endTime);
        }

        public static StringBuilder GetWithoutTagTransactions(DateTime startTime, DateTime endTime)
        {
            return TransactionDAL.GetWithoutTagTransactions(startTime, endTime);
        }
        #endregion


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection GetFilteredRecords(string filter)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TransactionDAL.GetFilteredRecords(filter);
        }

        public static DataTable GetDataTableFilteredRecords(string filter)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TransactionDAL.GetDataTableFilteredRecords(filter);
        }

        public static CBE.TransactionCollection FilteredTransactions(string filter)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TransactionDAL.FilteredTransactions(filter);
        }
        public static void JoinAuditTransaction(long parentTransactionId, long childOneTransactionId, long childTwoTransactionId, string auditedVRN, int auditedVehicleClassId, int auditorID)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.TransactionDAL.JoinAuditTransaction(parentTransactionId, childOneTransactionId, childTwoTransactionId, auditedVRN, auditedVehicleClassId, auditorID);
        }
        
    }
}
