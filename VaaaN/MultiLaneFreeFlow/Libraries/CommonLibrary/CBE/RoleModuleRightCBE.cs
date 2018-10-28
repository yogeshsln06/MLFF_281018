using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class RoleModuleRightCBE
    {
        private Int32 roleId;
     
        private Int32 moduleActivityEntryId;
        private int transferStatus;

        public RoleModuleRightCBE()
        {
            this.roleId = 0;
          
         
            this.moduleActivityEntryId = 0;
            this.transferStatus = 1;
        }

        public Int32 RoleId
        {
            get { return this.roleId; }
            set { this.roleId = value; }
        }

    

        public Int32 ModuleActivityEntryId
        {
            get { return this.moduleActivityEntryId; }
            set { this.moduleActivityEntryId = value; }
        }
        public Int32 TransferStatus
        {
            get { return transferStatus; }
            set { transferStatus = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("RoleID = " + this.roleId.ToString() + Environment.NewLine);
            
            sb.Append("moduleActivityEntryId = " + this.moduleActivityEntryId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class RoleModuleRightCollection : CollectionBase
    {
        public RoleModuleRightCollection()
        {
        }
        public RoleModuleRightCBE this[int index]
        {
            get { return (RoleModuleRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(RoleModuleRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(RoleModuleRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, RoleModuleRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(RoleModuleRightCBE value)
        {
            List.Remove(value);
        }
    }
}
