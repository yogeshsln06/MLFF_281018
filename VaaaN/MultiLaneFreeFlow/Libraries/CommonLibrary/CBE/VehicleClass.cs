using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class VehicleClassCBE
    {
        int tmsId;
        int id;
        string name;
        int modifierId;
        DateTime creationDate;
        DateTime modificationDate;
        Int32 transferStatus;

        
        public VehicleClassCBE()
        {
            this.tmsId = 0;
            this.id = 0;
            this.name = string.Empty;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 0;
        }
        public int TMSId
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
        public Int32 Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        [Display(Name = "Name", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public Int32 ModifierId
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

        

      

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Id = " + this.id + Environment.NewLine);
            sb.Append("Name = " + this.name + Environment.NewLine);
            sb.Append("TransferStatus = " + this.transferStatus + Environment.NewLine);
            return sb.ToString();
        }
    }

    public class VehicleClassCollection : CollectionBase
    {
        public VehicleClassCollection()
        {
        }
        public VehicleClassCBE this[int index]
        {
            get { return (VehicleClassCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(VehicleClassCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(VehicleClassCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, VehicleClassCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(VehicleClassCBE value)
        {
            List.Remove(value);
        }
    }
}
