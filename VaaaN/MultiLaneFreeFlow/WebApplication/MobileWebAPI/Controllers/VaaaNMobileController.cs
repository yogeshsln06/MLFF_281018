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

namespace MobileWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VaaaNMobileController : ApiController
    {
        HttpResponseMessage response = null;

        #region API for Register Customer
        [Route("VaaaN/IndonesiaMLFFMobileApi/RegisterUser")]
        [HttpPost]
        public HttpResponseMessage RegisterMobileUser(VehicleRegistration objVehicleRegistration)
        {
            CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
            try
            {

                objCustomerAccountCBE.TmsId = 1;
                objCustomerAccountCBE.ResidentId = objVehicleRegistration.ResidentIdentityNumber;
                objCustomerAccountCBE.FirstName = objVehicleRegistration.Name;
                objCustomerAccountCBE.LastName = string.Empty;
                objCustomerAccountCBE.BirthPlace = string.Empty;
                //objCustomerAccountCBE.BirthDate = null;
                objCustomerAccountCBE.AddressLine1 = objVehicleRegistration.Address;


                objCustomerAccountCBE.CreationDate = DateTime.Now;
                objCustomerAccountCBE.AccountStatus = 1;
                objCustomerAccountCBE.TransferStatus = 1;
                objCustomerAccountCBE.RegistartionThrough = 2;
                objCustomerAccountCBE.QueueStatus = 1;
                objCustomerAccountCBE.UserPassword = VaaaN.MLFF.Libraries.CommonLibrary.Cryptography.Encryption.ComputeHash(objCustomerAccountCBE.UserPassword);


                int entryId = CustomerAccountBLL.InsertByMobile(objCustomerAccountCBE);
                if (entryId > 0)
                {
                    Log("User Created By Mobile with Account Id : " + entryId.ToString());
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    Log("Failed to create user with Nobile No : " + objCustomerAccountCBE.MobileNo);
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                Log("Failed to create user with Nobile No : " + objCustomerAccountCBE.MobileNo + " with Exception: " + ex);
            }
            return response;
        }
        #endregion


        #region Helper Method
        public void Log(String ExceptionMsg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(ExceptionMsg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MobileWebAPI);
        }

        public class VehicleRegistration
        {
            public Int32 ResidentIdentityNumber { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string MobilePhoneNumber { get; set; }
            public string EmailAddress { get; set; }
            public string VehicleRegistrationCertificateNumber { get; set; }
            public string VehicleRegistrationNumber { get; set; }
            public string OwnerName { get; set; }
            public string OwnerAddress { get; set; }
            public string VehicleClassification { get; set; }
            public string TIDFront { get; set; }
            public string TIDRear { get; set; }
            public string VehicleImageFront { get; set; }
            public string VehicleImageRear { get; set; }
            public string VehicleImageRight { get; set; }
            public string VehicleImageLeft { get; set; }
            public string VehicleRegistrationCertificateImage { get; set; }
            public string ResidentIdentityImage { get; set; }


        }
        #endregion
    }
}
