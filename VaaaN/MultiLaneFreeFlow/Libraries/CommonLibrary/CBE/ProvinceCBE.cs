using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class ProvinceCBE
    {
        private Int32 tmsId;
        private Int32 provinceId;
        private String provinceName;
        private Int32 provinceCode;
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

        public int ProvinceCode
        {
            get
            {
                return this.provinceCode;
            }

            set
            {
                this.provinceCode = value;
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

        public ProvinceCBE()
        {
            this.tmsId = 0;
            this.provinceId = 0;
            this.provinceName = String.Empty;
            this.provinceCode = 0;
            this.creationDate = DateTime.Now;
            this.modifierId = 0;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("provinceId = " + this.provinceId + Environment.NewLine);
            sb.Append("provinceName = " + this.provinceName + Environment.NewLine);
            sb.Append("provinceCode = " + this.provinceCode + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);

            return sb.ToString();
        }

    }

    public class ProvinceCBECollection : CollectionBase
    {
        public ProvinceCBECollection()
        {
        }
        public ProvinceCBE this[int index]
        {
            get { return (ProvinceCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(ProvinceCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(ProvinceCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, ProvinceCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(ProvinceCBE value)
        {
            List.Remove(value);
        }
    }

}
