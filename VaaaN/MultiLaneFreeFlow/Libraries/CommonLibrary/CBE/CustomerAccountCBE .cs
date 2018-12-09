using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerAccountCBE
    {
        private Int32 tmsId;
        private Int32 accountId;
        private String firstName;
        private String lastName;
        private String mobileNo;
        private String emailId;
        private String description;
        private String address;
        private String fullAddress;
        private Decimal accountBalance;
        private Int32 balance;//New Balance For Recharge Amount
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private String customerImagePath;
        private Int32 isDocVerified;
        private Int32 accountStatus;
        private Int32 transferStatus;
        private String residentId;
        private String birthPlace;
        private Nullable<DateTime> birthDate;
        private String rt;
        private String rw;
        private String rt_rw;
        private Int32 provinceId;
        private string provinceName;
        private Int32 cityId;
        private string cityName;
        private Int32 districtId;
        private string districtName;
        private Int32 subDistrictId;
        private string subDistrictName;
        private Int32 postalCode;
        private Int32 nationality;
        private string nationalityName;
        private Int32 gender;
        private string genderName;
        private Int32 maritalStatus;
        private string maritalStatusName;
        private String occupation;
        private String residentidcardimagePath;
        private Nullable<DateTime> validUntil;
        private Int32 registartionThrough;
        private String userPassword;
        //private String adressLine1;
        //private String scannedDocsPath1;
        //private String scannedDocsPath2;
        //private String scannedDocsPath3;
        //private String scannedDocsPath4;
        //private String addressLine2;
        //private String addressLine3;
        //private Int32 queueStatus;


        public CustomerAccountCBE()
        {
            this.tmsId = 0;
            this.accountId = 0;
            this.firstName = String.Empty;
            this.lastName = String.Empty;
            this.mobileNo = String.Empty;
            this.emailId = string.Empty;
            this.description = String.Empty;
            this.address = String.Empty;
            this.accountBalance = 0;
            this.Balance = 0;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.customerImagePath = String.Empty;
            this.isDocVerified = 0;
            this.accountStatus = 0;
            this.transferStatus = 0;
            this.residentId = string.Empty;
            this.birthPlace = String.Empty;
            this.birthDate = null;
            this.rt = String.Empty;
            this.rw = String.Empty;
            this.provinceId = 0;
            this.provinceName = String.Empty;
            this.cityId = 0;
            this.cityName = String.Empty;
            this.districtId = 0;
            this.districtName = String.Empty;
            this.subDistrictId = 0;
            this.subDistrictName = String.Empty;
            this.postalCode = 0;
            this.nationality = 0;
            this.nationalityName = String.Empty;
            this.gender = 0;
            this.genderName = String.Empty;
            this.maritalStatus = 0;
            this.maritalStatusName = String.Empty;
            this.occupation = string.Empty;
            this.residentidcardimagePath = string.Empty;
            this.validUntil = null;
            this.registartionThrough = 0;
            this.userPassword = string.Empty;
        }

        public Int32 TmsId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        public Int32 AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        [Display(Name = "LastName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }
        [Required]
        [Display(Name = "MobileNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String MobileNo
        {
            get { return this.mobileNo; }
            set { this.mobileNo = value; }
        }
        [Required]
        [Display(Name = "EmailId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String EmailId
        {
            get { return this.emailId; }
            set { this.emailId = value; }
        }
        [Required]
        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        [Display(Name = "Address", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Required]
        public String Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public String FullAddress
        {
            get { return this.fullAddress; }
            set { this.fullAddress = value; }
        }

        [Display(Name = "AccountBalance", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Decimal AccountBalance
        {
            get { return this.accountBalance; }
            set { this.accountBalance = value; }
        }
        public int Balance
        {
            get
            {
                return this.balance;
            }

            set
            {
                this.balance = value;
            }
        }
        public Int32 ModifierId
        {
            get { return this.modifierId; }
            set { this.modifierId = value; }
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

        public String CustomerImagePath
        {
            get { return this.customerImagePath; }
            set { this.customerImagePath = value; }
        }

        [Display(Name = "Profile Image")]
        public System.Web.HttpPostedFileBase CustomerImage { get; set; }

        [Display(Name = "Document Verified")]
        public Int32 IsDocVerified
        {
            get { return this.isDocVerified; }
            set { this.isDocVerified = value; }
        }

        public Int32 AccountStatus
        {
            get { return this.accountStatus; }
            set { this.accountStatus = value; }
        }

        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }


        [Display(Name = "eKTP")]//Resident Identity Number
        public string ResidentId
        {
            get
            {
                return this.residentId;
            }

            set
            {
                this.residentId = value;
            }
        }

        [Display(Name = "Birth Place")]
        public string BirthPlace
        {
            get
            {
                return this.birthPlace;
            }

            set
            {
                this.birthPlace = value;
            }
        }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public Nullable<DateTime> BirthDate
        {
            get
            {
                return this.birthDate;
            }

            set
            {
                this.birthDate = value;
            }
        }

        [Display(Name = "RT")]
        public string RT
        {
            get
            {
                return this.rt;
            }

            set
            {
                this.rt = value;
            }
        }

        [Display(Name = "RW")]
        public string RW
        {
            get
            {
                return this.rw;
            }

            set
            {
                this.rw = value;
            }
        }

        [Display(Name = "RT/RW")]
        public string RT_RW
        {
            get
            {
                return this.rt_rw;
            }

            set
            {
                this.rt_rw = value;
            }
        }


        [Display(Name = "Province")]
        public int ProvinceId
        {
            get
            {
                return this.provinceId;
            }

            set
            {
                this.provinceId = value;
            }
        }

        public string ProvinceName
        {
            get
            {
                return this.provinceName;
            }

            set
            {
                this.provinceName = value;
            }
        }

        [Display(Name = "Kabupaten/Kota")]
        public int CityId
        {
            get
            {
                return this.cityId;
            }

            set
            {
                this.cityId = value;
            }
        }

        public string CityName
        {
            get
            {
                return this.cityName;
            }

            set
            {
                this.cityName = value;
            }
        }

        [Display(Name = "Kecamatan")]
        public int DistrictId
        {
            get
            {
                return this.districtId;
            }

            set
            {
                this.districtId = value;
            }
        }

        public string DistrictName
        {
            get
            {
                return this.districtName;
            }

            set
            {
                this.districtName = value;
            }
        }

        [Display(Name = "Kelurahan/Desa")]
        public int SubDistrictId
        {
            get
            {
                return this.subDistrictId;
            }

            set
            {
                this.subDistrictId = value;
            }
        }

        public string SubDistrictName
        {
            get
            {
                return this.subDistrictName;
            }

            set
            {
                this.subDistrictName = value;
            }
        }

        [Display(Name = "Postal Code")]
        public int PostalCode
        {
            get
            {
                return this.postalCode;
            }

            set
            {
                this.postalCode = value;
            }
        }

        public int Nationality
        {
            get
            {
                return this.nationality;
            }

            set
            {
                this.nationality = value;
            }
        }

        public string NationalityName
        {
            get
            {
                return this.nationalityName;
            }

            set
            {
                this.nationalityName = value;
            }
        }

        public int Gender
        {
            get
            {
                return this.gender;
            }

            set
            {
                this.gender = value;
            }
        }
        public string GenderName
        {
            get
            {
                return this.genderName;
            }

            set
            {
                this.genderName = value;
            }
        }

        [Display(Name = "Marital Status")]
        public int MaritalStatus
        {
            get
            {
                return this.maritalStatus;
            }

            set
            {
                this.maritalStatus = value;
            }
        }

        public string MaritalStatusName
        {
            get
            {
                return this.maritalStatusName;
            }

            set
            {
                this.maritalStatusName = value;
            }
        }
        public string Occupation
        {
            get
            {
                return this.occupation;
            }

            set
            {
                this.occupation = value;
            }
        }

        public string ResidentidcardImagePath
        {
            get
            {
                return this.residentidcardimagePath;
            }

            set
            {
                this.residentidcardimagePath = value;
            }
        }

        [Display(Name = "Resident Id Card Image")]
        public System.Web.HttpPostedFileBase ResidentidImage { get; set; }

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

        public int RegistartionThrough
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

        [Display(Name = "Password")]
        public string UserPassword
        {
            get { return this.userPassword; }
            set { this.userPassword = value; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("accountId = " + this.accountId + Environment.NewLine);
            sb.Append("firstName = " + this.firstName + Environment.NewLine);
            sb.Append("lastName = " + this.lastName + Environment.NewLine);
            sb.Append("mobileNo = " + this.mobileNo + Environment.NewLine);
            sb.Append("emailId = " + this.emailId + Environment.NewLine);
            sb.Append("description = " + this.description + Environment.NewLine);
            sb.Append("address = " + this.address + Environment.NewLine);
            sb.Append("accountBalance = " + this.accountBalance + Environment.NewLine);
            sb.Append("ModifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("customerImagePath = " + this.customerImagePath + Environment.NewLine);
            sb.Append("isDocVerified = " + this.isDocVerified + Environment.NewLine);
            sb.Append("accountStatus = " + this.accountStatus + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);
            sb.Append("residentId = " + this.residentId + Environment.NewLine);
            sb.Append("birthPlace = " + this.birthPlace + Environment.NewLine);
            sb.Append("birthDate = " + this.birthDate + Environment.NewLine);
            sb.Append("rt = " + this.rt + Environment.NewLine);
            sb.Append("rw = " + this.rw + Environment.NewLine);
            sb.Append("province = " + this.provinceId + Environment.NewLine);
            sb.Append("city = " + this.cityId + Environment.NewLine);
            sb.Append("district = " + this.districtId + Environment.NewLine);
            sb.Append("subDistrict = " + this.subDistrictId + Environment.NewLine);
            sb.Append("postalCode = " + this.postalCode + Environment.NewLine);
            sb.Append("nationality = " + this.nationality + Environment.NewLine);
            sb.Append("gender = " + this.gender + Environment.NewLine);
            sb.Append("maritalStatus = " + this.maritalStatus + Environment.NewLine);
            sb.Append("occupation = " + this.occupation + Environment.NewLine);
            sb.Append("residentidcardimagePath = " + this.residentidcardimagePath + Environment.NewLine);
            sb.Append("validUntil = " + this.validUntil + Environment.NewLine);
            sb.Append("registartionThrough = " + this.registartionThrough + Environment.NewLine);
            sb.Append("userPassword = " + this.userPassword + Environment.NewLine);
            return sb.ToString();
        }

    }

    public class CustomerAccountCollection : CollectionBase
    {
        public CustomerAccountCollection()
        {
        }
        public CustomerAccountCBE this[int index]
        {
            get { return (CustomerAccountCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerAccountCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerAccountCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerAccountCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerAccountCBE value)
        {
            List.Remove(value);
        }
    }
}
