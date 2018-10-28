using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class UserModuleActivityRightBLL
    {
        /// <summary>
        /// Read module right by User Id
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE> GetUserModuleRightByUserId(Int32 user_Id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleActivityRightDAL.GetUserModuleRightByUserId(user_Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE GetModuleRightById(Int32 Id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleActivityRightDAL.GetModuleRightById(Id);
        }
        /// <summary>
        /// Insert or update Module rights by user Id and Module Id
        /// </summary>
        /// <param name="module"></param>
        public static void InsertUpdateUserModuleActivityRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserModuleActivityRightCBE module)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserModuleActivityRightDAL.InsertUpdateUserModuleActivityRight(module);
        }
    }
}
