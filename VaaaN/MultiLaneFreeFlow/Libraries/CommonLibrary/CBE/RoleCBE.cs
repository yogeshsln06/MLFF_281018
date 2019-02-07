using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class RoleCBE
    {
        private Int32 roleId;
  
        private String roleName;
        private int modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private string description;
        private int transferStatus;
        private int isactive;

        public RoleCBE()
        {
            this.roleId = 0;
        
            this.roleName = String.Empty;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.description = String.Empty;
            this.transferStatus = 1;
            this.isactive = 1;
        }

        public Int32 RoleId
        {
            get { return this.roleId; }
            set { this.roleId = value; }
        }
        [Display(Name = "IsActive", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        public Int32 ISActive
        {
            get { return this.isactive; }
            set { this.isactive = value; }
        }

     

        [Display(Name ="RoleName", ResourceType = typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen))]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public String RoleName
        {
            get { return this.roleName; }
            set { this.roleName = value; }
        }

        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public int ModifierId
        {
            get { return this.modifierId; }
            set { this.modifierId = value; }
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
        public int TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("RoleId = " + this.roleId.ToString() + Environment.NewLine);
            sb.Append("RoleName = " + this.roleName + Environment.NewLine);
            sb.Append("Description = " + this.description + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class RoleCollection : CollectionBase
    {
        public RoleCollection()
        {
        }
        public RoleCBE this[int index]
        {
            get { return (RoleCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(RoleCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(RoleCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, RoleCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(RoleCBE value)
        {
            List.Remove(value);
        }
    }
}
