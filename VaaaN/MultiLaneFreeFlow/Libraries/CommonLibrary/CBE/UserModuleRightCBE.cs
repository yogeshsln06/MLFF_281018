using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserModuleRightCBE
    {
        private Int32 userId;
  
        private Int32 moduleActivityEntryId;
        private int transferStatus;

        public UserModuleRightCBE()
        {
            this.userId = 0;
         
            this.moduleActivityEntryId = 0;
            this.transferStatus = 1;
        }

     

        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public Int32 ModuleActivityEntryId
        {
            get { return this.moduleActivityEntryId; }
            set { this.moduleActivityEntryId = value; }
        }

        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UserID = " + this.userId.ToString() + Environment.NewLine);
          
            sb.Append("moduleActivityEntryId = " + this.moduleActivityEntryId.ToString() + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class UserModuleRightCollection : CollectionBase
    {
        public UserModuleRightCollection()
        {
        }
        public UserModuleRightCBE this[int index]
        {
            get { return (UserModuleRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserModuleRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(UserModuleRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, UserModuleRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserModuleRightCBE value)
        {
            List.Remove(value);
        }
    }
}
