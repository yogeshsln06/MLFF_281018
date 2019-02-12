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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE GetProvinceById(CBE.ProvinceCBE Province)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ProvinceDAL.GetProvinceById(Province);
        }
        public static string Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE Province)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ProvinceDAL.Insert(Province);
        }

        public static string Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.ProvinceCBE Province)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.ProvinceDAL.Update(Province);
        }
    }
}
