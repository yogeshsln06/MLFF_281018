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
    public class RoleModuleRightBLL
    {
        public RoleModuleRightBLL()
        {
        }


        /// <summary>
        /// Insert or Update Rights for Role & module
        /// </summary>
        /// <param name="module"></param>
        public static void InsertUpdateRoleModuleRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE module)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.InsertUpdateRoleModuleRight(module);
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightCBE roleModuleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.Insert(roleModuleRight);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightCBE roleModuleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.Delete(roleModuleRight);
        }

        public static void DeleteRoleRightByRoleId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightCBE roleModuleRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.DeleteRoleRightByRoleId(roleModuleRight);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.GetAll();
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleModuleRightActivityCBE> GetRoleModuleRightByRoleId(Int32 role_Id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleModuleRightDAL.GetRoleModuleRightByRoleId(role_Id);
        }
    }
}
