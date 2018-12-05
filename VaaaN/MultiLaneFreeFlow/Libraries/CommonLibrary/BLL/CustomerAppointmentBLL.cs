using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class CustomerAppointmentBLL
    {
        public CustomerAppointmentBLL()
        {
        }

        public static Int32 Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAppointmentDAL.Insert(customerAppointment);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAppointmentDAL.Update(customerAppointment);
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAppointmentDAL.GetAllCustomerAppointment();
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE GetCustomerAppointmentById(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAppointmentDAL.GetCustomerAppointmentById(customerAppointment);
        }

        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCollection GetCustomerAppointmentByAccountId(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAppointmentCBE customerAppointment)
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerAppointmentDAL.GetCustomerAppointmentByAccountId(customerAppointment);
        }
    }
}
