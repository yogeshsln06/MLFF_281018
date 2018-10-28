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
using System.Data;


namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for ModuleBLL
    /// </summary>

    public class ModuleBLL
    {
        public ModuleBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.Insert(module);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.Update(module);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.GetAll();
        }
        
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE GetModuleById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.GetModuleById(module);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE GetModuleById(int moduelId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.GetModuleById(moduelId);
        }

    }
}
