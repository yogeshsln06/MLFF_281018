﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerVehicleCBE
    {
        private Int32 tmsId;
        private Int32 accountId;
        private Int32 entryId;
        private String vehicleRCNumber;
        private String vehicleRCNumberImagePath;
        private String vehRegNo;
        private String ownerName;
        private String ownerAddress;
        private String brand;
        private String vehicleType;
        private String vehicleCategory;
        private String model;
        private Int32 manufacturingYear;
        private String cyclinderCapacity;
        private String frameNumber;
        private String engineNumber;
        private String vehicleColor;
        private Int32 fuelType;
        private String fuelTypeName;
        private Int32 licencePlateColor;
        private String licencePlateColorName;
        private Int32 registrationYear;
        private String vehicleOwnershipDocumentNumber;
        private String locationCode;
        private String registrationQueueNumber;
        private String vehicleImageFront;
        private String vehicleImageRear;
        private String vehicleImageRight;
        private String vehicleImageLeft;
        private Int16 exceptionFlag;
        private String exceptionFlagName;
        private Int16 status;
        private Nullable<DateTime> validUntil;
        private String tidFront;
        private String tidRear;
        private String tagId;
        private Int32 vehicleClassId;
        private String vehicleClassName;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifiedBy;
        private Int32 transferStatus;
        private Decimal accountBalance;
        private Int16 registartionThrough;
        private Int16 isDocVerified;
        private Int16 queueStatus;
        private String customerQueueStatusName;


        public CustomerVehicleCBE()
        {
            this.tmsId = 1;
            this.accountId = 0;
            this.entryId = 0;
            this.vehicleRCNumber = string.Empty;
            this.vehicleRCNumberImagePath = string.Empty;
            this.vehRegNo = string.Empty;
            this.ownerName = string.Empty;
            this.ownerAddress = string.Empty;
            this.brand = string.Empty;
            this.vehicleType = string.Empty;
            this.vehicleCategory = string.Empty;
            this.model = string.Empty;
            this.manufacturingYear = 0;
            this.cyclinderCapacity = string.Empty;
            this.frameNumber = string.Empty;
            this.engineNumber = string.Empty;
            this.vehicleColor = string.Empty;
            this.fuelType = 0;
            this.licencePlateColor = 0;
            this.registrationYear = 0;
            this.vehicleOwnershipDocumentNumber = string.Empty;
            this.locationCode = string.Empty;
            this.registrationQueueNumber = string.Empty;
            this.vehicleImageFront = string.Empty;
            this.vehicleImageRear = string.Empty;
            this.vehicleImageRight = string.Empty;
            this.vehicleImageLeft = string.Empty;
            this.exceptionFlag = 0;
            this.status = 0;
            this.validUntil = null;
            this.tidFront = string.Empty;
            this.tidRear = string.Empty;
            this.tagId = string.Empty;
            this.vehicleClassId = 0;
            this.vehicleClassName = string.Empty;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifiedBy = 0;
            this.transferStatus = 1;
            this.accountBalance = 0;
            this.registartionThrough = 0;
            this.isDocVerified = 1;
            this.queueStatus = 1;
        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        [Display(Name = "Customer ID")]
        public Int32 AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        [Display(Name = "Vehicle ID")]
        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }

        [Display(Name = "Certificate Num")]
        [Required]
        public String VehicleRCNumber
        {
            get
            {
                return this.vehicleRCNumber;
            }

            set
            {
                this.vehicleRCNumber = value;
            }
        }

        [Display(Name = "Registration Certificate Image")]
        [Required]
        public String VehicleRCNumberImagePath
        {
            get
            {
                return this.vehicleRCNumberImagePath;
            }

            set
            {
                this.vehicleRCNumberImagePath = value;
            }
        }

        [Display(Name = "Registration Certificate Image")]
        public System.Web.HttpPostedFileBase VehicleRCNumberImage { get; set; }

        [Display(Name = "Registration Num")]
        //[Display(Name = "VehicleRegNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Required]
        [StringLength(9, ErrorMessage = "Vehicle Registration Num max length is 9")]
        public String VehRegNo
        {
            get { return this.vehRegNo; }
            set { this.vehRegNo = value; }
        }


        [Required]
        [Display(Name = "Owner Name")]
        public String OwnerName
        {
            get { return this.ownerName; }
            set { this.ownerName = value; }
        }

        [Display(Name = "Owner Address")]
        [Required]
        public String OwnerAddress
        {
            get { return this.ownerAddress; }
            set { this.ownerAddress = value; }
        }

        public String Brand
        {
            get
            {
                return this.brand;
            }

            set
            {
                this.brand = value;
            }
        }

        [Display(Name = "Type")]
        public String VehicleType
        {
            get
            {
                return this.vehicleType;
            }

            set
            {
                this.vehicleType = value;
            }
        }

        [Display(Name = "Category")]
        public String VehicleCategory
        {
            get
            {
                return this.vehicleCategory;
            }

            set
            {
                this.vehicleCategory = value;
            }
        }

        public String Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
            }
        }

        [Display(Name = "Manufacturing Year")]
        public Int32 ManufacturingYear
        {
            get
            {
                return this.manufacturingYear;
            }

            set
            {
                this.manufacturingYear = value;
            }
        }

        [Display(Name = "Cylinder Capacity")]
        public String CyclinderCapacity
        {
            get
            {
                return this.cyclinderCapacity;
            }

            set
            {
                this.cyclinderCapacity = value;
            }
        }

        [Display(Name = "Frame Num")]
        public String FrameNumber
        {
            get
            {
                return this.frameNumber;
            }

            set
            {
                this.frameNumber = value;
            }
        }

        [Display(Name = "Engine Num")]
        public String EngineNumber
        {
            get
            {
                return this.engineNumber;
            }

            set
            {
                this.engineNumber = value;
            }
        }

        [Display(Name = "Color")]
        public String VehicleColor
        {
            get
            {
                return this.vehicleColor;
            }

            set
            {
                this.vehicleColor = value;
            }
        }

        [Display(Name = "Fuel Type")]
        public Int32 FuelType
        {
            get
            {
                return this.fuelType;
            }

            set
            {
                this.fuelType = value;
            }
        }

        public String FuelTypeName
        {
            get
            {
                return this.fuelTypeName;
            }

            set
            {
                this.fuelTypeName = value;
            }
        }

        [Display(Name = "Lisence Plate Color")]
        public Int32 LicencePlateColor
        {
            get
            {
                return this.licencePlateColor;
            }

            set
            {
                this.licencePlateColor = value;
            }
        }

        public String LicencePlateColorName
        {
            get
            {
                return this.licencePlateColorName;
            }

            set
            {
                this.licencePlateColorName = value;
            }
        }

        [Display(Name = "Registration Year")]
        public Int32 RegistrationYear
        {
            get
            {
                return this.registrationYear;
            }

            set
            {
                this.registrationYear = value;
            }
        }

        [Display(Name = "Ownership Document Num")]
        public String VehicleOwnershipDocumentNumber
        {
            get
            {
                return this.vehicleOwnershipDocumentNumber;
            }

            set
            {
                this.vehicleOwnershipDocumentNumber = value;
            }
        }

        [Display(Name = "Location Code")]
        public String LocationCode
        {
            get
            {
                return this.locationCode;
            }

            set
            {
                this.locationCode = value;
            }
        }

        [Display(Name = "Registration Queue Num")]
        public String RegistrationQueueNumber
        {
            get
            {
                return this.registrationQueueNumber;
            }

            set
            {
                this.registrationQueueNumber = value;
            }
        }

        [Display(Name = "Image Front")]
        [Required]
        public String VehicleImageFront
        {
            get
            {
                return this.vehicleImageFront;
            }

            set
            {
                this.vehicleImageFront = value;
            }
        }

        [Display(Name = "Image Front")]
        public System.Web.HttpPostedFileBase FrontImage { get; set; }

        [Display(Name = "Image Rear")]
        [Required]
        public String VehicleImageRear
        {
            get
            {
                return this.vehicleImageRear;
            }

            set
            {
                this.vehicleImageRear = value;
            }
        }

        [Display(Name = "Image Rear")]
        public System.Web.HttpPostedFileBase RearImage { get; set; }

        [Display(Name = "Image Right")]
        [Required]
        public String VehicleImageRight
        {
            get
            {
                return this.vehicleImageRight;
            }

            set
            {
                this.vehicleImageRight = value;
            }
        }

        [Display(Name = "Image Right")]
        public System.Web.HttpPostedFileBase RightImage { get; set; }

        [Display(Name = "Image Left")]
        [Required]
        public String VehicleImageLeft
        {
            get
            {
                return this.vehicleImageLeft;
            }

            set
            {
                this.vehicleImageLeft = value;
            }
        }

        [Display(Name = "Image Left")]
        public System.Web.HttpPostedFileBase LeftImage { get; set; }

        [Display(Name = "Exception Flag")]
        public Int16 ExceptionFlag
        {
            get { return this.exceptionFlag; }
            set { this.exceptionFlag = value; }
        }

        public String ExceptionFlagName
        {
            get { return this.exceptionFlagName; }
            set { this.exceptionFlagName = value; }
        }

        public Int16 Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Valid Until")]
        public Nullable<DateTime> ValidUntil
        {
            get
            {
                return this.validUntil;
            }

            set
            {
                this.validUntil = value;
            }
        }

        [Display(Name = "TID Front")]
        [Required]
        public String TidFront
        {
            get
            {
                return this.tidFront;
            }

            set
            {
                this.tidFront = value;
            }
        }

        [Display(Name = "TID Rear")]
        [Required]
        public String TidRear
        {
            get
            {
                return this.tidRear;
            }

            set
            {
                this.tidRear = value;
            }
        }


        [Display(Name = "EPC")]
        public String TagId
        {
            get { return this.tagId; }
            set { this.tagId = value; }
        }

        //[Display(Name = "VehicleClass", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Display(Name = "Classification")]
        [Required]
        public Int32 VehicleClassId
        {
            get { return this.vehicleClassId; }
            set { this.vehicleClassId = value; }
        }

        [Display(Name = "Vehicle Class Name")]
        public String VehicleClassName
        {
            get
            {
                return this.vehicleClassName;
            }

            set
            {
                this.vehicleClassName = value;
            }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set { this.creationDate = value; }
        }

        public DateTime ModificationDate
        {
            get { return this.modificationDate; }
            set { this.modificationDate = value; }
        }

        public Int32 ModifiedBy
        {
            get { return this.modifiedBy; }
            set { this.modifiedBy = value; }
        }

        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        //[Display(Name = "AccountBalance", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Display(Name = "Balance")]
        [Required]
        public Decimal AccountBalance
        {
            get { return this.accountBalance; }
            set { this.accountBalance = value; }
        }

        [Display(Name = "Document Verified")]
        public Int16 IsDocVerified
        {
            get { return this.isDocVerified; }
            set { this.isDocVerified = value; }
        }

        [Display(Name = "Registartion Through")]
        [Required]
        public Int16 RegistartionThrough
        {
            get
            {
                return this.registartionThrough;
            }

            set
            {
                this.registartionThrough = value;
            }
        }

        [Display(Name = "Status")]
        public Int16 QueueStatus
        {
            get
            {
                return this.queueStatus;
            }

            set
            {
                this.queueStatus = value;
            }
        }
        public String CustomerQueueStatusName
        {
            get
            {
                return this.customerQueueStatusName;
            }

            set
            {
                this.customerQueueStatusName = value;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("accountId = " + this.accountId + Environment.NewLine);
            sb.Append("entryId = " + this.entryId + Environment.NewLine);
            sb.Append("vehicleRCNumber = " + this.vehicleRCNumber + Environment.NewLine);
            sb.Append("vehicleRCNumberImagePath = " + this.vehicleRCNumberImagePath + Environment.NewLine);
            sb.Append("vehRegNo = " + this.vehRegNo + Environment.NewLine);
            sb.Append("ownerName = " + this.ownerName + Environment.NewLine);
            sb.Append("ownerAddress = " + this.ownerAddress + Environment.NewLine);
            sb.Append("brand = " + this.brand + Environment.NewLine);
            sb.Append("vehicleType = " + this.vehicleType + Environment.NewLine);
            sb.Append("vehicleCategory = " + this.vehicleCategory + Environment.NewLine);
            sb.Append("model = " + this.model + Environment.NewLine);
            sb.Append("manufacturingYear = " + this.manufacturingYear + Environment.NewLine);
            sb.Append("cyclinderCapacity = " + this.cyclinderCapacity + Environment.NewLine);
            sb.Append("frameNumber = " + this.frameNumber + Environment.NewLine);
            sb.Append("engineNumber = " + this.engineNumber + Environment.NewLine);
            sb.Append("vehicleColor = " + this.vehicleColor + Environment.NewLine);
            sb.Append("fuelType = " + this.fuelType + Environment.NewLine);
            sb.Append("licencePlateColor = " + this.licencePlateColor + Environment.NewLine);
            sb.Append("registrationYear = " + this.registrationYear + Environment.NewLine);
            sb.Append("vehicleOwnershipDocumentNumber = " + this.vehicleOwnershipDocumentNumber + Environment.NewLine);
            sb.Append("locationCode = " + this.locationCode + Environment.NewLine);
            sb.Append("registrationQueueNumber = " + this.registrationQueueNumber + Environment.NewLine);
            sb.Append("vehicleImageFront = " + this.vehicleImageFront + Environment.NewLine);
            sb.Append("vehicleImageRear = " + this.vehicleImageRear + Environment.NewLine);
            sb.Append("VehicleImageRightSide = " + this.VehicleImageRight + Environment.NewLine);
            sb.Append("VehicleImageLeftSide = " + this.VehicleImageLeft + Environment.NewLine);
            sb.Append("exceptionFlag = " + this.exceptionFlag + Environment.NewLine);
            sb.Append("status = " + this.status + Environment.NewLine);
            sb.Append("validUntil = " + this.validUntil + Environment.NewLine);
            sb.Append("tidFront = " + this.tidFront + Environment.NewLine);
            sb.Append("tidRear = " + this.tidRear + Environment.NewLine);
            sb.Append("tagId = " + this.tagId + Environment.NewLine);
            sb.Append("vehicleClassId = " + this.vehicleClassId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifiedBy + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);
            sb.Append("accountBalance = " + this.accountBalance + Environment.NewLine);
            sb.Append("registartionThrough = " + this.registartionThrough + Environment.NewLine);
            sb.Append("isDocVerified = " + this.isDocVerified + Environment.NewLine);
            sb.Append("queueStatus = " + this.queueStatus + Environment.NewLine);
            return sb.ToString();
        }
    }

    public class CustomerVehicleCollection : CollectionBase
    {
        public CustomerVehicleCollection()
        {
        }
        public CustomerVehicleCBE this[int index]
        {
            get { return (CustomerVehicleCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerVehicleCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerVehicleCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerVehicleCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerVehicleCBE value)
        {
            List.Remove(value);
        }
    }
}
