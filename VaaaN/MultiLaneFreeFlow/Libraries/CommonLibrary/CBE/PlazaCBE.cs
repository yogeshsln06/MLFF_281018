using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class PlazaCBE
    {
        private Int32 tmsId;
        private Int32 plazaId;
        private string plazaName;
        private string location;
        private string ipAddress;
        private Decimal longitude;
        private Decimal latitude;
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;

        public PlazaCBE()
        {
            this.TmsId = 0;
            this.PlazaId = 0;
            this.PlazaName = string.Empty;
            this.Location = string.Empty;
            this.IpAddress = string.Empty;
            this.Longitude = 0;
            this.Latitude = 0;
            this.ModifierId = 0;
            this.CreationDate = DateTime.Now;
            this.ModificationDate = DateTime.Now;
        }

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

        public int PlazaId
        {
            get
            {
                return this.plazaId;
            }

            set
            {
                this.plazaId = value;
            }
        }
        [Required]
        [Display(Name = "Gantry Name")]
        public string PlazaName
        {
            get
            {
                return this.plazaName;
            }

            set
            {
                this.plazaName = value;
            }
        }
        [Display(Name = "Location")]
        [Required]
        public string Location
        {
            get
            {
                return this.location;
            }

            set
            {
                this.location = value;
            }
        }

        [Required]
       // [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$")]
        public string IpAddress
        {
            get
            {
                return this.ipAddress;
            }

            set
            {
                this.ipAddress = value;
            }
        }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N6}")]
        public Decimal Longitude
        {
            get
            {
                return this.longitude;
            }

            set
            {
                this.longitude = value;
            }
        }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N6}")]
        public Decimal Latitude
        {
            get
            {
                return this.latitude;
            }

            set
            {
                this.latitude = value;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("plazaId = " + this.plazaId + Environment.NewLine);
            sb.Append("plazaName = " + this.plazaName + Environment.NewLine);
            sb.Append("location = " + this.location + Environment.NewLine);
            sb.Append("ipAddress = " + this.ipAddress + Environment.NewLine);
            sb.Append("ModifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);

            return sb.ToString();
        }


    }

    public class PlazaCollection : CollectionBase
    {
        public PlazaCollection()
        {
        }
        public PlazaCBE this[int index]
        {
            get { return (PlazaCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(PlazaCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(PlazaCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, PlazaCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(PlazaCBE value)
        {
            List.Remove(value);
        }
    }
}
