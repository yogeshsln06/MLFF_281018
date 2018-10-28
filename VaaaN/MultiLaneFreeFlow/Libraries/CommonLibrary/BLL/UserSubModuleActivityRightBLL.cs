
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class UserSubModuleActivityRightBLL
    {
        public static IEnumerable<VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE> GetUserSubModuleRightByUserIdAndModuleId(Int32 user_Id, Int32 module_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserSubModuleActivityRightDAL.GetUserSubModuleRightByUserIdAndModuleId(user_Id, module_id);
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE GetSubModuleRightByUserIdandSubmoduleId(int userId, int submodule_id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserSubModuleActivityRightDAL.GetSubmoduleRightByUserIdandSubmoduleId(userId, submodule_id);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE GetSubModuleRightById(Int32 Id)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserSubModuleActivityRightDAL.GetSubModuleRightById(Id);
        }

        public static void InsertUpdateUserSubModuleActivityRight(VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserSubModuleActivityRightCBE subModule)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserSubModuleActivityRightDAL.InsertUpdateUserSubModuleActivityRight(subModule);
        }
    }
}
