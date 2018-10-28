using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserLoginDetailCBE
    {
   
        private Int32 userId;
        private DateTime loginTime;
        private DateTime logoutTime;
        private Int32 moduleId;
        private Int32 submoduleId;
        private Int32 transferStatus;

        public UserLoginDetailCBE()
        {
           
            this.userId = 0;
            this.loginTime = DateTime.MaxValue;
            this.logoutTime = DateTime.MaxValue;
            this.moduleId = 0;
            this.submoduleId = 0;
            this.transferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
        }

   
        public Int32 UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }
        public DateTime LoginTime
        {
            get { return this.loginTime; }
            set { this.loginTime = value; }
        }
        public DateTime LogoutTime
        {
            get { return this.logoutTime; }
            set { this.logoutTime = value; }
        }
        public Int32 ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }
        public Int32 SubmoduleId
        {
            get { return this.submoduleId; }
            set { this.submoduleId = value; }
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
            sb.Append("LoginTime = " + this.loginTime.ToString() + Environment.NewLine);
            sb.Append("LogOutTime = " + this.logoutTime.ToString() + Environment.NewLine);
            sb.Append("ModuleId = " + this.moduleId.ToString() + Environment.NewLine);
            sb.Append("SubModuleId = " + this.moduleId.ToString() + Environment.NewLine);
            sb.Append("TransferStatus = " + this.transferStatus.ToString() + Environment.NewLine);
            return sb.ToString();
        }
    }
    public class UserLoginDetailCollection : CollectionBase
    {
        public UserLoginDetailCollection()
        {
        }
        public UserLoginDetailCBE this[int index]
        {
            get { return (UserLoginDetailCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserLoginDetailCBE value)
        {
            return (List.Add(value));

        }
        public int IndexOf(UserLoginDetailCBE value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, UserLoginDetailCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserLoginDetailCBE value)
        {
            List.Remove(value);
        }
    }
}
