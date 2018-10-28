using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    [Serializable]
    public class NodeFluxEvent
    {
        DateTime timestamp;
        Int32 plazaId;
        string plazaName;
        string laneName;
        string vehicleClassName;
        string vrn;
        string cameraLocation;
        string numberPlatePath;
        string vehiclePath;
        string videoURL;

        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                this.timestamp = value;

            }
        }

        public Int32 PlazaId
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
        public string LaneName
        {
            get
            {
                return this.laneName;
            }
            set
            {
                this.laneName = value;
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
        public string VRN
        {
            get
            {
                return this.vrn;
            }
            set
            {
                this.vrn = value;
            }
        }
        public string CameraLocation
        {
            get
            {
                return this.cameraLocation;
            }
            set
            {
                this.cameraLocation = value;
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
    }
}
