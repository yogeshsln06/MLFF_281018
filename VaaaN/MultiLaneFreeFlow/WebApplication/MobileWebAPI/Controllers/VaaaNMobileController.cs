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
using MobileWebAPI.Models;
using VaaaN.MLFF.Libraries.CommonLibrary;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace MobileWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VaaaNMobileController : ApiController
    {
        #region Globle variable 
        string sJSONResponse = string.Empty;
        string customerImageName = string.Empty;
        DataTable dt = new DataTable();
        CustomerAccountCBE objCustomerAccountCBE = new CustomerAccountCBE();
        CustomerVehicleCBE objCustomerVehicleCBE = new CustomerVehicleCBE();
        ResponseMessage objResponse = new ResponseMessage();
        List<ModelStateList> objResponseMessage = new List<ModelStateList>();

        #endregion


        #region API for Register Customer
        [Route("VaaaN/IndonesiaMLFFMobileApi/RegisterCustomer")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage RegisterMobileCustomer(CustomerDataProcess objVehicleRegistration)
        {
            if (ModelState.IsValid)
            {
                string CustomerFilepath = Constants.EventPath;

                List<CustomerAccountCBE> customerDataList = new List<CustomerAccountCBE>();
                CustomerRegistrationResponce objCustomerRegistrationResponce = new CustomerRegistrationResponce();
                int CustomerentryId = 0;
                try
                {
                    #region Change the Mobile format
                    if (!string.IsNullOrEmpty(objVehicleRegistration.MobilePhoneNumber))
                    {
                        objVehicleRegistration.MobilePhoneNumber = Constants.MobileNoPrefix(objVehicleRegistration.MobilePhoneNumber.Trim());

                    }
                    #endregion

                    #region Validate Customer and process customer data
                    objCustomerAccountCBE.ResidentId = objVehicleRegistration.ResidentIdentityNumber.Trim();
                    objCustomerAccountCBE.MobileNo = objVehicleRegistration.MobilePhoneNumber.Trim();
                    objCustomerAccountCBE.EmailId = objVehicleRegistration.EmailAddress.Trim();
                    List<CustomerAccountCBE> CustomerAccountByResident = CustomerAccountBLL.ValidateCustomerAccount(objCustomerAccountCBE);
                    if (CustomerAccountByResident.Count > 0)
                    {
                        CustomerentryId = CustomerAccountByResident[0].AccountId;
                        #region Validate Custmer Vehicle 
                        List<CustomerVehicleCBE> customerVehicleDataList = new List<CustomerVehicleCBE>();
                        customerVehicleDataList = CustomerVehicleBLL.GetAllAsList();
                        List<CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim().ToLower());
                        if (VehRegNofiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Vehicle Registration Number already exists.";
                            objResponseMessage.Add(objModelState);
                        }

                        #region Validate Vehicle Front
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Front_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageFront = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageFront, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            Log("Unable to save vehicle image front of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            objModelState.ErrorMessage = "Invalid vehicle image front.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Rear
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Rear_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageRear = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageRear, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image rear of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image rear.";
                            objResponseMessage.Add(objModelState);
                        }
                        #endregion

                        #region Validate Vehicle Left
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Left_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageLeft = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageLeft, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image Left of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image left.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Right
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Right_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageRight = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageRight, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image Right of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image right.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Registration Certificate Image
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleRegistrationCertificateImage = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleRegistrationCertificateImage, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle registration certificate image of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle registration certificate image.";
                            objResponseMessage.Add(objModelState);
                        }
                        #endregion

                        #endregion
                        if (objResponseMessage.Count > 0)
                        {
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse.Replace("[", "").Replace("]", ""));
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                        else
                        {
                            #region Save Customer Vehicle Data
                            objCustomerVehicleCBE.AccountId = CustomerentryId;
                            sJSONResponse = SaveVehicleData(objVehicleRegistration, objCustomerVehicleCBE);
                            if (sJSONResponse.Contains("Somthing went wrong"))
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                            else
                                return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                            #endregion
                        }
                    }
                    else
                    {
                        #region Cutomer Not Cound Need to registor
                        #region Validate Customer Account
                        customerDataList = CustomerAccountBLL.GetAllAsList();
                        List<CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == objCustomerAccountCBE.MobileNo.ToString());
                        List<CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == objCustomerAccountCBE.EmailId.ToString().Trim());
                        List<CustomerAccountCBE> Registrationfiltered = customerDataList.FindAll(x => x.ResidentId == objCustomerAccountCBE.ResidentId.Trim());

                        #region Validate Mobile Resident Id and Email Id
                        if (Registrationfiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Identity Card already exists.";
                            objResponseMessage.Add(objModelState);
                        }
                        if (Mobilefiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Mobile phone number already exists.";
                            objResponseMessage.Add(objModelState);
                        }
                        if (Emailfiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Email address already exists.";
                            objResponseMessage.Add(objModelState);
                        }
                        #endregion

                        #region Validate Image
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"Customer\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.ResidentIdentityNumber.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.ResidentIdentityCardImage = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.ResidentIdentityCardImage, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save resident identity card image of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid identity card image.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #endregion

                        #region Validate Custmer Vehicle 
                        List<CustomerVehicleCBE> customerVehicleDataList = new List<CustomerVehicleCBE>();
                        customerVehicleDataList = CustomerVehicleBLL.GetAllAsList();
                        List<CustomerVehicleCBE> VehRegNofiltered = customerVehicleDataList.FindAll(x => x.VehRegNo.ToLower() == objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim().ToLower());
                        if (VehRegNofiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Vehicle Registration Number already exists.";
                            objResponseMessage.Add(objModelState);
                        }

                        #region Validate Vehicle Front
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Front_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageFront = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageFront, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image front of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image front.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Rear
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Rear_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageRear = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageRear, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image rear of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image rear.";
                            objResponseMessage.Add(objModelState);
                        }
                        #endregion

                        #region Validate Vehicle Left
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Left_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageLeft = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageLeft, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image Left of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image left.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Right
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Right_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleImageRight = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleImageRight, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle image Right of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle image right.";
                            objResponseMessage.Add(objModelState);

                        }
                        #endregion

                        #region Validate Vehicle Registration Certificate Image
                        try
                        {
                            CustomerFilepath = Constants.CustomerImagePath + @"VehicleImage\";
                            if (!Directory.Exists(CustomerFilepath))
                            {
                                Directory.CreateDirectory(CustomerFilepath);
                            }
                            customerImageName = objVehicleRegistration.VehicleRegistrationNumber.ToString().Trim() + "_Document_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".jpeg";
                            objVehicleRegistration.VehicleRegistrationCertificateImage = Constants.SaveByteArrayAsImage(CustomerFilepath + customerImageName, objVehicleRegistration.VehicleRegistrationCertificateImage, customerImageName);
                        }
                        catch (Exception ex)
                        {
                            Log("Unable to save vehicle registration certificate image of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid vehicle registration certificate image.";
                            objResponseMessage.Add(objModelState);
                        }
                        #endregion

                        #endregion

                        if (objResponseMessage.Count > 0)
                        {
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                        else
                        {
                            #region Data set By Mobile App for Customer Account
                            objCustomerAccountCBE.FirstName = objVehicleRegistration.CustomerName.Trim();
                            objCustomerAccountCBE.Address = objVehicleRegistration.Address.Trim();
                            objCustomerAccountCBE.ResidentidcardImagePath = objVehicleRegistration.ResidentIdentityCardImage;
                            objCustomerAccountCBE.TmsId = 1;
                            objCustomerAccountCBE.TransferStatus = 1;
                            objCustomerAccountCBE.AccountId = 0;
                            objCustomerAccountCBE.AccountStatus = 1;
                            objCustomerAccountCBE.CreationDate = DateTime.Now;
                            objCustomerAccountCBE.IsDocVerified = 1;
                            objCustomerAccountCBE.RegistartionThrough = 2;
                            #endregion

                            #region Insert Customer Data
                            CustomerentryId = CustomerAccountBLL.Insert(objCustomerAccountCBE);
                            #endregion

                            #region Save Customer Vehicle Data
                            objCustomerVehicleCBE.AccountId = CustomerentryId;
                            sJSONResponse = SaveVehicleData(objVehicleRegistration, objCustomerVehicleCBE);
                            if (sJSONResponse.Contains("Somthing went wrong"))
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                            else
                                return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                            #endregion
                        }
                        #endregion
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Log("Exception in Transaction History Summary. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Somthing went wrong.";
                    objResponseMessage.Add(objModelState);
                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public static string SaveVehicleData(CustomerDataProcess objVehicleRegistration, CustomerVehicleCBE objCustomerVehicleCBE)
        {
            string sJSONResponse = string.Empty;
            int CustomerVehicleentryId = 0;
            CustomerRegistrationResponce objCustomerRegistrationResponce = new CustomerRegistrationResponce();
            List<ModelStateList> objResponseMessage = new List<ModelStateList>();
            ModelStateList objModelState = new ModelStateList();
            #region Register Customer Vehicle
            try
            {
                if (objCustomerVehicleCBE.AccountId > 0)
                {
                    objCustomerVehicleCBE.TMSId = 1;
                    objCustomerVehicleCBE.AccountBalance = 0;
                    objCustomerVehicleCBE.EntryId = 0;
                    objCustomerVehicleCBE.TransferStatus = 1;
                    objCustomerVehicleCBE.QueueStatus = 1;
                    objCustomerVehicleCBE.RegistartionThrough = 2;
                    objCustomerVehicleCBE.CreationDate = DateTime.Now;
                    objCustomerVehicleCBE.IsDocVerified = 1;
                    objCustomerVehicleCBE.OwnerAddress = objVehicleRegistration.OwnerAddress.Trim();
                    objCustomerVehicleCBE.OwnerName = objVehicleRegistration.OwnerName.Trim();
                    objCustomerVehicleCBE.TidFront = objVehicleRegistration.TIDFront.Trim();
                    objCustomerVehicleCBE.TidRear = objVehicleRegistration.TIDRear.Trim();
                    objCustomerVehicleCBE.VehicleClassId = objVehicleRegistration.VehicleClassification;
                    objCustomerVehicleCBE.VehicleImageFront = objVehicleRegistration.VehicleImageFront;
                    objCustomerVehicleCBE.VehicleImageLeft = objVehicleRegistration.VehicleImageLeft;
                    objCustomerVehicleCBE.VehicleImageRear = objVehicleRegistration.VehicleImageRear;
                    objCustomerVehicleCBE.VehicleImageRight = objVehicleRegistration.VehicleImageRight;
                    objCustomerVehicleCBE.VehicleRCNumber = objVehicleRegistration.VehicleRegistrationCertificateNumber.Trim();
                    objCustomerVehicleCBE.VehicleRCNumberImagePath = objVehicleRegistration.VehicleRegistrationCertificateImage;
                    objCustomerVehicleCBE.VehRegNo = objVehicleRegistration.VehicleRegistrationNumber.Trim();
                    objCustomerVehicleCBE.TagId = Constants.VRNToByte(objVehicleRegistration.VehicleClassification, objVehicleRegistration.VehicleRegistrationNumber.Trim());

                    #region Insert Customer Vehicle
                    CustomerVehicleentryId = CustomerVehicleBLL.Insert(objCustomerVehicleCBE);
                    #endregion

                    if (CustomerVehicleentryId > 0)
                    {
                        objCustomerRegistrationResponce.CustomerAccountId = objCustomerVehicleCBE.AccountId;
                        objCustomerRegistrationResponce.CustomerVehicleId = CustomerVehicleentryId;
                        objCustomerRegistrationResponce.EPCNumber = objCustomerVehicleCBE.TagId;
                        objCustomerRegistrationResponce.ResidentIdentityNumber = objVehicleRegistration.ResidentIdentityNumber.Trim();
                        objCustomerRegistrationResponce.VehicleRegistrationCertificateNumber = objVehicleRegistration.VehicleRegistrationCertificateNumber.Trim();
                        objCustomerRegistrationResponce.VehicleRegistrationNumber = objVehicleRegistration.VehicleRegistrationNumber.Trim();
                        sJSONResponse = JsonConvert.SerializeObject(objCustomerRegistrationResponce);
                    }
                    else
                    {
                        objModelState.ErrorMessage = "Somthing went wrong.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    }

                }
                else {
                    objModelState.ErrorMessage = "Somthing went wrong.";
                    objResponseMessage.Add(objModelState);
                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sJSONResponse;
            #endregion
        }
        #endregion

        #region API for Customer Vehicle Recharge
        [Route("VaaaN/IndonesiaMLFFMobileApi/TopUPVehcileAccount")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage TOPUPBalance(CustomerVehicleInformationforTopup objCustomerVehicleInformation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    objCustomerVehicleCBE.VehicleRCNumber = objCustomerVehicleInformation.VehicleRegistrationCertificateNumber;
                    objCustomerVehicleCBE.VehRegNo = objCustomerVehicleInformation.VehicleRegistrationNumber;
                    dt = CustomerVehicleBLL.ValidateCustomerVehicleDetails(objCustomerVehicleCBE, objCustomerVehicleInformation.ResidentIdentityNumber);
                    if (dt.Rows.Count > 0)
                    {

                        try
                        {
                            #region Update account balance in database
                            objCustomerVehicleCBE.AccountBalance += objCustomerVehicleInformation.TopUpAmount;
                            objCustomerVehicleCBE.ModificationDate = DateTime.Now;
                            objCustomerVehicleCBE.EntryId = Convert.ToInt32(dt.Rows[0]["ENTRY_ID"].ToString());
                            objCustomerVehicleCBE.VehRegNo = objCustomerVehicleInformation.VehicleRegistrationNumber;
                            objCustomerVehicleCBE.AccountId = Convert.ToInt32(dt.Rows[0]["ACCOUNT_ID"].ToString());
                            objCustomerVehicleCBE.TMSId = Convert.ToInt32(dt.Rows[0]["TMS_ID"].ToString());
                            objCustomerVehicleCBE.AccountBalance = CustomerVehicleBLL.UpdateVehiclebalance(objCustomerVehicleCBE, objCustomerVehicleInformation.TopUpAmount);
                            #endregion

                            #region Make Transcation of TOP UP
                            Int32 entryId = 0;
                            DateTime transcationDateTime = DateTime.Now;
                            AccountHistoryCBE accountHistory = new AccountHistoryCBE();
                            accountHistory.TMSId = objCustomerVehicleCBE.TMSId;
                            accountHistory.AccountId = Convert.ToInt32(dt.Rows[0]["ACCOUNT_ID"].ToString());
                            accountHistory.CustomerVehicleEntryId = objCustomerVehicleCBE.EntryId;
                            accountHistory.TransactionTypeId = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransactionType.Recharge;
                            accountHistory.TransactionId = 0;
                            accountHistory.Amount = objCustomerVehicleInformation.TopUpAmount;
                            accountHistory.IsSMSSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                            accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent;
                            accountHistory.CreationDate = transcationDateTime;
                            accountHistory.ModificationDate = transcationDateTime;
                            accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                            entryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "successful.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in TOP Up. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Somthing went wrong.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "No customer account found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in TOP Up. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Somthing went wrong.";
                    objResponseMessage.Add(objModelState);
                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        #endregion

        #region API for Customer Vehicle Details
        [Route("VaaaN/IndonesiaMLFFMobileApi/InquiryCustomerVehicleDetail")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage VehicleDetails(CustomerVehicleInformation objCustomerVehicleInformation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    objCustomerVehicleCBE.VehicleRCNumber = objCustomerVehicleInformation.VehicleRegistrationCertificateNumber;
                    objCustomerVehicleCBE.VehRegNo = objCustomerVehicleInformation.VehicleRegistrationNumber;
                    dt = CustomerVehicleBLL.ValidateCustomerVehicleDetails(objCustomerVehicleCBE, objCustomerVehicleInformation.ResidentIdentityNumber);
                    if (dt.Rows.Count > 0)
                    {

                        try
                        {
                            CustomerVehicleDetails objCustomerVehicleDetails = new CustomerVehicleDetails();
                            objCustomerVehicleDetails.Balance = Convert.ToDecimal(dt.Rows[0]["ACCOUNT_BALANCE"].ToString());
                            objCustomerVehicleDetails.Brand = dt.Rows[0]["BRAND"].ToString();
                            objCustomerVehicleDetails.Category = dt.Rows[0]["VEHICLE_CATEGORY"].ToString();
                            objCustomerVehicleDetails.Classification = Convert.ToInt32(dt.Rows[0]["VEHICLE_CLASS_ID"].ToString());
                            objCustomerVehicleDetails.Color = dt.Rows[0]["VEHICLE_COLOR"].ToString();
                            objCustomerVehicleDetails.CylinderCapacity = dt.Rows[0]["CYCLINDER_CAPACITY"].ToString();
                            objCustomerVehicleDetails.EngineNumber = dt.Rows[0]["ENGINE_NUMBER"].ToString();
                            objCustomerVehicleDetails.ExceptionFlag = dt.Rows[0]["EXCEPTION_FLAG_NAME"].ToString();
                            objCustomerVehicleDetails.FrameNumber = dt.Rows[0]["FRAME_NUMBER"].ToString();
                            objCustomerVehicleDetails.FuelType = dt.Rows[0]["FUEL_TYPE_NAME"].ToString();
                            objCustomerVehicleDetails.LicensePlateColor = dt.Rows[0]["LICENCE_PLATE_COLOR_NAME"].ToString();
                            objCustomerVehicleDetails.LocationCode = dt.Rows[0]["LOCATION_CODE"].ToString();
                            objCustomerVehicleDetails.ManufactureYear = Convert.ToInt32(dt.Rows[0]["MANUFACTURING_YEAR"].ToString());
                            objCustomerVehicleDetails.Model = dt.Rows[0]["MODEL_NO"].ToString();
                            objCustomerVehicleDetails.OwnerAddress = dt.Rows[0]["OWNER_ADDRESS"].ToString();
                            objCustomerVehicleDetails.OwnerName = dt.Rows[0]["OWNER_NAME"].ToString();
                            objCustomerVehicleDetails.RegistrationQueueNumber = dt.Rows[0]["REG_QUEUE_NO"].ToString();
                            objCustomerVehicleDetails.RegistrationYear = Convert.ToInt32(dt.Rows[0]["REGISTRATION_YEAR"].ToString());
                            objCustomerVehicleDetails.ResidentIdentityNumber = dt.Rows[0]["RESIDENT_ID"].ToString();
                            objCustomerVehicleDetails.Status = dt.Rows[0]["QUEUE_STATUS"].ToString();
                            objCustomerVehicleDetails.TIDFront = dt.Rows[0]["TID_FRONT"].ToString();
                            objCustomerVehicleDetails.TIDRear = dt.Rows[0]["TID_REAR"].ToString();
                            objCustomerVehicleDetails.Type = dt.Rows[0]["VEHICLE_TYPE"].ToString();
                            objCustomerVehicleDetails.ValidUntil = dt.Rows[0]["VALID_UNTIL"].ToString();
                            if (!string.IsNullOrEmpty(objCustomerVehicleDetails.ValidUntil))
                            {
                                objCustomerVehicleDetails.ValidUntil = Convert.ToDateTime(objCustomerVehicleDetails.ValidUntil).ToString("yyyy-MM-dd");
                            }
                            objCustomerVehicleDetails.VehicleImageFront = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + dt.Rows[0]["VEHICLEIMAGE_FRONT"].ToString();
                            objCustomerVehicleDetails.VehicleImageLeft = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + dt.Rows[0]["VEHICLEIMAGE_LEFT"].ToString();
                            objCustomerVehicleDetails.VehicleImageRear = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + dt.Rows[0]["VEHICLEIMAGE_REAR"].ToString();
                            objCustomerVehicleDetails.VehicleImageRight = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + dt.Rows[0]["VEHICLEIMAGE_RIGHT"].ToString();
                            objCustomerVehicleDetails.VehicleOwnershipDocumentNumber = dt.Rows[0]["VEHICLE_OWNERSHIP_NO"].ToString();
                            objCustomerVehicleDetails.VehicleRegistrationCertificateImage = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + dt.Rows[0]["VEHICLE_RC_NO_PATH"].ToString();
                            objCustomerVehicleDetails.VehicleRegistrationCertificateNumber = dt.Rows[0]["VEHICLE_RC_NO"].ToString();
                            objCustomerVehicleDetails.VehicleRegistrationNumber = dt.Rows[0]["VEH_REG_NO"].ToString();
                            sJSONResponse = JsonConvert.SerializeObject(objCustomerVehicleDetails);
                            return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Somthing went wrong.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "No Customer account found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Somthing went wrong.";
                    objResponseMessage.Add(objModelState);
                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        #endregion

        #region API for Customer Vehicle Transaction History Summary
        [Route("VaaaN/IndonesiaMLFFMobileApi/InquiryCustomerVehicleTransactionHistorySummary")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage TransactionHistorySummary(CustomerVehicleInformation objCustomerVehicleInformation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    objCustomerVehicleCBE.VehicleRCNumber = objCustomerVehicleInformation.VehicleRegistrationCertificateNumber;
                    objCustomerVehicleCBE.VehRegNo = objCustomerVehicleInformation.VehicleRegistrationNumber;
                    dt = CustomerVehicleBLL.ValidateCustomerVehicleDetails(objCustomerVehicleCBE, objCustomerVehicleInformation.ResidentIdentityNumber);

                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            dt = AccountHistoryBLL.AccountHistoryByVehicle(objCustomerVehicleInformation.ResidentIdentityNumber, objCustomerVehicleInformation.VehicleRegistrationCertificateNumber, objCustomerVehicleInformation.VehicleRegistrationNumber);
                            if (dt.Rows.Count > 0)
                            {
                                try
                                {
                                    List<VehicleTransactionHistorySummary> objVehicleTransactionHistorySummaryList = new List<VehicleTransactionHistorySummary>();

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        VehicleTransactionHistorySummary objVehicleTransactionHistorySummary = new VehicleTransactionHistorySummary();

                                        if (dt.Rows[i]["RESIDENT_ID"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.ResidentIdentityNumber = Convert.ToString(dt.Rows[i]["RESIDENT_ID"].ToString());

                                        if (dt.Rows[i]["VEH_REG_NO"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.VehicleRegistrationNumber = dt.Rows[i]["VEH_REG_NO"].ToString();

                                        if (dt.Rows[i]["VEHICLE_RC_NO"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.VehicleRegistrationCertificateNumber = dt.Rows[i]["VEHICLE_RC_NO"].ToString();

                                        if (dt.Rows[i]["TRANSACTION_ID"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.TransactionID = Convert.ToInt32(dt.Rows[i]["TRANSACTION_ID"].ToString());

                                        if (dt.Rows[i]["TRANSACTION_TYPE_NAME"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.TransactionType = Convert.ToString(dt.Rows[i]["TRANSACTION_TYPE_NAME"].ToString());

                                        if (dt.Rows[i]["CREATION_DATE"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.TransactionTimestamp = Convert.ToDateTime(dt.Rows[i]["CREATION_DATE"].ToString());

                                        if (dt.Rows[i]["AMOUNT"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.TransactionAmount = Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString());

                                        if (dt.Rows[i]["PLAZA_NAME"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.GantryName = dt.Rows[i]["PLAZA_NAME"].ToString();

                                        if (dt.Rows[i]["LANE_NAME"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.LaneNumber = dt.Rows[i]["LANE_NAME"].ToString();

                                        objVehicleTransactionHistorySummaryList.Add(objVehicleTransactionHistorySummary);
                                    }
                                    sJSONResponse = JsonConvert.SerializeObject(objVehicleTransactionHistorySummaryList);
                                    return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                                }
                                catch (Exception ex)
                                {
                                    Log("Exception in Transaction History Summary. : " + ex.ToString());
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Somthing went wrong.";
                                    objResponseMessage.Add(objModelState);
                                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                                }
                            }
                            else {
                                ModelStateList objModelState = new ModelStateList();
                                objModelState.ErrorMessage = "No Transaction found.";
                                objResponseMessage.Add(objModelState);
                                sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in Transaction History Summary. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Somthing went wrong.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }

                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "No customer account found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in Transaction History Summary. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Somthing went wrong.";
                    objResponseMessage.Add(objModelState);
                    sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        #endregion

        #region Helper Method
        public void Log(String ExceptionMsg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(ExceptionMsg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MobileWebAPI);
        }

        #endregion
    }
}
