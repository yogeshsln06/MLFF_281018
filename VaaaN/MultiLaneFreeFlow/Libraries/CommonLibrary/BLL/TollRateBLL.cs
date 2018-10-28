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
    public class TollRateBLL
    {
        public TollRateBLL()
        {
        }

        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollrate)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TollRateDAL.Insert(tollrate);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tollrate)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.TollRateDAL.Update(tollrate);
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TollRateDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE GetSpecificTollRate(int TMSId, int PlazaId, int LaneId, int VehicleClassId, DateTime TransactionTime)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.TollRateDAL.GetSpecificTollRate(TMSId, PlazaId, LaneId, VehicleClassId, TransactionTime);
        }


    }
}
