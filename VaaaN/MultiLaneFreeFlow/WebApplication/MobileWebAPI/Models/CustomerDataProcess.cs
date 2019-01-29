using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileWebAPI.Models
{
    public class CustomerDataProcess
    {
        [Display(Name = "Resident Identity Number")]
        [Required(ErrorMessage = "The resident identity number is required")]
        public String ResidentIdentityNumber { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "The name is required")]
        public String CustomerName { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The address is required")]
        public String Address { get; set; }

        [Display(Name = "Mobile Phone Number")]
        [Required(ErrorMessage = "The mobile phone number is required")]
        public String MobilePhoneNumber { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String EmailAddress { get; set; }

        [Display(Name = "Vehicle registration certificate number")]
        [Required(ErrorMessage = "The Vehicle registration certificate number is required")]
        public String VehicleRegistrationCertificateNumber { get; set; }

        [Display(Name = "Vehicle Registration Number")]
        [Required(ErrorMessage = "The vehicle registration number is required")]
        [StringLength(10, ErrorMessage = "Vehicle Registration Number max length is 10")]
        public String VehicleRegistrationNumber { get; set; }

        [Display(Name = "Owner Name")]
        [Required(ErrorMessage = "The owner name is required")]
        public String OwnerName { get; set; }

        [Display(Name = "Owner Address")]
        [Required(ErrorMessage = "The owner address is required")]
        public String OwnerAddress { get; set; }

        [Display(Name = "Vehicle Classification")]
        [Required(ErrorMessage = "The vehicle classification is required")]
        [Range(1, 4, ErrorMessage = "Invalid vehicle classification")]
        public Int32 VehicleClassification { get; set; }

        [Display(Name = "TID Front")]
        [Required(ErrorMessage = "The TID front is required")]
        public String TIDFront { get; set; }

        [Display(Name = "TID Rear")]
        [Required(ErrorMessage = "The TID rear is required")]
        public String TIDRear { get; set; }

        [Display(Name = "Vehicle Image Front")]
        [Required(ErrorMessage = "The vehicle image front is required")]
        public String VehicleImageFront { get; set; }

        [Display(Name = "Vehicle Image Rear")]
        [Required(ErrorMessage = "The vehicle image rear is required")]
        public String VehicleImageRear { get; set; }

        [Display(Name = "Vehicle Image Right")]
        [Required(ErrorMessage = "The vehicle image right is required")]
        public String VehicleImageRight { get; set; }

        [Display(Name = "Vehicle Image Left")]
        [Required(ErrorMessage = "The vehicle image left is required")]
        public String VehicleImageLeft { get; set; }

        [Display(Name = "Vehicle Registration Certificate Image")]
        [Required(ErrorMessage = "The vehicle registration certificate image is required")]
        public String VehicleRegistrationCertificateImage { get; set; }

        [Display(Name = "Resident Identity Card Image")]
        [Required(ErrorMessage = "The resident identity card image is required")]
        public String ResidentIdentityCardImage { get; set; }
    }

    public class CustomerRegistrationResponce
    {
        [Display(Name = "Resident Identity Number")]
        public String ResidentIdentityNumber { get; set; }

        [Display(Name = "Vehicle registration certificate number")]
        public String VehicleRegistrationCertificateNumber { get; set; }

        [Display(Name = "Vehicle Registration Number")]
        [StringLength(10, ErrorMessage = "Vehicle Registration Number max length is 10")]
        public String VehicleRegistrationNumber { get; set; }

        [Display(Name = "EPC Number")]
        public String EPCNumber { get; set; }

        public Int32 CustomerAccountId { get; set; }
        public Int32 CustomerVehicleId { get; set; }
    }

    public class CustomerVehicleInformationforTopup : CustomerVehicleInformation
    {
        [Display(Name = "Top-Up Amount")]
        [Required(ErrorMessage = "Top-Up Amount is required")]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(10000, 1000000, ErrorMessage = "Top-Up must be 10000 to 1000000")]
        public Decimal TopUpAmount { get; set; }
    }

    public class CustomerVehicleInformation
    {
        [Display(Name = "Resident Identity Number")]
        [Required(ErrorMessage = "The resident identity number is required")]
        public String ResidentIdentityNumber { get; set; }

        [Display(Name = "Vehicle registration certificate number")]
        [Required(ErrorMessage = "The Vehicle registration certificate number is required")]
        public String VehicleRegistrationCertificateNumber { get; set; }

        [Display(Name = "Vehicle Registration Number")]
        [Required(ErrorMessage = "The vehicle registration number is required")]
        [StringLength(10, ErrorMessage = "Vehicle Registration Number max length is 10")]
        public String VehicleRegistrationNumber { get; set; }

        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }

    }

    public class CustomerInformation
    {
        [Display(Name = "Resident Identity Number")]
        [Required(ErrorMessage = "The resident identity number is required")]
        public String ResidentIdentityNumber { get; set; }
    }

    public class CustomerTIDDetails
    {
        [Display(Name = "TID")]
        [Required(ErrorMessage = "The TID is required")]
        public String TID { get; set; }

        [Display(Name = "Resident Identity Number")]
        [Required(ErrorMessage = "The resident identity number is required")]
        public String ResidentIdentityNumber { get; set; }
    }

    public class VehicleTransactionHistorySummary
    {
        public string ResidentIdentityNumber { get; set; }
        public string VehicleRegistrationCertificateNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public Int32 TransactionID { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string GantryName { get; set; }
        public string LaneNumber { get; set; }
        public Decimal TransactionAmount { get; set; }
        public Int32 RecordCount { get; set; }
    }

    public class CustomerVehicleDetails
    {
        public string ResidentIdentityNumber { get; set; }
        public string VehicleRegistrationCertificateNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public Int32 ManufactureYear { get; set; }
        public string CylinderCapacity { get; set; }
        public string FrameNumber { get; set; }
        public string EngineNumber { get; set; }
        public string Color { get; set; }
        public string FuelType { get; set; }
        public string LicensePlateColor { get; set; }
        public Int32 RegistrationYear { get; set; }
        public string VehicleOwnershipDocumentNumber { get; set; }
        public string LocationCode { get; set; }
        public string RegistrationQueueNumber { get; set; }
        public string VehicleImageFront { get; set; }
        public string VehicleImageRear { get; set; }
        public string VehicleImageRight { get; set; }
        public string VehicleImageLeft { get; set; }
        public string VehicleRegistrationCertificateImage { get; set; }
        public string ExceptionFlag { get; set; }
        public string Status { get; set; }
        public string ValidUntil { get; set; }
        public string TIDFront { get; set; }
        public string TIDRear { get; set; }
        public Int32 Classification { get; set; }
        public Decimal Balance { get; set; }
        public Int32 VehicleId { get; set; }

    }


    public class CustomerVehicleDetailCollection : CollectionBase
    {
        public CustomerVehicleDetailCollection()
        {
        }
        public CustomerVehicleDetails this[int index]
        {
            get { return (CustomerVehicleDetails)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerVehicleDetails value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerVehicleDetails value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerVehicleDetails value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerVehicleDetails value)
        {
            List.Remove(value);
        }
    }

    public class ResponseMessage
    {
        public List<ModelStateList> ModelState { get; set; }
    }

    public class ModelStateList
    {
        public string ErrorMessage { get; set; }
    }

    public class VehicleTransactionHistorySummaryPagging
    {


    }
}