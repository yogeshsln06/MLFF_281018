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

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for RoleRightBLL
    /// </summary>
    public class RoleSubmoduleRightBLL
    {
        public RoleSubmoduleRightBLL()
        {
        }
        public static void InsertUpdateRoleSubModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE submodule)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.InsertUpdateRoleSubModuleRight(submodule);
        }
        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubmoduleRightCBE roleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.Insert(roleRight);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubmoduleRightCBE roleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.Delete(roleRight);
        }

        public static void DeleteRoleRightByRoleId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubmoduleRightCBE roleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.DeleteRoleRightByRoleId(roleRight);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubmoduleRightCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.GetAll();
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleSubModuleRightActivityCBE> GetRoleSubmoduleRightByRoleId(Int32 role_Id, Int32 moduleId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleSubmoduleRightDAL.GetRoleSubModuleRightByRoleId(role_Id, moduleId);
        }
    }
}
