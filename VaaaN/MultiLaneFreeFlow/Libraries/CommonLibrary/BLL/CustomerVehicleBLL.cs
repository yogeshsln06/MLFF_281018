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
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetByCustomerId(String customerId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetByTagId(customerId);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection GetUserByAccountId(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetUserByAccountId(customerVehicle);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetByTansactionCrosstalkEntryId (int tranCrosstalkENtryId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetByTansactionCrosstalkEntryId(tranCrosstalkENtryId);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE GetCustomerVehicleById(CBE.CustomerVehicleCBE customerVehicle)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerVehicleDAL.GetCustomerVehicleById(customerVehicle);
        }

    }
}
