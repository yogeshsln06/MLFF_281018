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
    /// Summary description for ModuleActivityBLL
    /// </summary>
    public class SubmoduleActivityBLL
    {
        public SubmoduleActivityBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE submoduleActivity)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubmoduleActivityDAL.Insert(submoduleActivity);
        }

        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE submoduleActivity)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubmoduleActivityDAL.Delete(submoduleActivity);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubmoduleActivityDAL.GetAll();
        }
    }
}
