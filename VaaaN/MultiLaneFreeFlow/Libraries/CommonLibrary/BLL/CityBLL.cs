using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class CityBLL
    {
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection GetByProvinceId(Libraries.CommonLibrary.CBE.CityCBE city)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.GetByProvinceId(city);
        }
    }
}
