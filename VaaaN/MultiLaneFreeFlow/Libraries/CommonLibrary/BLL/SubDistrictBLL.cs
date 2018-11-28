using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class SubDistrictBLL
    {
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection GetByDistrictId(Libraries.CommonLibrary.CBE.SubDistrictCBE subdistrict)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.GetByDistrictId(subdistrict);
        }
    }
}
