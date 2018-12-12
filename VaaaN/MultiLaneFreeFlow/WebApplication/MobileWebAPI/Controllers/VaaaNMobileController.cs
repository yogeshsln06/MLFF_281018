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
            List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
            bool isValidate = true;
            string Messsage = string.Empty;
            int CustomerentryId = 0;
            try
            {
                #region Data set By Mobile App for Customer Account
                objCustomerAccountCBE.ResidentId = objVehicleRegistration.ResidentIdentityNumber;
                objCustomerAccountCBE.FirstName = objVehicleRegistration.Name;
                objCustomerAccountCBE.Address = objVehicleRegistration.Address;
                objCustomerAccountCBE.MobileNo = objVehicleRegistration.MobilePhoneNumber;
                objCustomerAccountCBE.EmailId = objVehicleRegistration.EmailAddress;
                objCustomerAccountCBE.ValidUntil = objVehicleRegistration.ValidUntil;
                objCustomerAccountCBE.ResidentidcardImagePath = objVehicleRegistration.ResidentIdentityImage;
                if (!string.IsNullOrEmpty(objCustomerAccountCBE.MobileNo))
                {
                    objCustomerAccountCBE.MobileNo = VaaaN.MLFF.Libraries.CommonLibrary.Constants.MobileNoPrefix(objCustomerAccountCBE.MobileNo.Trim());

                }
                #endregion

                #region Data set to Null or empty for Customer Account
                objCustomerAccountCBE.TmsId = 1;
                objCustomerAccountCBE.LastName = string.Empty;
                objCustomerAccountCBE.BirthPlace = string.Empty;
                objCustomerAccountCBE.BirthDate = null;
                objCustomerAccountCBE.AccountBalance = 0;
                objCustomerAccountCBE.AccountId = 0;
                objCustomerAccountCBE.AccountStatus = 1;
                objCustomerAccountCBE.Balance = 0;
                objCustomerAccountCBE.CityId = 0;
                objCustomerAccountCBE.CreationDate = DateTime.Now;
                objCustomerAccountCBE.CustomerImagePath = string.Empty;
                objCustomerAccountCBE.Description = string.Empty;
                objCustomerAccountCBE.DistrictId = 0;
                objCustomerAccountCBE.Gender = 0;
                objCustomerAccountCBE.IsDocVerified = 1;
                objCustomerAccountCBE.MaritalStatus = 0;
                objCustomerAccountCBE.Nationality = 0;
                objCustomerAccountCBE.Occupation = string.Empty;
                objCustomerAccountCBE.PostalCode = 0;
                objCustomerAccountCBE.ProvinceId = 0;
                objCustomerAccountCBE.RegistartionThrough = 2;
                objCustomerAccountCBE.SubDistrictId = 0;
                objCustomerAccountCBE.TransferStatus = 1;
                objCustomerAccountCBE.UserPassword = string.Empty;
                #endregion

                #region Data set By Mobile App for Customer vehicle
                objCustomerVehicleCBE.AccountBalance = 0;
               
                objCustomerVehicleCBE.VehicleRCNumber = objVehicleRegistration.VehicleRegistrationCertificateNumber;
                objCustomerVehicleCBE.VehRegNo = objVehicleRegistration.VehicleRegistrationNumber;
                objCustomerVehicleCBE.OwnerName = objVehicleRegistration.OwnerName;
                objCustomerVehicleCBE.OwnerAddress = objVehicleRegistration.OwnerAddress;
                objCustomerVehicleCBE.VehicleImageFront = objVehicleRegistration.VehicleImageFront;
                objCustomerVehicleCBE.VehicleImageRear = objVehicleRegistration.VehicleImageRear;
                objCustomerVehicleCBE.VehicleImageLeft = objVehicleRegistration.VehicleImageLeft;
                objCustomerVehicleCBE.VehicleImageRight = objVehicleRegistration.VehicleImageRight;
                objCustomerVehicleCBE.VehicleRCNumberImagePath = objVehicleRegistration.VehicleRegistrationCertificateImage;
                objCustomerVehicleCBE.TidFront = objVehicleRegistration.TIDFront;
                objCustomerVehicleCBE.TidRear = objVehicleRegistration.TIDRear;
                objCustomerVehicleCBE.TagId = objVehicleRegistration.EPC;
                objCustomerVehicleCBE.VehicleClassId = objVehicleRegistration.VehicleClassification;
                #endregion

                #region Data set to Null or empty for Customer vehicle
                objCustomerVehicleCBE.AccountBalance = 0;
                objCustomerVehicleCBE.QueueStatus = 1;
                objCustomerVehicleCBE.CreationDate = DateTime.Now;
                objCustomerVehicleCBE.CyclinderCapacity = string.Empty;
                objCustomerVehicleCBE.EngineNumber = string.Empty;
                objCustomerVehicleCBE.EntryId = 0;
                objCustomerVehicleCBE.FrameNumber = string.Empty;
                objCustomerVehicleCBE.FuelType = 0;
                objCustomerVehicleCBE.IsDocVerified = 0;
                objCustomerVehicleCBE.LicencePlateColor = 0;
                objCustomerVehicleCBE.LocationCode = string.Empty;
                objCustomerVehicleCBE.ManufacturingYear = 0;
                objCustomerVehicleCBE.RegistartionThrough = 2;
                objCustomerVehicleCBE.RegistrationYear = 0;
                objCustomerVehicleCBE.TMSId = 1;
                objCustomerVehicleCBE.TransferStatus = 1;
                #endregion


                #region Inster/Update Customer Account and Vehicle
                try
                {
                    CustomerAccountCBE CustomerAccountByResident = CustomerAccountBLL.GetCustomerByResidentId(objCustomerAccountCBE);
                    if (CustomerAccountByResident.AccountId > 0)
                    {
                        CustomerentryId = CustomerAccountByResident.AccountId;
                        Log("Custmer Account Found for Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString());
                    }
                    else {
                        #region Validate Custmer Account
                        customerDataList = CustomerAccountBLL.GetAllAsList();
                        List<CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == objCustomerAccountCBE.MobileNo.ToString());
                        List<CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == objCustomerAccountCBE.EmailId.ToString().Trim());
                        List<CustomerAccountCBE> Registrationfiltered = customerDataList.FindAll(x => x.ResidentId == objCustomerAccountCBE.ResidentId.Trim());
                        if (Registrationfiltered.Count > 0)
                        {
                            isValidate = false;
                            Messsage = "eKTP already exists";
                            Log("eKTP already exists");
                            response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                        }
                        else if (Mobilefiltered.Count > 0)
                        {
                            isValidate = false;
                            Messsage = "Mobile Number already exists";
                            Log("Mobile Number already exists");
                            response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                        }
                        else if (Emailfiltered.Count > 0)
                        {
                            isValidate = false;
                            Messsage = "Email Id already exists";
                            Log("Email Id already exists");
                            response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                        }

                        #endregion

                        #region Validate Custmer Vehicle 
                        List<CustomerVehicleCBE> customerVehicleDataList = new List<CustomerVehicleCBE>();
                        customerVehicleDataList = CustomerVehicleBLL.GetAllAsList();
                        List<CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim().ToLower());
                        List<CustomerVehicleCBE> TagIdfiltered = customerVehicleDataList.FindAll(x => x.TagId == objVehicleRegistration.EPC.Trim().ToString());
                        if (VehRegNofiltered.Count > 0)
                        {
                            isValidate = false;
                            Messsage = "Vehicle Registration Number already exists";
                            Log("Vehicle Registration Number already exists");
                            response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                        }
                        else if (TagIdfiltered.Count > 0)
                        {
                            isValidate = false;
                            Messsage = "EPC already exists";
                            Log("EPC already exists");
                            response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                        }
                        #endregion

                        if (isValidate)
                        {
                            Log("Custmer Account not Found for Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString() + " but all validation done");
                            CustomerentryId = CustomerAccountBLL.Insert(objCustomerAccountCBE);
                            if (CustomerentryId > 0)
                            {
                                Log("User Created By with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString());
                                objCustomerVehicleCBE.AccountId = CustomerentryId;
                               
                                #region Data process for Customer Vehicle
                                int VehicleEntryId = CustomerVehicleBLL.Insert(objCustomerVehicleCBE);
                                if (VehicleEntryId > 0)
                                {
                                    Messsage = "success";
                                    Log("Vehicle register successfully with VRN : " + objCustomerVehicleCBE.VehRegNo);
                                    response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                                }
                                else {
                                    Messsage = "failed";
                                    Log("Failed to create user vehicle with VRN : " + objCustomerVehicleCBE.VehRegNo);
                                    response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                                }
                                #endregion
                            }
                            else
                            {
                                Log("Failed to create user vehicle with VRN : " + objVehicleRegistration.VehicleRegistrationNumber);
                                Messsage = "failed";
                                response = Request.CreateResponse(HttpStatusCode.OK, Messsage);
                            }
                        }
                        else {
                            Log("Custmer Account not Found for Resident Id : " + objVehicleRegistration.ResidentIdentityNumber.ToString() + " but all validation not done " + Messsage);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Messsage = "failed";
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, Messsage);
                    Log("Failed to create user with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                }
                #endregion


            }
            catch (Exception ex)
            {
                Messsage = "failed";
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Messsage);
                Log("Failed to create user with Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
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
            public String ResidentIdentityNumber { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string MobilePhoneNumber { get; set; }
            public string EmailAddress { get; set; }

            public Nullable<DateTime> ValidUntil;
            public string VehicleRegistrationCertificateNumber { get; set; }
            public string VehicleRegistrationNumber { get; set; }
            public string OwnerName { get; set; }
            public string OwnerAddress { get; set; }
            public int VehicleClassification { get; set; }
            public string TIDFront { get; set; }
            public string TIDRear { get; set; }
            public string EPC { get; set; }
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
