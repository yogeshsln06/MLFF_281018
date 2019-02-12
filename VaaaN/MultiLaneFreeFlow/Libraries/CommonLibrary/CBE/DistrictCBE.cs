using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class DistrictCBE
    {
        private Int32 tmsId;
        private Int32 cityId;
        private Int32 districtId;
        private String districtName;
        private Int32 districtCode;
        private DateTime creationDate;
        private Int32 modifierId;
        private DateTime modificationDate;
        private Int32 transferStatus;

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

        public int DistrictCode
        {
            get
            {
                return this.districtCode;
            }

            set
            {
                this.districtCode = value;
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

        public DistrictCBE()
        {
            this.tmsId = 0;
            this.cityId = 0;
            this.districtId = 0;
            this.districtName = string.Empty;
            this.districtCode = 0;
            this.creationDate = DateTime.Now;
            this.modifierId = 0;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 0;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("districtId = " + this.districtId + Environment.NewLine);
            sb.Append("districtName = " + this.districtName + Environment.NewLine);
            sb.Append("districtCode = " + this.districtCode + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class DistrictCBECollection : CollectionBase
    {
        public DistrictCBECollection()
        {
        }
        public DistrictCBE this[int index]
        {
            get { return (DistrictCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(DistrictCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(DistrictCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, DistrictCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(DistrictCBE value)
        {
            List.Remove(value);
        }
    }
}
