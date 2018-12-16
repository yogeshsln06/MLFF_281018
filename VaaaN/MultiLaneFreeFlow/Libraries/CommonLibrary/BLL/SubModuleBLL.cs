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
    public class SubModuleBLL
    {
        public SubModuleBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.Insert(submodule);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.Update(submodule);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.GetAll();
        }

        public static DataTable GetSubModuleBySubModuleId(string sub_module_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.GetSubModuleBySubModuleId(sub_module_id);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE GetSubModuleById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.GetSubModuleById(submodule);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection GetByUserId(int userId, int moduleId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.GetByUserId(userId, moduleId);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection GetByModuleId(int moduleId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubModuleDAL.GetByModuleId(moduleId);
        }

    }
}
