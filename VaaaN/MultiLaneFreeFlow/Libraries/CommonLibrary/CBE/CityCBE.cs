﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CityCBE
    {
        private Int32 tmsId;
        private Int32 provinceId;
        private Int32 cityId;
        private String cityName;
        private Int32 cityCode;
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
        [Display(Name = "Province")]
        [Required]
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
        [Display(Name = "Kabupaten/Kota ID")]
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
        [Display(Name = "Kabupaten/Kota Name")]
        [Required]
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

        public int CityCode
        {
            get
            {
                return this.cityCode;
            }

            set
            {
                this.cityCode = value;
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

        public CityCBE()
        {
            this.tmsId = 0;
            this.provinceId = 0;
            this.cityId = 0;
            this.cityName = string.Empty;
            this.cityCode = 0;
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
            sb.Append("cityId = " + this.cityId + Environment.NewLine);
            sb.Append("cityName = " + this.cityName + Environment.NewLine);
            sb.Append("cityCode = " + this.cityCode + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class CityCBECollection : CollectionBase
    {
        public CityCBECollection()
        {
        }
        public CityCBE this[int index]
        {
            get { return (CityCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CityCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CityCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CityCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CityCBE value)
        {
            List.Remove(value);
        }
    }
}
