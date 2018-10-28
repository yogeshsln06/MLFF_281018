using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class VehicleClassBLL
    {
        public VehicleClassBLL()
        {
        }
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.Insert(vehicleClass);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicleClass)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.Update(vehicleClass);
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE> GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.GetAll();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection GetAllAsCollection()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.GetAllAsCollection();
        }

        public static Int32 GetVehicleByName(string VehicleName)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.GetVehicleByName(VehicleName);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE GetVehicleClassId(CBE.VehicleClassCBE vehicleClassCBE)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassDAL.GetVehicleClassId(vehicleClassCBE);
        }


    }


}
