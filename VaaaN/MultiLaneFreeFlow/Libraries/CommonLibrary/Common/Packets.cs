using System;
using System.Runtime.Serialization;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Common
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(CBE.CrossTalkPacketCBE))]
    public class CrossTalkPacket
    {
        private string source;
        private string destination;
        private CBE.CrossTalkPacketCBE payload;
        private DateTime timeStamp;

        [DataMember]
        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        [DataMember]
        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = value;
            }
        }

        [DataMember]
        public CBE.CrossTalkPacketCBE Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value;
            }
        }

        [DataMember]
        public DateTime TimeStamp
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
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(CBE.NodeFluxPacketCBE))]
    public class NodeFluxPacket
    {
        private string source;
        private string destination;
        private CBE.NodeFluxPacketCBE payload;
        private DateTime timeStamp;

        [DataMember]
        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        [DataMember]
        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = value;
            }
        }

        [DataMember]
        public CBE.NodeFluxPacketCBE Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value;
            }
        }

        [DataMember]
        public DateTime TimeStamp
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
    }
}
