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

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for RoleBLL
    /// </summary>
    public class CrossTalkBLL
    {
        public CrossTalkBLL()
        {
        }

        public static int Insert(CrossTalkPacketCBE crostalkCBE)
        {
            return CrossTalkDAL.Insert(crostalkCBE);
        }

        public static void Update(CrossTalkPacketCBE crostalkCBE)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CrossTalkDAL.Update(crostalkCBE);
        }


        public static CrossTalkPacketCollection GetAll()
        {
            return CrossTalkDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection GetRecent(int plazaId, string tagId, DateTime tagReportingTime)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CrossTalkDAL.GetRecent(plazaId, tagId, tagReportingTime);
        }
    }
}
