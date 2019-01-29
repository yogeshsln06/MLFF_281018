using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class LiveEventCBE : CrossTalkEvent
    {
        string deviceLocation;
        string numberPlatePath;
        string vehiclePath;
        string packetName;
        string videoURL;
        string datepacket;

        public string DeviceLocation
        {
            get
            {
                return this.deviceLocation;
            }
            set
            {
                this.deviceLocation = value;
            }
        }
        public string NumberPlatePath
        {
            get
            {
                return this.numberPlatePath;
            }
            set
            {
                this.numberPlatePath = value;
            }
        }
        public string VehiclePath
        {
            get
            {
                return this.vehiclePath;
            }
            set
            {
                this.vehiclePath = value;
            }
        }
        public string PacketName
        {
            get
            {
                return this.packetName;
            }
            set
            {
                this.packetName = value;
            }
        }
        public string VideoURL
        {
            get
            {
                return this.videoURL;
            }
            set
            {
                this.videoURL = value;
            }
        }

        public string Datepacket
        {
            get
            {
                return this.datepacket;
            }
            set
            {
                this.datepacket = value;
            }
        }
    }

    public class LiveEventCollection : CollectionBase
    {
        public LiveEventCollection()
        {
        }
        public LiveEventCBE this[int index]
        {
            get { return (LiveEventCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(LiveEventCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(LiveEventCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, LiveEventCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(LiveEventCBE value)
        {
            List.Remove(value);
        }
    }
}
