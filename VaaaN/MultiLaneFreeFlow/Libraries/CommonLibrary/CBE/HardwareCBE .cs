using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class HardwareCBE
    {
        private Int32 tmsId;
        private Int32 plazaId;
        private Int32 hardwareId;
        private String hardwareName;
        private Int32 hardwareType;
        private string hardwareTypeName;
        private Int32 hardwarePosition; //1 for front, 2 for rear only for camera
        private String manufacturerName;
        private String modelName;
        private String ipAddress;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifierId;
        private Int32 transferStatus;

        public HardwareCBE()
        {
            this.tmsId = 0;
            this.plazaId = 0;
            this.hardwareId = 0;
            this.hardwareName = String.Empty;
            this.hardwareType = 0;
            this.HardwareTypeName = string.Empty;
            this.hardwarePosition = 0;
            this.manufacturerName = String.Empty;
            this.modelName = String.Empty;
            this.ipAddress = String.Empty;
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

        public Int32 HardwareId
        {
            get { return this.hardwareId; }
            set { this.hardwareId = value; }
        }

        [Display(Name="HardwareName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String HardwareName
        {
            get { return this.hardwareName; }
            set { this.hardwareName = value; }
        }

        [Display(Name = "HardwareType", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 HardwareType
        {
            get { return this.hardwareType; }
            set { this.hardwareType = value; }
        }
        [Display(Name = "HardwareType", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public string HardwareTypeName
        {
            get
            {
                return this.hardwareTypeName;
            }

            set
            {
                this.hardwareTypeName = value;
            }
        }

        public Int32 HardwarePosition
        {
            get
            {
                return this.hardwarePosition;
            }

            set
            {
                this.hardwarePosition = value;
            }
        }
        [Display(Name = "ManufacturerName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String ManufacturerName
        {
            get { return this.manufacturerName; }
            set { this.manufacturerName = value; }
        }
        [Display(Name = "ModelName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String ModelName
        {
            get { return this.modelName; }
            set { this.modelName = value; }
        }
        [Display(Name = "IPAddress", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public String IpAddress
        {
            get { return this.ipAddress; }
            set { this.ipAddress = value; }
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
            sb.Append("hardwareId = " + this.hardwareId + Environment.NewLine);
            sb.Append("hardwareName = " + this.hardwareName + Environment.NewLine);
            sb.Append("hardwareType = " + this.hardwareType + Environment.NewLine);
            sb.Append("hardwarePosition = " + this.HardwarePosition + Environment.NewLine);
            sb.Append("manufacturerName = " + this.manufacturerName + Environment.NewLine);
            sb.Append("modelName = " + this.modelName + Environment.NewLine);
            sb.Append("ipAddress = " + this.ipAddress + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifierId + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class HardwareCollection : CollectionBase, IEnumerable
    {
        public HardwareCollection()
        {
        }
        public HardwareCBE this[int index]
        {
            get { return (HardwareCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(HardwareCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(HardwareCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, HardwareCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(HardwareCBE value)
        {
            List.Remove(value);
        }
    }
}
