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
        private String adressLine1;
        private Decimal accountBalance;
        private Int32 balance;//New Balance For Recharge Amount
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private String customerImagePath;
        private String scannedDocsPath1;
        private Int32 isDocVerified;
        private Int32 accountStatus;
        private Int32 transferStatus;
        private String scannedDocsPath2;
        private String scannedDocsPath3;
        private String scannedDocsPath4;
        private Int32 residentId;
        private String birthPlace;
        private Nullable<DateTime> birthDate;
        private Int32 gender;
        private String addressLine2;
        private String addressLine3;
        private Int32 districtId;
        private Int32 subDistrictId;
        private Int32 cityId;
        private Int32 provinceId;
        private Int32 postalCode;
        private Int32 maritalStatus;
        private String occupation;
        private Int32 nationality;
        private Nullable<DateTime> validUntil;
        private Int32 registartionThrough;
        private Int32 queueStatus;
        private String userPassword;

        public CustomerAccountCBE()
        {
            this.tmsId = 0;
            this.accountId = 0;
            this.firstName = String.Empty;
            this.lastName = String.Empty;
            this.mobileNo = String.Empty;
            this.emailId = string.Empty;
            this.description = String.Empty;
            this.adressLine1 = String.Empty;
            this.accountBalance = 0;
            this.Balance = 0;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.customerImagePath = String.Empty;
            this.scannedDocsPath1 = String.Empty;
            this.isDocVerified = 0;
            this.accountStatus = 0;
            this.transferStatus = 0;
            this.scannedDocsPath2 = string.Empty;
            this.scannedDocsPath3 = string.Empty;
            this.scannedDocsPath4 = string.Empty;
            this.residentId = 0;
            this.birthPlace = String.Empty;
            this.birthDate = null;
            this.gender = 0;
            this.addressLine2 = string.Empty;
            this.addressLine3 = string.Empty;
            this.districtId = 0;
            this.subDistrictId = 0;
            this.cityId = 0;
            this.provinceId = 0;
            this.postalCode = 0;
            this.maritalStatus = 0;
            this.occupation = string.Empty;
            this.nationality = 0;
            this.validUntil = null;
            this.registartionThrough = 0;
            this.queueStatus = 0;
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
        [Display(Name ="FirstName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
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
        public String AddressLine1
        {
            get { return this.adressLine1; }
            set { this.adressLine1 = value; }
        }
       
        [Display(Name = "Account Balance", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
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
        [Display(Name = "Customer Image")]
        public String CustomerImagePath
        {
            get { return this.customerImagePath; }
            set { this.customerImagePath = value; }
        }
        [Display(Name = "Scanned Document")]
        public String ScannedDocsPath1
        {
            get { return this.scannedDocsPath1; }
            set { this.scannedDocsPath1 = value; }
        }

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
        [Display(Name = "Customer Image")]
        public System.Web.HttpPostedFileBase CustomerImage { get; set; }
        [Display(Name = "Scanned Document")]
        public System.Web.HttpPostedFileBase ScannedDocument { get; set; }
        [Display(Name = "Document 2")]
        public string ScannedDocsPath2
        {
            get
            {
                return this.scannedDocsPath2;
            }

            set
            {
                this.scannedDocsPath2 = value;
            }
        }
        [Display(Name = "Document 3")]
        public string ScannedDocsPath3
        {
            get
            {
                return this.scannedDocsPath3;
            }

            set
            {
                this.scannedDocsPath3 = value;
            }
        }
        [Display(Name = "Document 4")]
        public string ScannedDocsPath4
        {
            get
            {
                return this.scannedDocsPath4;
            }

            set
            {
                this.scannedDocsPath4 = value;
            }
        }
        [Display(Name = "eKTP")]//Resident Identity Number
        public int ResidentId
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
        [Display(Name = "Address Line 2")]
        public string AddressLine2
        {
            get
            {
                return this.addressLine2;
            }

            set
            {
                this.addressLine2 = value;
            }
        }
        [Display(Name = "Address Line 3")]
        public string AddressLine3
        {
            get
            {
                return this.addressLine3;
            }

            set
            {
                this.addressLine3 = value;
            }
        }
        [Display(Name = "District")]
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
        [Display(Name = "Sub District")]
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
        [Display(Name = "City")]
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
        [Display(Name = "Registartion Through")]
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
        [Display(Name = "Queue Status")]
        public int QueueStatus
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
            sb.Append("addressLine1 = " + this.adressLine1 + Environment.NewLine);
            sb.Append("accountBalance = " + this.accountBalance + Environment.NewLine);
            sb.Append("ModifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("customerImagePath = " + this.customerImagePath + Environment.NewLine);
            sb.Append("scannedDocsPath1 = " + this.scannedDocsPath1 + Environment.NewLine);
            sb.Append("isDocVerified = " + this.isDocVerified + Environment.NewLine);
            sb.Append("accountStatus = " + this.accountStatus + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);
            sb.Append("scannedDocsPath2 = " + this.scannedDocsPath2 + Environment.NewLine);
            sb.Append("scannedDocsPath3 = " + this.scannedDocsPath3 + Environment.NewLine);
            sb.Append("scannedDocsPath4 = " + this.scannedDocsPath4 + Environment.NewLine);
            sb.Append("residentId = " + this.residentId + Environment.NewLine);
            sb.Append("birthPlace = " + this.birthPlace + Environment.NewLine);
            sb.Append("birthDate = " + this.birthDate + Environment.NewLine);
            sb.Append("gender = " + this.gender + Environment.NewLine);
            sb.Append("addressLine2 = " + this.addressLine2 + Environment.NewLine);
            sb.Append("addressLine3 = " + this.addressLine3 + Environment.NewLine);
            sb.Append("district = " + this.districtId + Environment.NewLine);
            sb.Append("subDistrict = " + this.subDistrictId + Environment.NewLine);
            sb.Append("city = " + this.cityId + Environment.NewLine);
            sb.Append("province = " + this.provinceId + Environment.NewLine);
            sb.Append("postalCode = " + this.postalCode + Environment.NewLine);
            sb.Append("maritalStatus = " + this.maritalStatus + Environment.NewLine);
            sb.Append("occupation = " + this.occupation + Environment.NewLine);
            sb.Append("nationality = " + this.nationality + Environment.NewLine);
            sb.Append("validUntil = " + this.validUntil + Environment.NewLine);
            sb.Append("registartionThrough = " + this.registartionThrough + Environment.NewLine);
            sb.Append("queueStatus = " + this.queueStatus + Environment.NewLine);
            sb.Append("userPassword = " + this.userPassword + Environment.NewLine);
            return sb.ToString();
        }

        /*
Birthplace
Birthdate
Address
RT/RW
Kelurahan/Desa
Kecamatan
Kabupaten/Kota
Postal Code
Province
Nationality
Gender
Marital Status
Occupation
Photo
Mobile Phone Number
Email Address
Valid Until

        
        */
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
