using System;
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.GetAll();
        }

        public static DataTable AccountHistoryBYAccountId(int AccountId, int TranactionType)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryBYAccountId(AccountId, TranactionType);
        }

        public static DataTable AccountHistoryByVehicle(string ResidentIdentityNumber, string VehicleRegistrationCertificateNumber, string VehicleRegistrationNumber)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.AccountHistoryDAL.AccountHistoryByVehicle(ResidentIdentityNumber, VehicleRegistrationCertificateNumber, VehicleRegistrationNumber);
        }
    }
}
