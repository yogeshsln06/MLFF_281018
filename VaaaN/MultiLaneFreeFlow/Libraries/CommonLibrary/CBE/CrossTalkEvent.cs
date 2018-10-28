using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    [Serializable]
    public class CrossTalkEvent
    {
        DateTime timestamp;
        Int32 plazaId;
        string plazaName;
        string laneName;
        string vehicleClassName;
        string vrn;

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

    }
}
