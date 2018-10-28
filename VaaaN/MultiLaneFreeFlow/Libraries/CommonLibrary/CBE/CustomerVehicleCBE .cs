using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerVehicleCBE
    {
        private Int32 tmsId;
        private Int32 accountId;
        private Int32 entryId;
        private String tagId;
        private Int32 vehicleClassId;
        private string vehicleClassName;
        private String vehRegNo;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifierId;
        private Int32 modifiedBy;
        private Int32 transferStatus;
        private Int32 amount;

        public CustomerVehicleCBE()
        {
            this.tmsId = 0;
            this.accountId = 0;
            this.entryId = 0;
            this.tagId = String.Empty;
            this.vehicleClassId = 0;
            this.vehicleClassName = string.Empty;
            this.vehRegNo = String.Empty;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifierId = 0;
            this.modifiedBy = 0;
            this.transferStatus = 0;
            this.amount = 0;
        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        public Int32 AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }
        [Required]
        [Display(Name = "TagId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String TagId
        {
            get { return this.tagId; }
            set { this.tagId = value; }
        }
        [Display(Name = "VehicleClass", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 VehicleClassId
        {
            get { return this.vehicleClassId; }
            set { this.vehicleClassId = value; }
        }

        [Required]
        [Display(Name ="VehicleRegNo", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String VehRegNo
        {
            get { return this.vehRegNo; }
            set { this.vehRegNo = value; }
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

        public Int32 ModifierId
        {
            get { return this.modifierId; }
            set { this.modifierId = value; }
        }


        public Int32 ModifiedBy
        {
            get { return this.modifiedBy; }
            set { this.modifiedBy = value; }
        }
        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }
        [Display(Name = "Vehicle Class")]
        public string VehicleClassName
        {
            get
            {
                return this.vehicleClassName;
            }

            set
            {
                this.vehicleClassName = value;
            }
        }
        [Display(Name = "Amount", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [Required]
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("accountId = " + this.accountId + Environment.NewLine);
            sb.Append("entryId = " + this.entryId + Environment.NewLine);
            sb.Append("vehRegNo = " + this.vehRegNo + Environment.NewLine);
            sb.Append("tagId = " + this.tagId + Environment.NewLine);
            sb.Append("vehicleClassId = " + this.vehicleClassId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifierId + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class CustomerVehicleCollection : CollectionBase
    {
        public CustomerVehicleCollection()
        {
        }
        public CustomerVehicleCBE this[int index]
        {
            get { return (CustomerVehicleCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerVehicleCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerVehicleCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerVehicleCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerVehicleCBE value)
        {
            List.Remove(value);
        }
    }
}
