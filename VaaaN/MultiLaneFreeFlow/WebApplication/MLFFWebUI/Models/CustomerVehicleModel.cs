using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace MLFFWebUI.Models
{
    public class CustomerVehicleModel : CustomerVehicleCBE
    {
        private String firstName;
        private String mobileNo;
        private String emailId;
        private String residentId;
        private String address;
        private bool searchEnable;


        [Required]
        [Display(Name = "Name", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        [Required]
        [Display(Name = "Mobile Phone")]
        // [Display(Name = "MobileNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Number must be numeric")]
        public String MobileNo
        {
            get { return this.mobileNo; }
            set { this.mobileNo = value; }
        }

        [Required]
        //[Display(Name = "EmailId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Display(Name = "Email")]
        public String EmailId
        {
            get { return this.emailId; }
            set { this.emailId = value; }
        }

        [Display(Name = "Address", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Required]
        public String Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        [Display(Name = "Resident ID")]//Resident Identity Number
        [Required]
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

        public bool ImageFrontChnage { get; set; }
        public bool ImageRearChnage { get; set; }
        public bool ImageLeftChnage { get; set; }
        public bool ImageRightChnage { get; set; }
        public bool RCNumberImageChnage { get; set; }
        public bool SearchEnable { get; set; }
        public bool SendEmail { get; set; }
    }
}