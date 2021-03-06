﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class AccountHistoryBLL
    {
        public AccountHistoryBLL()
        {
        }
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.Insert(accountHistory);
        }

        public static int Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.Update(accountHistory);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.Delete(accountHistory);
        }
        public static int UpdateBalanceStatus(int EntryId, int SentStatus, string response)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.UpdateBalanceStatus(EntryId, SentStatus, response);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.GetAll();
        }

        public static DataTable AccountHistoryBYAccountId(int AccountId, int TranactionType)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryBYAccountId(AccountId, TranactionType);
        }

        public static DataTable AccountHistoryBYAccountIdLazyLoad(int AccountId, int pageIndex, int pageSize)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryBYAccountIdLazyLoad(AccountId, pageIndex, pageSize);
        }

        public static DataTable AccountHistoryBYVehicleIdLazyLoad(int AccountId, int Vehicleid, int pageIndex, int pageSize)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryBYVehicleIdLazyLoad(AccountId, Vehicleid, pageIndex, pageSize);
        }

        public static DataTable AccountHistoryByVehicle(string ResidentIdentityNumber, string VehicleRegistrationCertificateNumber, string VehicleRegistrationNumber)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryByVehicle(ResidentIdentityNumber, VehicleRegistrationCertificateNumber, VehicleRegistrationNumber);
        }

        public static DataTable AccountHistoryByVehicleWithPaging(string ResidentIdentityNumber, string VehicleRegistrationCertificateNumber, string VehicleRegistrationNumber, int PageIndex, int PageSize)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryByVehicleWithPaging(ResidentIdentityNumber, VehicleRegistrationCertificateNumber, VehicleRegistrationNumber, PageIndex, PageSize);
        }
        public static DataTable GetTopUpDataTableFilteredRecordsLazyLoad(int pageIndex, int pageSize)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.GetTopUpDataTableFilteredRecordsLazyLoad(pageIndex, pageSize);
        }

        public static DataTable GetTopUpDataTableFilteredRecords(string filter)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.GetTopUpDataTableFilteredRecords(filter);
        }
    }
}
