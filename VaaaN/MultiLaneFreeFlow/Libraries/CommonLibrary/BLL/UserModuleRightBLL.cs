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

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for UserRightBLL
    /// </summary>
    public class UserModuleRightBLL
    {
        public UserModuleRightBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCBE userRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleRightDAL.Insert(userRight);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCBE userRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleRightDAL.Delete(userRight);
        }

        public static void DeleteUserRightByUserId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCBE userRight)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleRightDAL.DeleteUserModuleRightByUserId(userRight);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleRightDAL.GetAll();
        }
    }
}
