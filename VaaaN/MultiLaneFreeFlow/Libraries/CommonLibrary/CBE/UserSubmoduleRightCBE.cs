using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserSubmoduleRightCBE
    {
        private Int32 userId;
     
        private Int32 subModuleActivityEntryId;
        private int transferStatus;

        public UserSubmoduleRightCBE()
        {
            this.userId = 0;
       
            this.subModuleActivityEntryId = 0;
            this.transferStatus = 1;
        }

        public Int32 UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

     
        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public Int32 SubModuleActivityEntryId
        {
            get { return this.subModuleActivityEntryId; }
            set { this.subModuleActivityEntryId = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UserID = " + this.userId.ToString() + Environment.NewLine);
            sb.Append("subModuleActivityEntryId = " + this.subModuleActivityEntryId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class UserSubmoduleRightCollection : CollectionBase
    {
        public UserSubmoduleRightCollection()
        {
        }
        public UserSubmoduleRightCBE this[int index]
        {
            get { return (UserSubmoduleRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserSubmoduleRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(UserSubmoduleRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, UserSubmoduleRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserSubmoduleRightCBE value)
        {
            List.Remove(value);
        }
    }
}
