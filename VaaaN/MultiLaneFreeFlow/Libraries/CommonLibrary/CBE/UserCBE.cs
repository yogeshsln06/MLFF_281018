using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserCBE
    {
        private Int32 userId;
       
        private String loginName;
        private String firstName;
        private String lastName;
        private String fullName;
        private String description;
        private String password;
        private String address;
        private int roleId;
        private String roleName;
        private DateTime accountExpiryDate;
        private int modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private int transferStatus;
        private string fingerPrint1;
        public String user_profile_pic_name;
        public DateTime user_dob;
        public String mobile_no;
        public String email_id;
        public bool user_status;
        public UserCBE()
        {
            this.userId = 0;
         
            this.loginName = String.Empty;
            this.firstName = String.Empty;
            this.lastName = String.Empty;
            this.fullName = String.Empty;
            this.description = String.Empty;
            this.password = String.Empty;
            this.address = String.Empty;
            this.roleId = 0;
            this.roleName = String.Empty;
            this.accountExpiryDate = DateTime.Now.AddYears(20);
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 1;
            this.fingerPrint1 = string.Empty;
            this.user_profile_pic_name = string.Empty;
            this.user_dob = DateTime.Now.AddYears(-18);
            this.mobile_no = string.Empty;
            this.email_id = string.Empty;
            this.user_status = false;
        }

        public Int32 UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UserDob
        {
            get { return this.user_dob; }
            set { this.user_dob = value; }
        }
        [Display(Name = "MobileNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Number must be numeric")]
        public String MobileNo
        {
            get { return this.mobile_no; }
            set { this.mobile_no = value; }
        }
        [Display(Name = "EmailId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String EmailId
        {
            get { return this.email_id; }
            set { this.email_id = value; }
        }
       
        [Display(Name = "Username")]
        [Required]
        public String LoginName
        {
            get { return this.loginName; }
            set { this.loginName = value; }
        }
        [Display(Name = "Name")]
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

        public String FullName
        {
            get { return this.fullName; }
            set { this.fullName = value; }
        }
        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [Display(Name = "Password")]
        [Required]
        public String Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

       // [Display(Name = "Address", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        //[Display(Name = "RoleName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 RoleId
        {
            get { return this.roleId; }
            set { this.roleId = value; }
        }
        [Display(Name = "Role Name")]
        // [Display(Name = "RoleName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String RoleName
        {
            get { return this.roleName; }
            set { this.roleName = value; }
        }
        [Display(Name = "Account Expiry Date")]
        //[Display(Name = "AccountExpiryDate", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AccountExpiryDate
        {
            get { return this.accountExpiryDate; }
            set { this.accountExpiryDate = value; }
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
        public int TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        [Display(Name = "User Profile Picture")]
        public HttpPostedFileBase UserProfilePic { get; set; }

        public String UserProfilePicName
        {
            get { return this.user_profile_pic_name; }
            set { this.user_profile_pic_name = value; }
        }
        public bool UserStatus
        {
            get { return this.user_status; }
            set { this.user_status = value; }
        }
        public string FingerPrint1
        {
            get { return this.fingerPrint1; }
            set { this.fingerPrint1 = value; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("userId = " + this.userId + Environment.NewLine);
          
            sb.Append("loginName = " + this.loginName + Environment.NewLine);
            sb.Append("firstName = " + this.firstName + Environment.NewLine);
            sb.Append("lastName = " + this.lastName + Environment.NewLine);
            sb.Append("description = " + this.description + Environment.NewLine);
            sb.Append("password = " + this.password + Environment.NewLine);
            sb.Append("address = " + this.address + Environment.NewLine);
            sb.Append("roleId = " + this.roleId + Environment.NewLine);
            sb.Append("roleName = " + this.roleName + Environment.NewLine);
            sb.Append("accountExpiryDate = " + this.accountExpiryDate + Environment.NewLine);
            sb.Append("ModifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate);

            return sb.ToString();
        }
    }

    public class UserCollection : CollectionBase
    {
        public UserCollection()
        {
        }
        public UserCBE this[int index]
        {
            get { return (UserCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(UserCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, UserCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserCBE value)
        {
            List.Remove(value);
        }
    }
}
