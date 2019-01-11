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

    public class SMSCommunicationHistoryBLL
    {
        public SMSCommunicationHistoryBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SMSCommunicationHistoryDAL.Insert(sms);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SMSCommunicationHistoryDAL.Update(sms);
        }

        public static void UpdateFirstResponse(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SMSCommunicationHistoryDAL.UpdateFirstResponse(sms);
        }

        public static void UpdateSecondResponse(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.SMSCommunicationHistoryDAL.UpdateSecondResponse(sms);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCollection GetFilteredRecords(string filter)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SMSCommunicationHistoryDAL.GetFilteredRecords(filter);
        }
    }
}
