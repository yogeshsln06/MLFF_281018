using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class ModuleCBE
    {
        private Int32 moduleId;

        private String moduleName;
        private Int32 isGuiVisible;
        private int transferStatus;
        private string moduleurl;
        private string icon;

        public ModuleCBE()
        {
            this.moduleId = 0;

            this.moduleName = String.Empty;
            this.isGuiVisible = 0;
            this.transferStatus = 1;
            this.moduleurl = string.Empty;
            this.icon = string.Empty;
        }
        public Int32 ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }


        public String ModuleName
        {
            get { return this.moduleName; }
            set { this.moduleName = value; }
        }

        public Int32 IsGuiVisible
        {
            get { return this.isGuiVisible; }
            set { this.isGuiVisible = value; }
        }
        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public string ModuleUrl
        {
            get { return this.moduleurl; }
            set { this.moduleurl = value; }
        }

        public string Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ModuleID" + this.moduleId.ToString() + Environment.NewLine);
            sb.Append("ModuleName" + this.moduleName.ToString() + Environment.NewLine);
            sb.Append("IsGuiVisible" + this.isGuiVisible.ToString() + Environment.NewLine);
            sb.Append("icon" + this.icon.ToString() + Environment.NewLine);

            return sb.ToString();
        }

    }
    public class ModuleCollection : CollectionBase
    {
        public ModuleCollection()
        {
        }
        public ModuleCBE this[int index]
        {
            get { return (ModuleCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(ModuleCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(ModuleCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, ModuleCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(ModuleCBE value)
        {
            List.Remove(value);
        }

    }
}
