﻿using System;
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
using System.Globalization;
using System.Messaging;

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
        private MessageQueue smsMessageQueue;
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
                        List<CustomerVehicleCBE> TIDFrontfiltered = customerVehicleDataList.FindAll(x => (x.TidFront == objVehicleRegistration.TIDFront.ToString() || x.TidRear == objVehicleRegistration.TIDFront.ToString()));
                        List<CustomerVehicleCBE> TIDRearfiltered = customerVehicleDataList.FindAll(x => (x.TidFront == objVehicleRegistration.TIDRear.ToString() || x.TidRear == objVehicleRegistration.TIDRear.ToString()));
                        if (objVehicleRegistration.TIDFront == objVehicleRegistration.TIDRear)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Rear TID must be different from Front TID.";
                            objResponseMessage.Add(objModelState);
                        }

                        if (VehRegNofiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Vehicle Registration Number already exists.";
                            objResponseMessage.Add(objModelState);
                        }

                        if (TIDFrontfiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Front TID already exists.";
                            objResponseMessage.Add(objModelState);
                        }

                        if (TIDRearfiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Rear TID already exists.";
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
                            if (sJSONResponse.Contains("Something went wrong"))
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                            else
                                return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                            #endregion
                        }
                    }
                    else
                    {
                        #region Cutomer Not found Need to registor
                        #region Validate Customer Account
                        customerDataList = CustomerAccountBLL.GetAllAsList();
                        List<CustomerAccountCBE> Mobilefiltered = customerDataList.FindAll(x => x.MobileNo == objCustomerAccountCBE.MobileNo.ToString());
                        List<CustomerAccountCBE> Emailfiltered = customerDataList.FindAll(x => x.EmailId == objCustomerAccountCBE.EmailId.ToString().Trim());
                        List<CustomerAccountCBE> Registrationfiltered = customerDataList.FindAll(x => x.ResidentId == objCustomerAccountCBE.ResidentId.Trim());

                        #region Validate Mobile Resident Id and Email Id
                        if (Registrationfiltered.Count > 0)
                        {
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Resident Card already exists.";
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
                            Log("Unable to save resident Resident card image of Resident Id : " + objVehicleRegistration.ResidentIdentityNumber + " with Exception: " + ex);
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Invalid Resident card image.";
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
                            if (sJSONResponse.Contains("Something went wrong"))
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
                    objModelState.ErrorMessage = "Something went wrong.";
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
                        objModelState.ErrorMessage = "Something went wrong.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                    }

                }
                else {
                    objModelState.ErrorMessage = "Something went wrong.";
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

        #region API for Customer Vehicle Top-Up
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
                    dt = CustomerVehicleBLL.GetCustomerVehicleDetails(objCustomerVehicleCBE);
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
                            accountHistory.OpeningBalance = (objCustomerVehicleCBE.AccountBalance - objCustomerVehicleInformation.TopUpAmount);
                            accountHistory.ClosingBalance = objCustomerVehicleCBE.AccountBalance;
                            entryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);

                            #endregion

                            #region Save outgoing message in database
                            // This message will be sent by SMS service
                            CustomerAccountCBE customerAccount = new CustomerAccountCBE();
                            customerAccount.TmsId = 1;
                            customerAccount.AccountId = objCustomerVehicleCBE.AccountId;
                            customerAccount = CustomerAccountBLL.GetCustomerById(customerAccount);


                            System.Messaging.Message smsMessage = new System.Messaging.Message();
                            smsMessage.Formatter = new BinaryMessageFormatter();
                            VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                            CultureInfo culture = new CultureInfo("id-ID");



                            string Topup = Constants.TopUp;
                            Topup = Topup.Replace("[rechargeamount]", Decimal.Parse(objCustomerVehicleInformation.TopUpAmount.ToString()).ToString("C", culture));
                            Topup = Topup.Replace("[vehregno]", objCustomerVehicleInformation.VehicleRegistrationNumber);
                            Topup = Topup.Replace("[balance]", Decimal.Parse(objCustomerVehicleCBE.AccountBalance.ToString()).ToString("C", culture));
                            Topup = Topup.Replace("[transactiondatetime]", transcationDateTime.ToString());
                            Topup = Topup.Replace("[tid]", entryId.ToString());

                            smsDetail.SMSMessage = Topup;
                            smsDetail.AccountId = customerAccount.AccountId;
                            smsDetail.CustomerName = customerAccount.FirstName;
                            smsDetail.SenderMobileNumber = customerAccount.MobileNo;
                            smsDetail.AccountHistoryId = entryId;
                            smsMessage.Body = smsDetail;
                            smsMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.smsMessageQueue);
                            smsMessageQueue.Send(smsMessage);

                            Log("outbound message inserted successfully.");
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
                            objModelState.ErrorMessage = "Something went wrong.";
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
                    objModelState.ErrorMessage = "Something went wrong.";
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
                            List<CustomerVehicleDetails> objCustomerVehicleDetails = new List<CustomerVehicleDetails>();
                            CustomerVehicleDetails objCustomerVehicleDetail = new CustomerVehicleDetails();

                            objCustomerVehicleDetails = ConvertDataTableToList(dt);
                            objCustomerVehicleDetail = objCustomerVehicleDetails[0];
                            //objCustomerVehicleDetail.Balance = objCustomerVehicleDetails[0].Balance;
                            //objCustomerVehicleDetail.Brand = objCustomerVehicleDetails[0].Brand;
                            //objCustomerVehicleDetail.Category = objCustomerVehicleDetails[0].Category;
                            //objCustomerVehicleDetail.Classification = objCustomerVehicleDetails[0].Classification;
                            //objCustomerVehicleDetail.Color = objCustomerVehicleDetails[0].Color;
                            //objCustomerVehicleDetail.CylinderCapacity = objCustomerVehicleDetails[0].CylinderCapacity;
                            //objCustomerVehicleDetail.EngineNumber = objCustomerVehicleDetails[0].EngineNumber;
                            //objCustomerVehicleDetail.ExceptionFlag = objCustomerVehicleDetails[0].ExceptionFlag;
                            //objCustomerVehicleDetail.FrameNumber = objCustomerVehicleDetails[0].FrameNumber;
                            //objCustomerVehicleDetail.FuelType = objCustomerVehicleDetails[0].FuelType;
                            //objCustomerVehicleDetail.LicensePlateColor = objCustomerVehicleDetails[0].LicensePlateColor;
                            //objCustomerVehicleDetail.LocationCode = objCustomerVehicleDetails[0].LocationCode;
                            //objCustomerVehicleDetail.ManufactureYear = objCustomerVehicleDetails[0].ManufactureYear;
                            //objCustomerVehicleDetail.Model = objCustomerVehicleDetails[0].Model;
                            //objCustomerVehicleDetail.OwnerAddress = objCustomerVehicleDetails[0].OwnerAddress;
                            //objCustomerVehicleDetail.OwnerName = objCustomerVehicleDetails[0].OwnerName;
                            //objCustomerVehicleDetail.RegistrationQueueNumber = objCustomerVehicleDetails[0].RegistrationQueueNumber;
                            //objCustomerVehicleDetail.RegistrationYear = objCustomerVehicleDetails[0].RegistrationYear;
                            //objCustomerVehicleDetail.ResidentIdentityNumber = objCustomerVehicleDetails[0].ResidentIdentityNumber;
                            //objCustomerVehicleDetail.Status = objCustomerVehicleDetails[0].Status;
                            //objCustomerVehicleDetail.TIDFront = objCustomerVehicleDetails[0].TIDFront;
                            //objCustomerVehicleDetail.TIDRear = objCustomerVehicleDetails[0].TIDRear;
                            //objCustomerVehicleDetail.Type = objCustomerVehicleDetails[0].Type;
                            //objCustomerVehicleDetail.ValidUntil = objCustomerVehicleDetails[0].ValidUntil;
                            //objCustomerVehicleDetail.VehicleImageFront = objCustomerVehicleDetails[0].VehicleImageFront;
                            //objCustomerVehicleDetail.VehicleImageLeft = objCustomerVehicleDetails[0].VehicleImageLeft;
                            //objCustomerVehicleDetail.VehicleImageRear = objCustomerVehicleDetails[0].VehicleImageRear;
                            //objCustomerVehicleDetail.VehicleImageRight = objCustomerVehicleDetails[0].VehicleImageRight;
                            //objCustomerVehicleDetail.VehicleOwnershipDocumentNumber = objCustomerVehicleDetails[0].VehicleOwnershipDocumentNumber;
                            //objCustomerVehicleDetail.VehicleRegistrationCertificateImage = objCustomerVehicleDetails[0].VehicleRegistrationCertificateImage;
                            //objCustomerVehicleDetail.VehicleRegistrationCertificateNumber = objCustomerVehicleDetails[0].VehicleRegistrationCertificateNumber;
                            //objCustomerVehicleDetail.VehicleRegistrationNumber = objCustomerVehicleDetails[0].VehicleRegistrationNumber;
                            //objCustomerVehicleDetail.VehicleId = objCustomerVehicleDetails[0].VehicleId;
                            sJSONResponse = JsonConvert.SerializeObject(objCustomerVehicleDetail);
                            return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong.";
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
                    objModelState.ErrorMessage = "Something went wrong.";
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


        #region API for Customer Vehicle Details List
        [Route("VaaaN/IndonesiaMLFFMobileApi/InquiryCustomerVehicleList")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage InquiryCustomerVehicleList(CustomerInformation objCustomerInformation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dt = CustomerVehicleBLL.CustomerVehicleDetailsByResidentId(objCustomerInformation.ResidentIdentityNumber);
                    if (dt.Rows.Count > 0)
                    {

                        try
                        {
                            List<CustomerVehicleDetails> customerVehicleList = new List<CustomerVehicleDetails>();
                            customerVehicleList = ConvertDataTableToList(dt);
                            sJSONResponse = JsonConvert.SerializeObject(customerVehicleList);
                            return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "No vehicle found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Something went wrong.";
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

        #region API for Customer Start Driving
        [Route("VaaaN/IndonesiaMLFFMobileApi/StartDriving")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage StartDriving(CustomerTIDDetails objCustomerTIDDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dt = CustomerVehicleBLL.CustomerVehicleDetailsByTID(objCustomerTIDDetails.TID, string.Empty);
                    if (dt.Rows.Count > 0)
                    {

                        try
                        {
                            List<CustomerVehicleDetails> objCustomerVehicleDetails = new List<CustomerVehicleDetails>();
                            CustomerVehicleDetails objCustomerVehicleDetail = new CustomerVehicleDetails();

                            objCustomerVehicleDetails = ConvertDataTableToList(dt);
                            objCustomerVehicleDetail = objCustomerVehicleDetails[0];

                            sJSONResponse = JsonConvert.SerializeObject(objCustomerVehicleDetail);
                            return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                        }
                        catch (Exception ex)
                        {
                            Log("Exception in Inquiry Customer Vehicle Detail. : " + ex.ToString());
                            ModelStateList objModelState = new ModelStateList();
                            objModelState.ErrorMessage = "Something went wrong.";
                            objResponseMessage.Add(objModelState);
                            sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                        }
                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "No vehicle found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in Start Driving. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Something went wrong.";
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


        #region API for Customer Validate TID
        [Route("VaaaN/IndonesiaMLFFMobileApi/ValidateTID")]
        [HttpPost]
        [Filters.ValidateModel]
        public HttpResponseMessage ValidateTID(TIDDetails objCustomerTIDDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dt = CustomerVehicleBLL.ValidateTID(objCustomerTIDDetails.TID);
                    if (dt.Rows.Count > 0)
                    {

                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "vehicle found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                    else {
                        ModelStateList objModelState = new ModelStateList();
                        objModelState.ErrorMessage = "vehicle not found.";
                        objResponseMessage.Add(objModelState);
                        sJSONResponse = JsonConvert.SerializeObject(objResponseMessage);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, sJSONResponse);
                    }
                }
                catch (Exception ex)
                {
                    Log("Exception in Start Driving. : " + ex.ToString());
                    ModelStateList objModelState = new ModelStateList();
                    objModelState.ErrorMessage = "Something went wrong.";
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

                            dt = AccountHistoryBLL.AccountHistoryByVehicleWithPaging(objCustomerVehicleInformation.ResidentIdentityNumber, objCustomerVehicleInformation.VehicleRegistrationCertificateNumber, objCustomerVehicleInformation.VehicleRegistrationNumber, objCustomerVehicleInformation.PageIndex, objCustomerVehicleInformation.PageSize);
                            // dt = AccountHistoryBLL.AccountHistoryByVehicle(objCustomerVehicleInformation.ResidentIdentityNumber, objCustomerVehicleInformation.VehicleRegistrationCertificateNumber, objCustomerVehicleInformation.VehicleRegistrationNumber);
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

                                        if (dt.Rows[i]["RecordCount"] != DBNull.Value)
                                            objVehicleTransactionHistorySummary.RecordCount = Convert.ToInt32(dt.Rows[i]["RecordCount"].ToString());

                                        objVehicleTransactionHistorySummaryList.Add(objVehicleTransactionHistorySummary);
                                    }
                                    sJSONResponse = JsonConvert.SerializeObject(objVehicleTransactionHistorySummaryList);
                                    return Request.CreateErrorResponse(HttpStatusCode.OK, sJSONResponse);
                                }
                                catch (Exception ex)
                                {
                                    Log("Exception in Transaction History Summary. : " + ex.ToString());
                                    ModelStateList objModelState = new ModelStateList();
                                    objModelState.ErrorMessage = "Something went wrong.";
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
                            objModelState.ErrorMessage = "Something went wrong.";
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
                    objModelState.ErrorMessage = "Something went wrong.";
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

        private static List<CustomerVehicleDetails> ConvertDataTableToList(DataTable dt)
        {
            List<CustomerVehicleDetails> customerVehicleList = new List<CustomerVehicleDetails>();

            foreach (DataRow dr in dt.Rows)
            {
                customerVehicleList.Add(ConvertDataTableToCBE(dr));
            }

            return customerVehicleList;
        }


        private static CustomerVehicleDetails ConvertDataTableToCBE(DataRow row)
        {

            CustomerVehicleDetails vehicle = new CustomerVehicleDetails();

            if (row["ENTRY_ID"] != DBNull.Value)
                vehicle.VehicleId = Convert.ToInt32(row["ENTRY_ID"]);
            if (row["VEH_REG_NO"] != DBNull.Value)
                vehicle.VehicleRegistrationNumber = Convert.ToString(row["VEH_REG_NO"]);

            if (row["VEHICLE_CLASS_ID"] != DBNull.Value)
                vehicle.Classification = Convert.ToInt32(row["VEHICLE_CLASS_ID"]);

            if (row["VEHICLE_RC_NO"] != DBNull.Value)
                vehicle.VehicleRegistrationCertificateNumber = Convert.ToString(row["VEHICLE_RC_NO"]);

            if (row["OWNER_NAME"] != DBNull.Value)
                vehicle.OwnerName = Convert.ToString(row["OWNER_NAME"]);

            if (row["OWNER_ADDRESS"] != DBNull.Value)
                vehicle.OwnerAddress = Convert.ToString(row["OWNER_ADDRESS"]);

            if (row["BRAND"] != DBNull.Value)
                vehicle.Brand = Convert.ToString(row["BRAND"]);

            if (row["VEHICLE_TYPE"] != DBNull.Value)
                vehicle.Type = Convert.ToString(row["VEHICLE_TYPE"]);

            if (row["VEHICLE_CATEGORY"] != DBNull.Value)
                vehicle.Category = Convert.ToString(row["VEHICLE_CATEGORY"]);

            if (row["MODEL_NO"] != DBNull.Value)
                vehicle.Model = Convert.ToString(row["MODEL_NO"]);

            if (row["MANUFACTURING_YEAR"] != DBNull.Value)
                vehicle.ManufactureYear = Convert.ToInt32(row["MANUFACTURING_YEAR"]);

            if (row["CYCLINDER_CAPACITY"] != DBNull.Value)
                vehicle.CylinderCapacity = Convert.ToString(row["CYCLINDER_CAPACITY"]);

            if (row["FRAME_NUMBER"] != DBNull.Value)
                vehicle.FrameNumber = Convert.ToString(row["FRAME_NUMBER"]);

            if (row["ENGINE_NUMBER"] != DBNull.Value)
                vehicle.EngineNumber = Convert.ToString(row["ENGINE_NUMBER"]);

            if (row["VEHICLE_COLOR"] != DBNull.Value)
                vehicle.Color = Convert.ToString(row["VEHICLE_COLOR"]);

            if (row["REGISTRATION_YEAR"] != DBNull.Value)
                vehicle.RegistrationYear = Convert.ToInt32(row["REGISTRATION_YEAR"]);

            if (row["VEHICLE_OWNERSHIP_NO"] != DBNull.Value)
                vehicle.VehicleOwnershipDocumentNumber = Convert.ToString(row["VEHICLE_OWNERSHIP_NO"]);

            if (row["LOCATION_CODE"] != DBNull.Value)
                vehicle.LocationCode = Convert.ToString(row["LOCATION_CODE"]);

            if (row["REG_QUEUE_NO"] != DBNull.Value)
                vehicle.RegistrationQueueNumber = Convert.ToString(row["REG_QUEUE_NO"]);

            if (row["VEHICLEIMAGE_FRONT"] != DBNull.Value)
                vehicle.VehicleImageFront = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + Convert.ToString(row["VEHICLEIMAGE_FRONT"]);

            if (row["VEHICLEIMAGE_REAR"] != DBNull.Value)
                vehicle.VehicleImageRear = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + Convert.ToString(row["VEHICLEIMAGE_REAR"]);

            if (row["VEHICLEIMAGE_RIGHT"] != DBNull.Value)
                vehicle.VehicleImageRight = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + Convert.ToString(row["VEHICLEIMAGE_RIGHT"]);

            if (row["VEHICLEIMAGE_LEFT"] != DBNull.Value)
                vehicle.VehicleImageLeft = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + Convert.ToString(row["VEHICLEIMAGE_LEFT"]);

            if (row["VEHICLE_RC_NO_PATH"] != DBNull.Value)
                vehicle.VehicleRegistrationCertificateImage = "http://poc-erp.balitower.co.id:5556/Attachment/VehicleImage/" + Convert.ToString(row["VEHICLE_RC_NO_PATH"]);

            if (row["TID_FRONT"] != DBNull.Value)
                vehicle.TIDFront = Convert.ToString(row["TID_FRONT"]);

            if (row["TID_REAR"] != DBNull.Value)
                vehicle.TIDRear = Convert.ToString(row["TID_REAR"]);

            if (row["ACCOUNT_BALANCE"] != DBNull.Value)
                vehicle.Balance = Convert.ToDecimal(row["ACCOUNT_BALANCE"]);

            if (row["EXCEPTION_FLAG_NAME"] != DBNull.Value)
                vehicle.ExceptionFlag = Convert.ToString(row["EXCEPTION_FLAG_NAME"]);

            if (row["FUEL_TYPE_NAME"] != DBNull.Value)
                vehicle.FuelType = Convert.ToString(row["FUEL_TYPE_NAME"]);

            if (row["LICENCE_PLATE_COLOR_NAME"] != DBNull.Value)
                vehicle.LicensePlateColor = Convert.ToString(row["LICENCE_PLATE_COLOR_NAME"]);

            if (row["QUEUE_STATUS"] != DBNull.Value)
                vehicle.Status = Convert.ToString(row["QUEUE_STATUS"]);

            if (row["VEHICLE_TYPE"] != DBNull.Value)
                vehicle.Type = Convert.ToString(row["VEHICLE_TYPE"]);

            if (row["VALID_UNTIL"] != DBNull.Value)
                vehicle.ValidUntil = Convert.ToString(row["VALID_UNTIL"]);

            if (row["RESIDENT_ID"] != DBNull.Value)
                vehicle.ResidentIdentityNumber = Convert.ToString(row["RESIDENT_ID"]);

            return vehicle;
        }

        #endregion
    }
}
