using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class PlazaBLL
    {
        public PlazaBLL()
        {
        }
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.PlazaDAL.Insert(plaza);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.PlazaDAL.Update(plaza);
        }

        public static Libraries.CommonLibrary.CBE.PlazaCollection GetAllAsCollection()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.PlazaDAL.GetAllAsCollection();
        }

        public static List<Libraries.CommonLibrary.CBE.PlazaCBE> GetAllAsList()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.PlazaDAL.GetAllAsList();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE GetPlazaById(CBE.PlazaCBE plaza)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.PlazaDAL.GetPlazaById(plaza);
        }
    }
}
