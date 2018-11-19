using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class VehicleClassMappingBLL
    {
        public VehicleClassMappingBLL()
        {
        }
        public static int Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE vehicleClass)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.Insert(vehicleClass);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE vehicleClass)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.Update(vehicleClass);
        }

        public static List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE> GetAllAsList()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetAllAsList();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection GetAllAsCollection()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetAllAsCollection();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE GetVehicleClassMappingById(CBE.VehicleClassMappingCBE vehicleClassCBE)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetVehicleClassMappingById(vehicleClassCBE);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection GetVehicleClassMappingByIdCollection(CBE.VehicleClassMappingCBE vehicleClassCBE)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetVehicleClassMappingByIdCollection(vehicleClassCBE);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCBE GetVehicleClassByANPRName(CBE.VehicleClassMappingCBE vehicleClassCBE)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetVehicleClassByANPRName(vehicleClassCBE);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassMappingCollection GetVehicleClassByANPRNameCollection(CBE.VehicleClassMappingCBE vehicleClassCBE)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.VehicleClassMappingDAL.GetVehicleClassByANPRNameCollection(vehicleClassCBE);
        }


    }


}
