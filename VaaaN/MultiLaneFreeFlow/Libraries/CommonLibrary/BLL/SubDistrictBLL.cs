using System;
using System.Collections.Generic;
using System.Data;
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

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBECollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.GetAll();
        }
        public static DataTable GetAll_DT()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.GetAll_DT();
        }
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE GetSubDistrictById(CBE.SubDistrictCBE SubDistrict)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.GetSubDistrictById(SubDistrict);
        }
        public static string Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE SubDistrict)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.Insert(SubDistrict);
        }

        public static string Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubDistrictCBE SubDistrict)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.SubDistrictDAL.Update(SubDistrict);
        }
    }
}
