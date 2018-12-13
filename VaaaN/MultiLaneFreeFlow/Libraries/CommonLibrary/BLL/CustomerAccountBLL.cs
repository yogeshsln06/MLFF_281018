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

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for RoleBLL
    /// </summary>
    public class CustomerAccountBLL
    {
        public CustomerAccountBLL()
        {
        }
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.Insert(account);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.Update(account);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetAllAsCollection()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetAllAsCollection();
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> GetAllAsList()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetAllAsList();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetByMobileNumber(string mobilenumber)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetByMobileNumber(mobilenumber);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetById(Int32 customerId, Int32 tmsId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetById(customerId, tmsId);
        }

        public static void UpdateBalance(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE account, Decimal amount)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.UpdateBalance(account, amount);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE GetCustomerById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetCustomerById(customer);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE GetCustomerByResidentId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetCustomerByResidentId(customer);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection GetLatestCustomerAccounts(DateTime lastCollectionUpdateTime)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.GetLatestCustomerAccounts(lastCollectionUpdateTime);
        }

        public static List<CBE.CustomerAccountCBE> ValidateCustomerAccount(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customer)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAccountDAL.ValidateCustomerAccount(customer);
        }
    }
}
