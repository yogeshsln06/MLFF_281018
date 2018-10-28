using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class TollRateCBE
    {
        private Int32 tmsId;
        private Int32 plazaId;
        private Int32 laneTypeId;
        private string laneTypeName;
        private Int32 transferStatus;
        private Int32 rateId;
        private Int32 profileId;
        private Int32 vehicleClassId;
        private string vehicleClassName;
        private string startTime;
        private string endTime;
        private Decimal amount;
        private String description;
        private Int32 modifiedBy;
        private DateTime creationDate;
        private DateTime modificationDate;

        public TollRateCBE()
        {
            this.tmsId = 0;
            this.plazaId = 0;
            this.transferStatus = 0;
            this.rateId = 0;
            this.profileId = 0;
            this.vehicleClassId = 0;
            this.VehicleClassName = string.Empty;
            this.laneTypeId = 0;
            this.laneTypeName = string.Empty;
            this.startTime = string.Empty;
            this.endTime = string.Empty;
            this.amount = 0;
            this.description = string.Empty;
            this.modifiedBy = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        public Int32 PlazaId
        {
            get { return this.plazaId; }
            set { this.plazaId = value; }
        }


        public Int32 RateId
        {
            get { return this.rateId; }
            set { this.rateId = value; }
        }
        public Int32 ProfileId
        {
            get { return this.profileId; }
            set { this.profileId = value; }
        }

        public Int32 VehicleClassId
        {
            get { return this.vehicleClassId; }
            set { this.vehicleClassId = value; }
        }

        /// <summary>
        /// Normal Lane =1, Trans Jakarta Lane =2
        /// </summary>
        public Int32 LaneTypeId
        {
            get { return this.laneTypeId; }
            set { this.laneTypeId = value; }
        }

        [Display(Name = "StartTime", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }
        [Display(Name = "EndTime", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }
        [Display(Name = "Amount", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Decimal Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public Int32 ModifiedBy
        {
            get { return this.modifiedBy; }
            set { this.modifiedBy = value; }
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
            get
            {
                return this.transferStatus;
            }

            set
            {
                this.transferStatus = value;
            }
        }

        [Display(Name = "VehicleClass", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
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

        [Display(Name = "LaneName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string LaneTypeName
        {
            get
            {
                return this.laneTypeName;
            }

            set
            {
                this.laneTypeName = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("plazaId = " + this.plazaId + Environment.NewLine);
            sb.Append("rateId = " + this.rateId + Environment.NewLine);
            sb.Append("profileId = " + this.profileId + Environment.NewLine);
            sb.Append("vehicleClassId = " + this.vehicleClassId + Environment.NewLine);
            sb.Append("laneId = " + this.laneTypeId + Environment.NewLine);
            sb.Append("startTime = " + this.startTime + Environment.NewLine);
            sb.Append("endTime = " + this.endTime + Environment.NewLine);
            sb.Append("amount = " + this.amount + Environment.NewLine);
            sb.Append("description = " + this.description + Environment.NewLine);
            sb.Append("modifiedBy = " + this.creationDate + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class TollRateCollection : CollectionBase
    {
        public TollRateCollection()
        {
        }
        public TollRateCBE this[int index]
        {
            get { return (TollRateCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(TollRateCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(TollRateCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, TollRateCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(TollRateCBE value)
        {
            List.Remove(value);
        }
    }
}
