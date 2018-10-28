using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.DAL
{
    public class LoginDAL
    {
        public static CBE.UserCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection GetUserByLoginName(String UserName)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.UserDAL.GetUserByLoginName(UserName);
        }
    }
}
