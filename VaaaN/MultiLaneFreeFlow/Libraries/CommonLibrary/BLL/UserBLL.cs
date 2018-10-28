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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for UserBLL
    /// </summary>
    public class UserBLL
    {
        public UserBLL()
        {
        }

        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            user.Password = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(user.Password);
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.Insert(user);
        }
        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.Update(user);
        }
        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE newObject, VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE oldObject)
        {
            //Update user record
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.Update(newObject);
        }
        public static void UpdateWithPassword(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE newObject, VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE oldObject)
        {
            newObject.Password = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(newObject.Password);
            //Update user record
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.UpdateWithPassword(newObject);
        }

        public static string UpdatePassword(string oldPassword, string changedPassword, int user_id, string email_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.UpdatePassword(oldPassword, changedPassword, user_id, email_id);
        }
        public static void Delete(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.Delete(user);
        }

        public static int User_Insert_LoginInfo(int userId)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.User_Insert_LoginInfo(userId);
        }

        public static void User_update_LoginInfo(int userId, int loginId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.User_update_LoginInfo(userId, loginId);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection GetUserAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserAll();
        }

        public static DataTable dtGetUserAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.dtGetUserAll();
        }
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE> GetAll(VaaaN.MLFF.Libraries.CommonLibrary.Common.SortingPagingInfo info, ref Int32 RecordCount)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetAll(info, ref RecordCount);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection PagedGetAll(int startRowIndex, int endRowIndex, ref int totalRows)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.PagedGetAll(startRowIndex, endRowIndex, ref totalRows);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE GetUserById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserById(user);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection GetUserByUserName(String UserName)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserByLoginName(UserName);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE ValidateUser(string loginName, string password)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.ValidateUser(loginName, password);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE GetUserByEmailId(string email_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserByEmailId(email_id);
        }
        public static string ValidateLoginName(string loginName, string email_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.ValidateLoginName(loginName, email_id);
        }
    }
}
