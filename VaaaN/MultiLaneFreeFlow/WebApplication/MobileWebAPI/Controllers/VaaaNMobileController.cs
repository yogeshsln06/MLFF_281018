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
            CustomerVehicleCBE objCustomerVehicleCBE = new CustomerVehicleCBE();
            //int CustomerentryId = 0;
            //try
            //{


            //    #region Data set By Mobile App for Customer Account
            //    objCustomerAccountCBE.ResidentId = objVehicleRegistration.ResidentIdentityNumber;
            //    objCustomerAccountCBE.FirstName = objVehicleRegistration.OwnerName;
            //    objCustomerAccountCBE.AddressLine1 = objVehicleRegistration.OwnerAddress;
            //    objCustomerAccountCBE.MobileNo = objVehicleRegistration.MobilePhoneNumber;
            //    objCustomerAccountCBE.EmailId = objVehicleRegistration.EmailAddress;
            //    objCustomerAccountCBE.ScannedDocsPath1 = objVehicleRegistration.ResidentIdentityImage;
            //    #endregion

            //    #region Data set to Null or empty for Customer Account
            //    objCustomerAccountCBE.TmsId = 1;
            //    objCustomerAccountCBE.LastName = string.Empty;
            //    objCustomerAccountCBE.BirthPlace = string.Empty;
            //    objCustomerAccountCBE.AccountBalance = 0;
            //    objCustomerAccountCBE.AccountId = 0;
            //    objCustomerAccountCBE.AccountStatus = 1;
            //    objCustomerAccountCBE.AddressLine2 = string.Empty;
            //    objCustomerAccountCBE.AddressLine3 = string.Empty;
            //    objCustomerAccountCBE.Balance = 0;
            //    objCustomerAccountCBE.CityId = 0;
            //    objCustomerAccountCBE.CreationDate = DateTime.Now;
            //    objCustomerAccountCBE.CustomerImagePath = string.Empty;
            //    objCustomerAccountCBE.Description = string.Empty;
            //    objCustomerAccountCBE.DistrictId = 0;
            //    objCustomerAccountCBE.Gender = 0;
            //    objCustomerAccountCBE.IsDocVerified = 1;
            //    objCustomerAccountCBE.MaritalStatus = 0;
            //    objCustomerAccountCBE.Nationality = 0;
            //    objCustomerAccountCBE.Occupation = string.Empty;
            //    objCustomerAccountCBE.PostalCode = 0;
            //    objCustomerAccountCBE.ProvinceId = 0;
            //    objCustomerAccountCBE.QueueStatus = 1;
            //    objCustomerAccountCBE.RegistartionThrough = 2;
            //    objCustomerAccountCBE.SubDistrictId = 0;
            //    objCustomerAccountCBE.TransferStatus = 1;
            //    objCustomerAccountCBE.UserPassword = string.Empty;
            //    //objCustomerAccountCBE.ScannedDocsPath1 = string.Empty;
            //    objCustomerAccountCBE.ScannedDocsPath2 = string.Empty;
            //    objCustomerAccountCBE.ScannedDocsPath3 = string.Empty;
            //    objCustomerAccountCBE.ScannedDocsPath4 = string.Empty;
            //    //objCustomerAccountCBE.ValidUntil = 0;
            //    //objCustomerAccountCBE.BirthDate=
            //    #endregion

            //    #region Inster/Update Customer Account
            //    CustomerAccountCBE ValidateCustomerResidentId = CustomerAccountBLL.GetCustomerByResidentId(objCustomerAccountCBE);
            //    if (ValidateCustomerResidentId.ResidentId == 0)
            //    {
            //        CustomerentryId = CustomerAccountBLL.InsertByMobile(objCustomerAccountCBE);
            //        if (CustomerentryId > 0)
            //        {
            //            Log("User Created By Mobile with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString());

            //        }
            //        else
            //        {
            //            Log("Failed to create user with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber);
            //            response = Request.CreateResponse(HttpStatusCode.BadRequest);
            //        }
            //    }
            //    else {
            //        CustomerentryId = ValidateCustomerResidentId.AccountId;
            //        Log("User already exists with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString());
            //    }
            //    #endregion

            //    #region Data set By Mobile App for Customer Vehicle
            //    if (CustomerentryId > 0)
            //    {
            //        objCustomerVehicleCBE.AccountBalance = 0;
            //        objCustomerVehicleCBE.AccountId = CustomerentryId;
            //        objCustomerVehicleCBE.Address = objVehicleRegistration.Address;
            //        objCustomerVehicleCBE.Amount =0;
            //        objCustomerVehicleCBE.Brand = 0;
            //        objCustomerVehicleCBE.CreationDate = DateTime.Now;
            //        objCustomerVehicleCBE.CyclinderCapacity = string.Empty;
            //        objCustomerVehicleCBE.EngineNumber = string.Empty;
            //        objCustomerVehicleCBE.EntryId = 0;
            //        objCustomerVehicleCBE.FrameNumber = string.Empty;
            //        objCustomerVehicleCBE.FrontTID = objVehicleRegistration.TIDFront;
            //        objCustomerVehicleCBE.FuelType = 0;
            //        objCustomerVehicleCBE.IsDocVerified = 0;
            //        objCustomerVehicleCBE.LicencePlateColor = 0;
            //        objCustomerVehicleCBE.LocationCode = string.Empty;
            //        objCustomerVehicleCBE.ManufacturingYear = 0;
            //        objCustomerVehicleCBE.ModelNo = string.Empty;
            //        objCustomerVehicleCBE.QueueStatus = 1;
            //        objCustomerVehicleCBE.RearTID = objVehicleRegistration.TIDRear;
            //        objCustomerVehicleCBE.RegistartionThrough = 2;
            //        objCustomerVehicleCBE.RegistrationYear = 0;
            //        objCustomerVehicleCBE.TagId = string.Empty;
            //        objCustomerVehicleCBE.TMSId = 1;
            //        objCustomerVehicleCBE.TransferStatus = 1;
            //        objCustomerVehicleCBE.ValidUntil = 0;
            //        objCustomerVehicleCBE.VehicleCategory = 0;
            //        objCustomerVehicleCBE.VehicleClassId = objVehicleRegistration.VehicleClassification;
            //        objCustomerVehicleCBE.VehicleClassName = string.Empty;
            //        objCustomerVehicleCBE.VehicleColor = 0;
            //        objCustomerVehicleCBE.VehicleImageFront = objVehicleRegistration.VehicleImageFront;
            //        objCustomerVehicleCBE.VehicleImageLeftSide = objVehicleRegistration.VehicleImageLeft;
            //        objCustomerVehicleCBE.VehicleImageRear = objVehicleRegistration.VehicleImageRear;
            //        objCustomerVehicleCBE.VehicleImageRightSide = objVehicleRegistration.VehicleImageRight;
            //        objCustomerVehicleCBE.VehicleRegistrationCertificateNumber = objVehicleRegistration.VehicleRegistrationCertificateNumber;
            //        objCustomerVehicleCBE.VehicleRegistrationCertificateNumberImagePath = objVehicleRegistration.VehicleRegistrationCertificateImage;
            //        objCustomerVehicleCBE.VehicleType = objVehicleRegistration.VehicleClassification;
            //        objCustomerVehicleCBE.VehRegNo = objVehicleRegistration.VehicleRegistrationNumber;

            //    }
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            //    Log("Failed to create user with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
            //}
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
            public int VehicleClassification { get; set; }
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
