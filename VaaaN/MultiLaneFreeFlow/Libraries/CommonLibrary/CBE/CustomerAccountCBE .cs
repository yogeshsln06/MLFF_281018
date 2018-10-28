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
        private Decimal accountBalance;
        private Int32 balance;//New Balance For Recharge Amount
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private String customerImagePath;
        private String scannedDocsPath;
        private Int32 isDocVerified;
        private Int32 accountStatus;
        private Int32 transferStatus;


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
            this.scannedDocsPath = String.Empty;
            this.isDocVerified = 0;
            this.accountStatus = 0;
            this.transferStatus = 0;
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
        [Required]
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
        [Required]
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
        [Display(Name = "Customer Image")]
        public String CustomerImagePath
        {
            get { return this.customerImagePath; }
            set { this.customerImagePath = value; }
        }
        [Display(Name = "Scanned Document")]
        public String ScannedDocsPath
        {
            get { return this.scannedDocsPath; }
            set { this.scannedDocsPath = value; }
        }

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
            sb.Append("scannedDocsPath = " + this.scannedDocsPath + Environment.NewLine);
            sb.Append("isDocVerified = " + this.isDocVerified + Environment.NewLine);
            sb.Append("accountStatus = " + this.accountStatus + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);

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
