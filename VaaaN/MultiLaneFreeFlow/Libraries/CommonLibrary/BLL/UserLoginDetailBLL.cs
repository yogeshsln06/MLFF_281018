using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    /// <summary>
    /// Summary description for DirectionBLL
    /// </summary>
    public class UserLoginDetailBLL
    {
        public UserLoginDetailBLL()
        {
        }

    
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserLoginDetailCollection GetAll()
        {
            //return VaaaN.TollMax.Library.DAL.UserLoginDetailDAL.GetAll(tmsId, plazaId);
            return new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserLoginDetailCollection();
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserLoginDetailCollection GetFilteredLoginDetail(string filter)
        {
            //return VaaaN.TollMax.Library.DAL.UserLoginDetailDAL.GetFilteredLoginDetail(filter);
            return new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserLoginDetailCollection();
        }
    }
}
