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
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for RoleBLL
    /// </summary>
    public class CustomerVehicleBLL
    {
        public CustomerVehicleBLL()
        {
        }

        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.Insert(vehicle);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.Update(vehicle);
        }

        public static Decimal UpdateVehiclebalance(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle, Decimal Amount)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.UpdateVehiclebalance(vehicle, Amount);
        }


        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> GetAllAsList()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetAllAsList();
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetAllAsCollection()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetAllAsCollection();
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetUserById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetUserById(vehicle);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetByTagId(String TagId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetByTagId(TagId);
        }
        public static DataTable GetCustomerVehicleById_DT(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleById_DT(customerVehicle);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetUserByAccountId(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetUserByAccountId(customerVehicle);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetByTansactionCrosstalkEntryId(int tranCrosstalkENtryId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetByTansactionCrosstalkEntryId(tranCrosstalkENtryId);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetCustomerVehicleById(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleById(customerVehicle);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetCustomerVehicleByVehRegNo(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleByVehRegNo(customerVehicle);
        }

        public static DataTable ValidateCustomerVehicleDetails(CBE.CustomerVehicleCBE vehicle, string ResidentId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.ValidateCustomerVehicleDetails(vehicle, ResidentId);
        }

        public static DataTable CustomerVehicleDetailsByResidentId(string ResidentId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.CustomerVehicleDetailsByResidentId(ResidentId);
        }

        public static DataTable CustomerVehicleDetailsByTID(string TID)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.CustomerVehicleDetailsByTID(TID);
        }

        public static DataTable GetAllVehicleinDataTable()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetAllVehicleinDataTable();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetLatestCustomerVehicles(DateTime lastCollectionUpdateTime)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetLatestCustomerVehicles(lastCollectionUpdateTime);
        }

        public static List<CustomerVehicleCBE> CustomerVehicleAccountLazyLoad(int PageIndex, int PageSize)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.CustomerVehicleAccountLazyLoad(PageIndex, PageSize);
        }

        public static List<CustomerVehicleCBE> GetCustomerVehicleByAccountId(int AccountId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleByAccountId(AccountId);
        }

        public static List<CBE.CustomerVehicleCBE> GetCustomerVehicleFiltered(string filtere)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleFiltered(filtere);
        }

        public static DataTable GetAllAsCSV()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetAllAsCSV();
        }

        public static DataSet GetVehicleBalanceReport(Int32 VehcileId, Int32 Month, Int32 Year, Int32 PMonth, Int32 PYear)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetVehicleBalanceReport(VehcileId, Month, Year, PMonth, PYear);
        }

        public static DataTable GetFilterCSV(string filtere)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetFilterCSV(filtere);
        }

        public static CBE.CustomerVehicleCollection GetCustomerbalanceUpdateMobile()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerbalanceUpdateMobile();
        }
    }
}
