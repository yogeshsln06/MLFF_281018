using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class ModuleActivityCBE
    {
      
        private Int32 entryId;
        private Int32 moduleId;
        private Int32 activityId;
        private int transferStatus;

        public ModuleActivityCBE()
        {
        
            this.entryId = 0;
            this.moduleId = 0;
            this.activityId = 0;
            this.transferStatus = 1;
        }

     

        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }

        public Int32 ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }

        public Int32 ActivityId
        {
            get { return this.activityId; }
            set { this.activityId = value; }
        }

        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("EntryID = " + this.entryId.ToString() + Environment.NewLine);
            sb.Append("Module ID = " + this.moduleId.ToString() + Environment.NewLine);
            sb.Append("Activity ID = " + this.activityId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class ModuleActivityCollection : CollectionBase
    {
        public ModuleActivityCollection()
        {
        }
        public ModuleActivityCBE this[int index]
        {
            get { return (ModuleActivityCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(ModuleActivityCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(ModuleActivityCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, ModuleActivityCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(ModuleActivityCBE value)
        {
            List.Remove(value);
        }
    }
}
