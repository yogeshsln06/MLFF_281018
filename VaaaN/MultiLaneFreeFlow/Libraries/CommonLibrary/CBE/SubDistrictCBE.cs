using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class SubDistrictCBE
    {
        private Int32 tmsId;
        private Int32 districtId;
        private Int32 subdistrictId;
        private String subdistrictName;
        private Int32 subdistrictCode;
        private DateTime creationDate;
        private Int32 modifierId;
        private DateTime modificationDate;
        private Int32 transferStatus;
        private Int32 zipCode;
        
        public int TmsId
        {
            get
            {
                return this.tmsId;
            }

            set
            {
                this.tmsId = value;
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
        [Display(Name = "Kelurahan/Desa ID")]
        [Required]
        public int SubDistrictId
        {
            get
            {
                return this.subdistrictId;
            }

            set
            {
                this.subdistrictId = value;
            }
        }
        [Display(Name = "Kelurahan/Desa Name")]
        [Required]
        public string SubDistrictName
        {
            get
            {
                return this.subdistrictName;
            }

            set
            {
                this.subdistrictName = value;
            }
        }

        public int SubDistrictCode
        {
            get
            {
                return this.subdistrictCode;
            }

            set
            {
                this.subdistrictCode = value;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this.creationDate;
            }

            set
            {
                this.creationDate = value;
            }
        }

        public int ModifierId
        {
            get
            {
                return this.modifierId;
            }

            set
            {
                this.modifierId = value;
            }
        }

        public DateTime ModificationDate
        {
            get
            {
                return this.modificationDate;
            }

            set
            {
                this.modificationDate = value;
            }
        }

        public int TransferStatus
        {
            get
            {
                return this.transferStatus;
            }

            set
            {
                this.transferStatus = value;
            }
        }

        public int ZipCode
        {
            get
            {
                return this.zipCode;
            }

            set
            {
                this.zipCode = value;
            }
        }

        public SubDistrictCBE()
        {
            this.tmsId = 0;
            this.districtId = 0;
            this.subdistrictId = 0;
            this.subdistrictName = string.Empty;
            this.subdistrictCode = 0;
            this.creationDate = DateTime.Now;
            this.modifierId = 0;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 0;
            this.zipCode = 00000;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("districtId = " + this.districtId + Environment.NewLine);
            sb.Append("subdistrictId = " + this.subdistrictId + Environment.NewLine);
            sb.Append("subdistrictName = " + this.subdistrictName + Environment.NewLine);
            sb.Append("subdistrictCode = " + this.subdistrictCode + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("zipCode = " + this.zipCode + Environment.NewLine);

            return sb.ToString();
        }
    }
    public class SubDistrictCBECollection : CollectionBase
    {
        public SubDistrictCBECollection()
        {
        }
        public SubDistrictCBE this[int index]
        {
            get { return (SubDistrictCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(SubDistrictCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(SubDistrictCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, SubDistrictCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(SubDistrictCBE value)
        {
            List.Remove(value);
        }
    }
}
