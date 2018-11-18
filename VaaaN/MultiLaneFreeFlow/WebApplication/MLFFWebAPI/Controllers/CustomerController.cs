using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;

namespace MLFFWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("VaaaN/MLFFApi")]

    public class CustomerController : ApiController
    {
        HttpResponseMessage response = null;

        public HttpResponseMessage RegisterCustomerQueue(CustomerQueueCBE customerQueue)
        {
            if (customerQueue != null)
            {
                //Call BLL
                CustomerQueueBLL.Insert(customerQueue);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            return response;
        }

    }
}
