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
    public class HardwareBLL
    {
        public HardwareBLL()
        {
        }

        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.Insert(hardware);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.Update(hardware);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE GetHardwareById(CBE.HardwareCBE hardware)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.GetHardwareById(hardware);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE GetHardwareByType(CBE.HardwareCBE hardware)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.GetHardwareByType(hardware);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection GetLatestHardwares(DateTime lastCollectionUpdateTime)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.GetLatestHardwares(lastCollectionUpdateTime);
        }


        public static string GetActiveANPR()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.HardwareDAL.GetActiveANPR();
        }

    }
}
