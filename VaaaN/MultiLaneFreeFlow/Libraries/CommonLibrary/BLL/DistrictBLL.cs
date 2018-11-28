using System;
using System.Collections.Generic;
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
    }
}
