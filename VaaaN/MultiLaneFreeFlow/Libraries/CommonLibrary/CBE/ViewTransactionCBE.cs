using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class ViewTransactionCBE
    {
        [Display(Name = "Select Report")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Int32 ReportCategory { get; set; }

        //[Display(Name = "End Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime EndTime { get; set; }

        [Display(Name = "From Date")]
        public String StartDate { get; set; }

        [Display(Name = "End Date")]
        public String EndDate { get; set; }

        [Display(Name = "Plate Number")]
        public string PlateNumber { get; set; }

        [Display(Name = "Gantry")]
        public Int32 GantryId { get; set; }

        [Display(Name = "Vehicle Class")]
        public Int32 VehicleClassId { get; set; }
        [Display(Name = "Transaction Category")]
        public Int32 TransactionCategoryId { get; set; }
        public List<CBE.TransactionCBE> Transaction { get; set; }

        [Display(Name = "Mobile Number")]
        public String MobileNo { get; set; }
    }
}
