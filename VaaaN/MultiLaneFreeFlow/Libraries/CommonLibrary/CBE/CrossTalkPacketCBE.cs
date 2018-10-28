using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    [Serializable]
    public class CrossTalkPacketCBE
    {
        #region Variables
        int tmsId;
        int plazaId;
        int laneId;
        int entryId;
        string eventType;
        string timeStamp;
        string uuid;
        string locationId;
        string parentUUID;
        string objectId;
        string firstRead;
        string lastRead;
        string observationUUID;
        string reads;
        int modifierId;
        DateTime creationDate;
        DateTime modificationDate;
        #endregion

        #region Property
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

        public int EntryId
        {
            get
            {
                return this.entryId;
            }
            set
            {
                this.entryId = value;
            }
        }
        public string EventType
        {
            get
            {
                return this.eventType;
            }
            set
            {
                this.eventType = value;
            }
        }
        public string TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
            }
        }
        public string UUID
        {
            get
            {
                return this.uuid;
            }
            set
            {
                this.uuid = value;
            }
        }
        public string LocationId
        {
            get
            {
                return this.locationId;
            }
            set
            {
                this.locationId = value;
            }
        }
        public string ParentUUID
        {
            get
            {
                return this.parentUUID;
            }
            set
            {
                this.parentUUID = value;
            }
        }
        public string ObjectId
        {
            get
            {
                return this.objectId;
            }
            set
            {
                this.objectId = value;
            }
        }
        public string FirstRead
        {
            get
            {
                return this.firstRead;
            }
            set
            {
                this.firstRead = value;
            }
        }
        public string LastRead
        {
            get
            {
                return this.lastRead;
            }
            set
            {
                this.lastRead = value;
            }
        }
        public string ObservationUUID
        {
            get
            {
                return this.observationUUID;
            }
            set
            {
                this.observationUUID = value;
            }
        }
        public string Reads
        {
            get
            {
                return this.reads;
            }
            set
            {
                this.reads = value;
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
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("EntryID = " + this.entryId.ToString() + Environment.NewLine);
            

            return sb.ToString();
        }
    }

    public class CrossTalkPacketCollection : CollectionBase
    {
        public CrossTalkPacketCollection()
        {
        }
        public CrossTalkPacketCBE this[int index]
        {
            get { return (CrossTalkPacketCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CrossTalkPacketCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CrossTalkPacketCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CrossTalkPacketCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CrossTalkPacketCBE value)
        {
            List.Remove(value);
        }
    }
}
