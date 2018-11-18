using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.BLL
{
    public class CustomerQueueBLL
    {
        public CustomerQueueBLL()
        {
        }

        public static void Insert(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE customerQueue)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerQueueDAL.Insert(customerQueue);
        }

        public static void Update(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCBE customerQueue)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerQueueDAL.Update(customerQueue);
        }


        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerQueueCollection GetAll()
        {
            return VaaaN.MLFF.Libraries.CommonLibrary.DAL.CustomerQueueDAL.GetAllCustomerQueue();
        }
    }
}
