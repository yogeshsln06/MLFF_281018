using System;
using System.Collections.Generic;
using System.Data;
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBECollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.GetAll();
        }
        public static DataTable GetAll_DT()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.GetAll_DT();
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE GetCityById(CBE.CityCBE city)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.GetCityById(city);
        }
        public static string Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE city)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.Insert(city);
        }

        public static string Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CityCBE city)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CityDAL.Update(city);
        }
    }
}
