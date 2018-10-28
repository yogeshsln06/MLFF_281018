using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserModuleActivityRightCBE
    {
        [Display]
        public int Id { get; set; }
        [Display]
        public int UserId { get; set; }
        [Display]
        public int ModuleId { get; set; }

        [Display(Name = "Module")]
        public string ModuleName { get; set; }

        [Display(Name = "View")]
        public bool ModuleView { get; set; }

        [Display(Name = "Add")]
        public bool ModuleAdd { get; set; }

        [Display(Name = "Edit")]
        public bool ModuleEdit { get; set; }

        [Display(Name = "Delete")]
        public bool ModuleDelete { get; set; }

        public UserModuleActivityRightCBE()
        {
            this.Id = 0;
            this.UserId = 0;
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.ModuleView = false;
            this.ModuleAdd = false;
            this.ModuleEdit = false;
            this.ModuleDelete = false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id =" + this.Id + Environment.NewLine);
            sb.Append("UserId = " + this.UserId + Environment.NewLine);
            sb.Append("ModuleId =" + this.ModuleId + Environment.NewLine);
            sb.Append("Module Name =" + this.ModuleName + Environment.NewLine);
            sb.Append("ModuleView =" + (this.ModuleView ? "Yes" : "No") + Environment.NewLine);
            sb.Append("ModuleAdd =" + (this.ModuleAdd ? "Yes" : "No") + Environment.NewLine);
            sb.Append("ModuleEdit =" + (this.ModuleEdit ? "Yes" : "No") + Environment.NewLine);
            sb.Append("ModuleDelete =" + (this.ModuleDelete ? "Yes" : "No") + Environment.NewLine);
            return sb.ToString();
        }
    }

    public class UserModuleActivityRightCollection : CollectionBase
    {
        public UserModuleActivityRightCollection()
        {
        }
        public UserModuleActivityRightCBE this[int index]
        {
            get { return (UserModuleActivityRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserModuleActivityRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(UserModuleActivityRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, UserModuleActivityRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserModuleActivityRightCBE value)
        {
            List.Remove(value);
        }
    }
}
