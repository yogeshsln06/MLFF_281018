using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    [Serializable]
    public class NodeFluxPacketCBE
    {
        #region Variables
        int tmsId;
        int entryId;

         
        string eventType;
        string timeStamp;
        Int32 gantryId;
        Int32 laneId;
        string camaraPosition;
        Int32 cameraId;
        string cameraName;
        string cameraAddress;
        string camaraCoordinate;
        string plateNumber;
        Int32 vehicleClassId;
        string vehicleClassName;
        Int32 vehicleSpeed;
        string plateThumbnail;
        string vehicleThumbnail;
        string videoURL;
        int provider;

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
        public int GantryId
        {
            get
            {
                return this.gantryId;
            }
            set
            {
                this.gantryId = value;
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

        [JsonProperty("event_type")]
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

        [JsonProperty("timestamp")]
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

        public Int32 LaneId
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

        public string CameraPosition
        {
            get
            {
                return this.camaraPosition;
            }
            set
            {
                this.camaraPosition = value;
            }
        }

        [JsonProperty("camera_id")]
        public Int32 CameraId
        {
            get
            {
                return this.cameraId;
            }
            set
            {
                this.cameraId = value;
            }
        }

        [JsonProperty("camera_name")]
        public string CameraName
        {
            get
            {
                return this.cameraName;
            }
            set
            {
                this.cameraName = value;
            }
        }
        public string CameraAddress
        {
            get
            {
                return this.cameraAddress;
            }
            set
            {
                this.cameraAddress = value;
            }
        }
        public string CamaraCoordinate
        {
            get
            {
                return this.camaraCoordinate;
            }
            set
            {
                this.camaraCoordinate = value;
            }
        }
      
        public string PlateNumber
        {
            get
            {
                return this.plateNumber;
            }
            set
            {
                this.plateNumber = value;
            }
        }
        public Int32 VehicleClassId
        {
            get
            {
                return this.vehicleClassId;
            }
            set
            {
                this.vehicleClassId = value;
            }
        }

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

        public Int32 VehicleSpeed
        {
            get
            {
                return this.vehicleSpeed;
            }
            set
            {
                this.vehicleSpeed = value;
            }
        }
        public string PlateThumbnail
        {
            get
            {
                return this.plateThumbnail;
            }
            set
            {
                this.plateThumbnail = value;
            }
        }
        public string VehicleThumbnail
        {
            get
            {
                return this.vehicleThumbnail;
            }
            set
            {
                this.vehicleThumbnail = value;
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

        public int Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                this.provider = value;
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

    public class NodeFluxPacketCollection : CollectionBase
    {
        public NodeFluxPacketCollection()
        {
        }
        public NodeFluxPacketCBE this[int index]
        {
            get { return (NodeFluxPacketCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(NodeFluxPacketCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(NodeFluxPacketCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, NodeFluxPacketCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(NodeFluxPacketCBE value)
        {
            List.Remove(value);
        }
    }
    public class NodeFluxPacketJSON
    {
        public string Event_Type { get; set; }
        public string TimeStamp { get; set; }
        public Int32 Gantry_Id { get; set; }
       
        public CameraFields Camera { get; set; }
        public DataFields Data { get; set; }

    }
    public class CameraFields
    {

        // public string Id { get; set; }
        // public string Name { get; set; }
        // public string Address { get; set; }
        //public string [] Coordinate { get; set; }

       
       
        public Int32 Camera_Position { get; set; }
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string[] Coordinate { get; set; }
        public Int32 Lane_Id { get; set; }
    }

    public class DataFields
    {
        //public string Plate { get; set; }
        //public string Vehicle_Type { get; set; }
        //public string Vehicle_Thumbnail { get; set; }
        //public string Thumbnail { get; set; }

        public string Plate { get; set; }

        public string Vehicle_Type { get; set; }

        public Int32 Vehicle_Speed { get; set; }

        public string Thumbnail { get; set; }

        public string Vehicle_Thumbnail { get; set; }

        public string Video_URL { get; set; }
    }

    public class OpenALPRPacketJSON
    {
        public string epoch_start { get; set; }
        public string epoch_end { get; set; }
        public Int32 camera_id { get; set; }
        public string agent_uid { get; set; }
        public string company_id { get; set; }
        public string agent_type { get; set; }
        public string best_uuid { get; set; }
        public string best_plate_number { get; set; }

        public vehicleDetails vehicle { get; set; }
        public web_server_configDetails web_server_config { get; set; }
        


    }
    
    
    public class vehicleDetails
    {
        public List<bodytype> body_type { get; set; }
    }

    public class bodytype
    {
        public decimal confidence { get; set; }
        public string name { get; set; }
    }

    public class web_server_configDetails
    {
        public string agent_label { get; set; }
        public string camera_label { get; set; }
    }
}
