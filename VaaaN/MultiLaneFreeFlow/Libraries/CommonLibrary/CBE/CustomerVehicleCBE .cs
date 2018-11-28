using System;
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
        private String tagId;
        private Int32 vehicleClassId;
        private string vehicleClassName;
        private String vehRegNo;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifierId;
        private Int32 modifiedBy;
        private Int32 transferStatus;
        private Int32 amount;
        private Int32 vehicleRegistrationCerticateId;
        private string address;
        private Int32 brand;
        private Int32 vehicleType;
        private Int32 vehicleCategory;
        private String modelNo;
        private Int32 manufacturingYear;
        private String cyclinderCapacity;
        private String frameNumber;
        private String engineNumber;
        private Int32 vehicleColor;
        private Int32 fuelType;
        private Int32 licencePlateColor;
        private Int32 registrationYear;
        private String locationCode;
        private String vehicleImageFront;
        private String vehicleImageRear;
        private String vehicleImageRightSide;
        private String vehicleImageLeftSide;
        private Int32 validUntil;
        public CustomerVehicleCBE()
        {
            this.tmsId = 0;
            this.accountId = 0;
            this.entryId = 0;
            this.tagId = String.Empty;
            this.vehicleClassId = 0;
            this.vehicleClassName = string.Empty;
            this.vehRegNo = String.Empty;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifierId = 0;
            this.modifiedBy = 0;
            this.transferStatus = 0;
            this.amount = 0;
            this.vehicleRegistrationCerticateId = 0;
            this.address = string.Empty;
            this.brand = 0;
            this.vehicleType = 0;
            this.vehicleCategory = 0;
            this.modelNo = string.Empty;
            this.manufacturingYear = 0;
            this.cyclinderCapacity = string.Empty;
            this.frameNumber = string.Empty;
            this.engineNumber = string.Empty;
            this.vehicleColor = 0;
            this.fuelType = 0;
            this.licencePlateColor = 0;
            this.registrationYear = 0;
            this.locationCode = string.Empty;
            this.vehicleImageFront = string.Empty;
            this.vehicleImageRear = string.Empty;
            this.vehicleImageRightSide = string.Empty;
            this.vehicleImageLeftSide = string.Empty;
            this.validUntil = 0;

        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        public Int32 AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }
        [Required]
        [Display(Name = "TagId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String TagId
        {
            get { return this.tagId; }
            set { this.tagId = value; }
        }
        [Display(Name = "VehicleClass", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 VehicleClassId
        {
            get { return this.vehicleClassId; }
            set { this.vehicleClassId = value; }
        }

        [Required]
        [Display(Name ="VehicleRegNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String VehRegNo
        {
            get { return this.vehRegNo; }
            set { this.vehRegNo = value; }
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

        public Int32 ModifierId
        {
            get { return this.modifierId; }
            set { this.modifierId = value; }
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
        [Display(Name = "Vehicle Class")]
        public string VehicleClassName
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
        [Display(Name = "Amount", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Required]
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public int VehicleRegistrationCerticateId
        {
            get
            {
                return this.vehicleRegistrationCerticateId;
            }

            set
            {
                this.vehicleRegistrationCerticateId = value;
            }
        }

        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
            }
        }

        public int Brand
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

        public int VehicleType
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
        
        public int VehicleCategory
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

        public string ModelNo
        {
            get
            {
                return this.modelNo;
            }

            set
            {
                this.modelNo = value;
            }
        }

        public int ManufacturingYear
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

        public string CyclinderCapacity
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

        public string FrameNumber
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

        public string EngineNumber
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

        public int VehicleColor
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

        public int FuelType
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

        public int LicencePlateColor
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

        public int RegistrationYear
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

        public string LocationCode
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

        public string VehicleImageFront
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

        [Display(Name = "Front Image")]
        public System.Web.HttpPostedFileBase FrontImage { get; set; }

        public string VehicleImageRear
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
        [Display(Name = "Rear Image")]
        public System.Web.HttpPostedFileBase RearImage { get; set; }
        public string VehicleImageRightSide
        {
            get
            {
                return this.vehicleImageRightSide;
            }

            set
            {
                this.vehicleImageRightSide = value;
            }
        }

        [Display(Name = "Right Side Image")]
        public System.Web.HttpPostedFileBase RightSideImage { get; set; }

        public string VehicleImageLeftSide
        {
            get
            {
                return this.vehicleImageLeftSide;
            }

            set
            {
                this.vehicleImageLeftSide = value;
            }
        }
        [Display(Name = "Left Side Image")]
        public System.Web.HttpPostedFileBase LeftSideImage { get; set; }
        public int ValidUntil
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("accountId = " + this.accountId + Environment.NewLine);
            sb.Append("entryId = " + this.entryId + Environment.NewLine);
            sb.Append("vehRegNo = " + this.vehRegNo + Environment.NewLine);
            sb.Append("tagId = " + this.tagId + Environment.NewLine);
            sb.Append("vehicleClassId = " + this.vehicleClassId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifierId + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);
            sb.Append("vehicleRegistrationCerticateId = " + this.vehicleRegistrationCerticateId + Environment.NewLine);
            sb.Append("address = " + this.address + Environment.NewLine);
            sb.Append("brand = " + this.brand + Environment.NewLine);
            sb.Append("vehicleType = " + this.vehicleType + Environment.NewLine);
            sb.Append("vehicleCategory = " + this.vehicleCategory + Environment.NewLine);
            sb.Append("modelNo = " + this.modelNo + Environment.NewLine);
            sb.Append("manufacturingYear = " + this.manufacturingYear + Environment.NewLine);
            sb.Append("cyclinderCapacity = " + this.cyclinderCapacity + Environment.NewLine);
            sb.Append("frameNumber = " + this.frameNumber + Environment.NewLine);
            sb.Append("engineNumber = " + this.engineNumber + Environment.NewLine);
            sb.Append("vehicleColor = " + this.vehicleColor + Environment.NewLine);
            sb.Append("fuelType = " + this.fuelType + Environment.NewLine);
            sb.Append("licencePlateColor = " + this.licencePlateColor + Environment.NewLine);
            sb.Append("registrationYear = " + this.registrationYear + Environment.NewLine);
            sb.Append("locationCode = " + this.locationCode + Environment.NewLine);
            sb.Append("vehicleImageFront = " + this.vehicleImageFront + Environment.NewLine);
            sb.Append("vehicleImageRear = " + this.vehicleImageRear + Environment.NewLine);
            sb.Append("VehicleImageRightSide = " + this.VehicleImageRightSide + Environment.NewLine);
            sb.Append("VehicleImageLeftSide = " + this.VehicleImageLeftSide + Environment.NewLine);
            sb.Append("validUntil = " + this.validUntil + Environment.NewLine);
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
