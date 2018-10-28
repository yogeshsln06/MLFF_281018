using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class LaneCBE
    {
        private Int32 tmsId;
        private Int32 plazaId;
        private String plazaName;
        private Int32 laneId;
        private Int32 laneTypeId;
        private String laneName;
        private Int32 cameraIdFront;
        private string cameraNameFront;
        private Int32 cameraIdRear;
        private string cameraNameRear;
        private Int32 etcReaderId;
        private string etcReaderName;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifierId;
        private Int32 transferStatus;
       
        public LaneCBE()
        {
            this.tmsId = 0;
            this.plazaId = 0;
            this.laneId = 0;
            this.laneName = String.Empty;
            this.cameraIdFront = 0;
            this.cameraNameFront = string.Empty;
            this.cameraIdRear = 0;
            this.cameraNameRear = string.Empty;
            this.etcReaderId = 0;
            this.etcReaderName = string.Empty;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifierId = 0;
            this.transferStatus = 0;
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

        [Display(Name = "GantryName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String PlazaName
        {
            get { return this.plazaName; }
            set { this.plazaName = value; }
        }
        public Int32 LaneId
        {
            get { return this.laneId; }
            set { this.laneId = value; }
        }

        public Int32 LaneTypeId
        {
            get { return this.laneTypeId; }
            set { this.laneTypeId = value; }
        }

        [Display(Name ="LaneName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String LaneName
        {
            get { return this.laneName; }
            set { this.laneName = value; }
        }

        public Int32 CameraIdFront
        {
            get { return this.cameraIdFront; }
            set { this.cameraIdFront = value; }
        }
        [Display(Name = "CameraNameFront", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string CameraNameFront
        {
            get
            {
                return this.cameraNameFront;
            }

            set
            {
                this.cameraNameFront = value;
            }
        }

        public Int32 CameraIdRear
        {
            get { return this.cameraIdRear; }
            set { this.cameraIdRear = value; }
        }
        [Display(Name = "CameraNameRear", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string CameraNameRear
        {
            get
            {
                return this.cameraNameRear;
            }

            set
            {
                this.cameraNameRear = value;
            }
        }

        public Int32 EtcReaderId
        {
            get { return this.etcReaderId; }
            set { this.etcReaderId = value; }
        }
        [Display(Name = "ETCReaderName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string EtcReaderName
        {
            get
            {
                return this.etcReaderName;
            }

            set
            {
                this.etcReaderName = value;
            }
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

        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

    

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("plazaId = " + this.plazaId + Environment.NewLine);
            sb.Append("laneId = " + this.laneId + Environment.NewLine);
            sb.Append("laneTypeId = " + this.laneTypeId + Environment.NewLine);
            sb.Append("laneName = " + this.laneName + Environment.NewLine);
            sb.Append("cameraIdFront = " + this.cameraIdFront + Environment.NewLine);
            sb.Append("cameraIdRear = " + this.cameraIdRear + Environment.NewLine);
            sb.Append("etcReaderId = " + this.etcReaderId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifierId + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);
            
            return sb.ToString();
        }
    }

    public class LaneCollection : CollectionBase
    {
        public LaneCollection()
        {
        }
        public LaneCBE this[int index]
        {
            get { return (LaneCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(LaneCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(LaneCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, LaneCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(LaneCBE value)
        {
            List.Remove(value);
        }
    }
}
