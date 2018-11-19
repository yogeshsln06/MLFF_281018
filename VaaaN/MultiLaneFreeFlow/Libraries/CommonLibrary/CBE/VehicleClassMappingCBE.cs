using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class VehicleClassMappingCBE
    {
        private Int32 tmsId;
        private Int32 mappingvehicleClassId;
        private Int32 mlffvehicleClassId;
        private string mlffvehicleClassName;
        private Int32 anprvehicleClassId;
        private string anprvehicleClassName;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifierId;
        private Int32 modifiedBy;
        private Int32 transferStatus;

        public VehicleClassMappingCBE()
        {
            this.tmsId = 0;
            this.mappingvehicleClassId = 0;
            this.mlffvehicleClassId = 0;
            this.mlffvehicleClassName = string.Empty;
            this.anprvehicleClassId = 0;
            this.anprvehicleClassName = string.Empty;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifierId = 0;
            this.modifiedBy = 0;
            this.transferStatus = 0;
        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

     
        
        [Display(Name = "MappingVehicleClassId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 MappingVehicleClassId
        {
            get { return this.mappingvehicleClassId; }
            set { this.mappingvehicleClassId = value; }
        }

        [Display(Name = "MLFFVehicleClassId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 MLFFVehicleClassId
        {
            get { return this.mlffvehicleClassId; }
            set { this.mlffvehicleClassId = value; }
        }

        [Display(Name = "ANPRVehicleClassId", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 ANPRVehicleClassId
        {
            get { return this.anprvehicleClassId; }
            set { this.anprvehicleClassId = value; }
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


        public Int32 ModifiedBy
        {
            get { return this.modifiedBy; }
            set { this.modifiedBy = value; }
        }
        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }
        [Display(Name = "MLFF Vehicle Class Name")]
        public string MLFFVehicleClassName
        {
            get
            {
                return this.mlffvehicleClassName;
            }

            set
            {
                this.mlffvehicleClassName = value;
            }
        }

        [Display(Name = "ANPR Vehicle Class Name")]
        public string ANPRVehicleClassName
        {
            get
            {
                return this.anprvehicleClassName;
            }

            set
            {
                this.anprvehicleClassName = value;
            }
        }
        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("mappingvehicleClassId = " + this.mappingvehicleClassId + Environment.NewLine);
            sb.Append("mlffvehicleClassId = " + this.mlffvehicleClassId + Environment.NewLine);
            sb.Append("mlffvehicleClassName = " + this.mlffvehicleClassName + Environment.NewLine);
            sb.Append("anprvehicleClassId = " + this.anprvehicleClassId + Environment.NewLine);
            sb.Append("anprvehicleClassName = " + this.anprvehicleClassName + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy = " + this.modifierId + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class VehicleClassMappingCollection : CollectionBase
    {
        public VehicleClassMappingCollection()
        {
        }
        public VehicleClassMappingCBE this[int index]
        {
            get { return (VehicleClassMappingCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(VehicleClassMappingCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(VehicleClassMappingCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, VehicleClassMappingCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(VehicleClassMappingCBE value)
        {
            List.Remove(value);
        }
    }
}
