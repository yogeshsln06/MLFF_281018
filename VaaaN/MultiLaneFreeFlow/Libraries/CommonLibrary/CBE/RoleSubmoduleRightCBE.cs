using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class RoleSubmoduleRightCBE
    {
        private Int32 roleId;

        private Int32 subModuleActivityEntryId;
        private int transferStatus;
       
 
        public RoleSubmoduleRightCBE()
        {
            this.roleId = 0;
           
            this.subModuleActivityEntryId = 0;
            this.transferStatus = 1;
       
        }

        public Int32 RoleId
        {
            get { return this.roleId; }
            set { this.roleId = value; }
        }

      

        public Int32 SubModuleActivityEntryId
        {
            get { return this.subModuleActivityEntryId; }
            set { this.subModuleActivityEntryId = value; }
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
          
            sb.Append("subModuleActivityEntryId = " + this.subModuleActivityEntryId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class RoleSubmoduleRightCollection : CollectionBase
    {
        public RoleSubmoduleRightCollection()
        {
        }
        public RoleSubmoduleRightCBE this[int index]
        {
            get { return (RoleSubmoduleRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(RoleSubmoduleRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(RoleSubmoduleRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, RoleSubmoduleRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(RoleSubmoduleRightCBE value)
        {
            List.Remove(value);
        }
    }
}
