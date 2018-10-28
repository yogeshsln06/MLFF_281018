using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class TransactionCBE
    {
        #region Variable
        private int tmsId;
        private int plazaId;
        private int laneId;
        private Int64 transactionId;
        private DateTime transactionDateTime; //newly added - EVI timestamp will be considered as transaction time

        //private string crosstalkTagId;
        //private string crosstalkVehicleClassId;
        //private string crosstalkVRN;
        //private string crosstalkTimestamp; //newly added

        //private string nodefluxVRNFront;
        //private string nodefluxVehicleClassIdFront;
        //private string nodefluxTimestampFront; //newly added

        //private string nodefluxVRNRear;
        //private string nodefluxVehicleClassIdRear;
        //private string nodefluxTimestampRear; //newly added

        private int crosstalkEntryId = -1;
        private int nodefluxEntryIdFront = -1;
        private int nodefluxEntryIdRear = -1;

        private int isBalanceUpdated = -1;
        private int isTransfered = -1;
        private int isViolation = -1;
        private int modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;

        private int auditStatus = -1;
        private int auditorId = -1;
        private DateTime auditDate;
        private int auditedVehicleClassId = -1;
        private string auditedVRN = string.Empty;
        #endregion

        #region Properties
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
        public int LaneId
        {
            get
            {
                return this.laneId;
            }
            set
            {
                this.laneId = value;
            }
        }
        public Int64 TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = value;
            }
        }
        public DateTime TransactionDateTime
        {
            get
            {
                return this.transactionDateTime;
            }
            set
            {
                this.transactionDateTime = value;
            }
        }

        #region Commented
        //[Display(Name ="EVI Id")]
        //public string CrosstalkTagId
        //{
        //    get
        //    {
        //        return this.crosstalkTagId;
        //    }
        //    set
        //    {
        //        this.crosstalkTagId = value;
        //    }
        //}
        //[Display(Name = "EVI Class")]
        //public string CrosstalkVehicleClassId
        //{
        //    get
        //    {
        //        return this.crosstalkVehicleClassId;
        //    }
        //    set
        //    {
        //        this.crosstalkVehicleClassId = value;
        //    }
        //}
        //[Display(Name = "EVI VRN")]
        //public string CrosstalkVRN
        //{
        //    get
        //    {
        //        return this.crosstalkVRN;
        //    }
        //    set
        //    {
        //        this.crosstalkVRN = value;
        //    }
        //}
        //public string CrosstalkTimestamp
        //{
        //    get
        //    {
        //        return this.crosstalkTimestamp;
        //    }
        //    set
        //    {
        //        this.crosstalkTimestamp = value;
        //    }
        //}

        //[Display(Name = "AVC VRN FRONT")]
        //public string NodefluxVRNFront
        //{
        //    get
        //    {
        //        return this.nodefluxVRNFront;
        //    }
        //    set
        //    {
        //        this.nodefluxVRNFront = value;
        //    }
        //}
        //[Display(Name = "AVC Class FRONT")]
        //public string NodefluxVehicleClassIdFront
        //{
        //    get
        //    {
        //        return this.nodefluxVehicleClassIdFront;
        //    }
        //    set
        //    {
        //        this.nodefluxVehicleClassIdFront = value;
        //    }
        //}
        //public string NodefluxTimestampFront
        //{
        //    get
        //    {
        //        return this.nodefluxTimestampFront;
        //    }
        //    set
        //    {
        //        this.nodefluxTimestampFront = value;
        //    }
        //}

        //[Display(Name = "AVC VRN REAR")]
        //public string NodefluxVRNRear
        //{
        //    get
        //    {
        //        return this.nodefluxVRNRear;
        //    }
        //    set
        //    {
        //        this.nodefluxVRNRear = value;
        //    }
        //}
        //[Display(Name = "AVC Class REAR")]
        //public string NodefluxVehicleClassIdRear
        //{
        //    get
        //    {
        //        return this.nodefluxVehicleClassIdRear;
        //    }
        //    set
        //    {
        //        this.nodefluxVehicleClassIdRear = value;
        //    }
        //}
        //public string NodefluxTimestampRear
        //{
        //    get
        //    {
        //        return this.nodefluxTimestampRear;
        //    }
        //    set
        //    {
        //        this.nodefluxTimestampRear = value;
        //    }
        //}
        #endregion

        public int CrosstalkEntryId
        {
            get
            {
                return this.crosstalkEntryId;
            }
            set
            {
                this.crosstalkEntryId = value;
            }
        }
        public int NodefluxEntryIdFront
        {
            get
            {
                return this.nodefluxEntryIdFront;
            }
            set
            {
                this.nodefluxEntryIdFront = value;
            }
        }
        public int NodefluxEntryIdRear
        {
            get
            {
                return this.nodefluxEntryIdRear;
            }
            set
            {
                this.nodefluxEntryIdRear = value;
            }
        }

        public int IsBalanceUpdated
        {
            get
            {
                return this.isBalanceUpdated;
            }
            set
            {
                this.isBalanceUpdated = value;
            }
        }
        public int IsTransfered
        {
            get
            {
                return this.isTransfered;
            }
            set
            {
                this.isTransfered = value;
            }
        }
        public int IsViolation
        {
            get
            {
                return this.isViolation;
            }
            set
            {
                this.isViolation = value;
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

        public int AuditStatus
        {
            get
            {
                return this.auditStatus;
            }
            set
            {
                this.auditStatus = value;
            }
        }
        public int AuditorId
        {
            get
            {
                return this.auditorId;
            }
            set
            {
                this.auditorId = value;
            }
        }
        public DateTime AuditDate
        {
            get
            {
                return this.auditDate;
            }
            set
            {
                this.auditDate = value;
            }
        }
        public int AuditedVehicleClassId
        {
            get
            {
                return this.auditedVehicleClassId;
            }
            set
            {
                this.auditedVehicleClassId = value;
            }
        }
        public string AuditedVRN
        {
            get
            {
                return this.auditedVRN;
            }
            set
            {
                this.auditedVRN = value;
            }
        }
        #endregion

        public override string ToString()
        {
            return base.ToString(); //to do
        }

    }

    public class TransactionCollection : CollectionBase
    {
        public TransactionCollection()
        {
        }
        public TransactionCBE this[int index]
        {
            get { return (TransactionCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(TransactionCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(TransactionCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, TransactionCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(TransactionCBE value)
        {
            List.Remove(value);
        }
    }
}
