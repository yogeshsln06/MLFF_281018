using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class SubmoduleCBE
    {

        private Int32 subModuleId;
        private Int32 moduleId;

        private String submoduleName;
        private Int32 isGuiVisible;
        private int transferStatus;
        private string languageKey;
        private string submoduleUrl;
        private string icon;

        public SubmoduleCBE()
        {
            this.subModuleId = 0;
            this.moduleId = 0;

            this.submoduleName = String.Empty;
            this.isGuiVisible = 0;
            this.transferStatus = 1;
            this.languageKey = string.Empty;
            this.submoduleUrl = string.Empty;
            this.icon = string.Empty;
        }

        public Int32 SubModuleId
        {
            get { return this.subModuleId; }
            set { this.subModuleId = value; }
        }


        public Int32 ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }



        public String SubModuleName
        {
            get { return this.submoduleName; }
            set { this.submoduleName = value; }
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
        public String LanguageKey
        {
            get { return this.languageKey; }
            set { this.languageKey = value; }
        }

        public string SubmoduleUrl
        {
            get { return this.submoduleUrl; }
            set { this.submoduleUrl = value; }
        }

        public string Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SubModuleId" + this.subModuleId.ToString() + Environment.NewLine);
            sb.Append("ModuleID" + this.moduleId.ToString() + Environment.NewLine);

            sb.Append("SubModuleName" + this.submoduleName.ToString() + Environment.NewLine);
            sb.Append("IsGuiVisible" + this.isGuiVisible.ToString() + Environment.NewLine);
            sb.Append("icon" + this.icon.ToString() + Environment.NewLine);

            return sb.ToString();
        }

    }
    public class SubmoduleCollection : CollectionBase
    {
        public SubmoduleCollection()
        {
        }
        public SubmoduleCBE this[int index]
        {
            get { return (SubmoduleCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(SubmoduleCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(SubmoduleCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, SubmoduleCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(SubmoduleCBE value)
        {
            List.Remove(value);
        }
    }
}
