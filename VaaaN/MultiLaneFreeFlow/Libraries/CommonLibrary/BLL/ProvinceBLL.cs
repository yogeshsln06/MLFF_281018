using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class ProvinceBLL
    {
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBECollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ProvinceDAL.GetAll();
        }
    }
}
