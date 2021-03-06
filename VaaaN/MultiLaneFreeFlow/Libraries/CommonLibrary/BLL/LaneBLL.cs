#region Copyright message
/*
� copyright 2011 VaaaN Infra (P) Ltd. All rights reserved.

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
    public class LaneBLL
    {
        public LaneBLL()
        {
        }

        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.LaneDAL.Insert(lane);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.LaneDAL.Update(lane);
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.LaneDAL.GetAllAsCollection();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection GetLaneByPlazaId(CBE.LaneCBE lane)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.LaneDAL.GetLaneByPlazaId(lane);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE GetLaneById(CBE.LaneCBE lane)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.LaneDAL.GetLaneById(lane);
        } 
      

    }
}
