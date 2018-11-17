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
    public class NodeFluxBLL
    {
        public NodeFluxBLL()
        {
        }

        public static int Insert(NodeFluxPacketCBE nodefluxPacketCBE)
        {
            return NodeFluxDAL.Insert(nodefluxPacketCBE);
        }

        public static void Update(NodeFluxPacketCBE nodefluxPacketCBE)
        {
            NodeFluxDAL.Update(nodefluxPacketCBE);
        }


        public static NodeFluxPacketCollection GetAll()
        {
            return NodeFluxDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection GetRecent(int plazaId, string vrn, DateTime nodeFluxReportingTime, int cameraPosition)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetRecent(plazaId, vrn, nodeFluxReportingTime, cameraPosition);
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE GetByEntryId(int entryId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(entryId);
        }

    }
}
