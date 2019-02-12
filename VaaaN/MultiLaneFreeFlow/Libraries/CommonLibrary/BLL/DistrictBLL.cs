using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class DistrictBLL
    {
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection GetByCityId(Libraries.CommonLibrary.CBE.DistrictCBE district)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.DistrictDAL.GetByCityId(district);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBECollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.DistrictDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE GetDistrictById(CBE.DistrictCBE District)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.DistrictDAL.GetDistrictById(District);
        }
        public static string Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE District)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.DistrictDAL.Insert(District);
        }

        public static string Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.DistrictCBE District)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.DistrictDAL.Update(District);
        }
    }
}
