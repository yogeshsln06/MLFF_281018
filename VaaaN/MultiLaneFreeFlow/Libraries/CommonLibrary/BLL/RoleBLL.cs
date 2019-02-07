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
    public class RoleBLL
    {
        public RoleBLL()
        {
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> GetRoleList(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, ref Int32 RecordCount)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetRoleList(info, ref RecordCount);
        }

        public static string Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.Insert(role);
        }

        public static string Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role, string oldrole_name)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.Update(role, oldrole_name);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.Delete(role);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCollection GetRoleAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetRoleAll();
        }

        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE> GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetAll();
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCollection PagedGetAll(int startRowIndex, int endRowIndex, ref int totalRows)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.PagedGetAll(startRowIndex, endRowIndex, ref totalRows);
        }
        public static DataTable GetRoleById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetRoleById(role);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE GetRoleByIdcollection(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetRoleByIdCollection(role);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCollection GetRoleByName(VaaaN.MLFF.Libraries.CommonLibrary.CBE.RoleCBE role)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.RoleDAL.GetRoleByName(role);
        }
    }
}
