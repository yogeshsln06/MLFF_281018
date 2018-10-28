using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class SubmoduleActivityCBE
    {
  
        private Int32 entryId;
        private Int32 subModuleId;
        private Int32 activityId;
        private int transferStatus;
      
        public SubmoduleActivityCBE()
        {
         
            this.entryId = 0;
          
            this.subModuleId = 0;
            this.activityId = 0;
            this.transferStatus = 0;
        }

      
        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }

        public Int32 SubModuleId
        {
            get { return this.subModuleId; }
            set {  this.subModuleId = value; }
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
            sb.Append("Entry ID = " + this.entryId.ToString() + Environment.NewLine);
            sb.Append("subModule ID = " + this.subModuleId.ToString() + Environment.NewLine);
            sb.Append("Activity ID = " + this.activityId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class SubmoduleActivityCollection : CollectionBase
    {
        public SubmoduleActivityCollection()
        {
        }
        public SubmoduleActivityCBE this[int index]
        {
            get { return (SubmoduleActivityCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(SubmoduleActivityCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(SubmoduleActivityCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, SubmoduleActivityCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(SubmoduleActivityCBE value)
        {
            List.Remove(value);
        }
    }
}
