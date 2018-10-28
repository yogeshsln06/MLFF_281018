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
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary
{
    public class UserPrivileges
    {
        #region Variable
        static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCollection userModuleRights;
        static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleActivityCollection moduleActivities;
        static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubmoduleRightCollection userSubmoduleRights;
        static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCollection subModuleActivities;
        static int currentATMSId =VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentTMSId();
        static int currentPlazaId = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentPlazaId();
        #endregion
                
        static UserPrivileges()
        {
            userModuleRights = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserModuleRightBLL.GetAll();
            moduleActivities = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ModuleActivityBLL.GetAll();

            userSubmoduleRights = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubmoduleRightBLL.GetAll();
            subModuleActivities = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubmoduleActivityBLL.GetAll();
        }

        #region Modules Right
        public static bool IsModuleViewAllowed(int atmsId, int userId, int moduleId)
        {
            bool isAllowed = false;

            //Admin and superUser has all the rights
            if (userId == 1)
            {
                return true;
            }

            //Check module right
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleRightCBE userModuleRight in userModuleRights)
            {
                if (userModuleRight.UserId == userId)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleActivityCBE moduleActivity in moduleActivities)
                    {
                        if (userModuleRight.ModuleActivityEntryId == moduleActivity.EntryId && moduleActivity.ModuleId == moduleId
                            && moduleActivity.ActivityId == (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.Activity.View)
                        {
                            isAllowed = true;

                            break;
                        }
                    }
                }

                if (isAllowed) break;
            }

            if (isAllowed)
            {
                //Check if there is any submodule if multiple submodule and there is no rights for at leat one submodule then module right should be denied
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetAll();
                ArrayList alSubModules = new ArrayList();
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule in subModules)
                {
                    if (subModule.ModuleId == moduleId)
                    {
                        if (IsSubModuleViewAllowed(userId, subModule.SubModuleId))
                        {
                            alSubModules.Add(subModule);
                        }
                    }
                }

                if (alSubModules.Count == 0)
                {
                    isAllowed = false;
                }
            }

            return isAllowed;
        }

        public static bool IsModuleAddAllowed(int userId, int moduleId)
        {
            return false;
        }
        public static bool IsModuleEditAllowed(int userId, int moduleId)
        {
            return false;
        }
        public static bool IsModuleDeleteAllowed(int userId, int moduleId)
        {
            return false;
        }
        public static bool IsModuleProcessAllowed(int userId, int moduleId)
        {
            return false;
        }
        #endregion

        #region Submodules Right
        /// <summary>
        /// Check if current logged in user is allowed to view the module or not.
        /// </summary>
        /// <param name="userId">User id of the logged in user.</param>
        /// <param name="moduleId">Module id</param>
        /// <returns>Status if allowed or not</returns>
        public static bool IsSubModuleViewAllowed(int userId, int subModuleId)
        {
            userModuleRights = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserModuleRightBLL.GetAll();
            moduleActivities = VaaaN.MLFF.Libraries.CommonLibrary.BLL.ModuleActivityBLL.GetAll();

            userSubmoduleRights = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserSubmoduleRightBLL.GetAll();
            subModuleActivities = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubmoduleActivityBLL.GetAll();

            bool isAllowed = false;

            //Admi and superUser has all the rights
            if (userId == 1)
            {
                return true;
            }

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubmoduleRightCBE userSubmoduleRight in userSubmoduleRights)
            {
                if (userSubmoduleRight.UserId == userId)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE subModuleActivity in subModuleActivities)
                    {
                        if (userSubmoduleRight.SubModuleActivityEntryId == subModuleActivity.EntryId && subModuleActivity.SubModuleId == subModuleId &&
                            subModuleActivity.ActivityId == (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.Activity.View)
                        {
                            isAllowed = true;
                            break;
                        }
                    }
                }

                if (isAllowed) break;
            }

            return isAllowed;
        }

        /// <summary>
        /// Check if current logged in user is allowed to add the new record.
        /// </summary>
        /// <param name="userId">User id of the logged in user.</param>
        /// <param name="moduleId">Module Id</param>
        /// <returns>Status if allowed or not</returns>
        public static bool IsSubModuleAddAllowed(int userId, int moduleId, int subModuleId)
        {
            bool isAllowed = false;

            //Admi and superUser has all the rights
            if (userId == 1)
            {
                return true;
            }

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubmoduleRightCBE userSubmoduleRight in userSubmoduleRights)
            {
                if (userSubmoduleRight.UserId == userId)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE subModuleActivity in subModuleActivities)
                    {
                        if (userSubmoduleRight.SubModuleActivityEntryId == subModuleActivity.EntryId && subModuleActivity.SubModuleId == subModuleId &&
                            subModuleActivity.ActivityId == (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.Activity.Add)
                        {
                            isAllowed = true;
                            break;
                        }
                    }
                }

                if (isAllowed) break;
            }

            return isAllowed;
        }

        /// <summary>
        /// Check if current logged in user is allowed to edit the existing record.
        /// </summary>
        /// <param name="userId">User id of the logged in user.</param>
        /// <param name="moduleId">Module Id</param>
        /// <returns>Status if allowed or not</returns>
        public static bool IsSubModuleEditAllowed(int userId, int moduleId, int subModuleId)
        {
            bool isAllowed = false;

            //Admi and superUser has all the rights
            if (userId == 1)
            {
                return true;
            }

            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubmoduleRightCBE userSubmoduleRight in userSubmoduleRights)
            {
                if (userSubmoduleRight.UserId == userId)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE subModuleActivity in subModuleActivities)
                    {
                        if (userSubmoduleRight.SubModuleActivityEntryId == subModuleActivity.EntryId && subModuleActivity.SubModuleId == subModuleId &&
                            subModuleActivity.ActivityId == (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.Activity.Edit)
                        {
                            isAllowed = true;
                            break;
                        }
                    }
                }

                if (isAllowed) break;
            }

            return isAllowed;
        }

        /// <summary>
        /// Check if current logged in user is allowed to delete the existing record.
        /// </summary>
        /// <param name="userId">User id of the logged in user.</param>
        /// <param name="moduleId">Module Id</param>
        /// <returns>Status if allowed or not</returns>
        public static bool IsSubModuleDeleteAllowed(int userId, int moduleId, int subModuleId)
        {
            bool isAllowed = false;
            ////DON'T delete it
            //foreach (VaaaN.TollMax.Library.CBE.UserSubmoduleRightCBE userSubmoduleRight in userSubmoduleRights)
            //{
            //    if (userSubmoduleRight.UserId == userId)
            //    {
            //        foreach (VaaaN.TollMax.Library.CBE.SubmoduleActivityCBE subModuleActivity in subModuleActivities)
            //        {
            //            if (userSubmoduleRight.SubModuleActivityEntryId == subModuleActivity.EntryId && subModuleActivity.SubModuleId == subModuleId &&
            //                subModuleActivity.ActivityId == (int)VaaaN.TollMax.Library.Constants.Activity.Delete)
            //            {
            //                isAllowed = true;
            //                break;
            //            }
            //        }
            //    }

            //    if (isAllowed) break;
            //}

            return isAllowed;
        }

        ///// <summary>
        ///// Check if current logged in user is allowed to delete the existing record.
        ///// </summary>
        ///// <param name="userId">User id of the logged in user.</param>
        ///// <param name="moduleId">Module Id</param>
        ///// <returns>Status if allowed or not</returns>
        //public static bool IsSubModuleProcessAllowed(int userId, int moduleId, int subModuleId)
        //{
        //    bool isAllowed = false;

        //    //Admi and superUser has all the rights
        //    if (userId == 1 || userId == 2)
        //    {
        //        return true;
        //    }

        //    foreach (VaaaN.ATMS.Libraries.CommonLibrary.CBE.UserSubmoduleRightCBE userSubmoduleRight in userSubmoduleRights)
        //    {
        //        if (userSubmoduleRight.UserId == userId)
        //        {
        //            foreach (VaaaN.ATMS.Libraries.CommonLibrary.CBE.SubmoduleActivityCBE subModuleActivity in subModuleActivities)
        //            {
        //                if (userSubmoduleRight.SubModuleActivityEntryId == subModuleActivity.EntryId && subModuleActivity.SubModuleId == subModuleId &&
        //                    subModuleActivity.ActivityId == (int)VaaaN.ATMS.Libraries.CommonLibrary.Constants.Activity.Process)
        //                {
        //                    isAllowed = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (isAllowed) break;
        //    }

        //    return isAllowed;
        //}
        #endregion
    }
}
