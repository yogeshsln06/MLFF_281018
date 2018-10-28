using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class UserSubModuleActivityRightCBE
    {
        [Display]
        public int Id { get; set; }
        [Display]
        public int UserId { get; set; }

        [Display(Name = "Module Id")]
        public int ModuleId { get; set; }

        [Display(Name = "Module")]
        public String ModuleName { get; set; }

        [Display]
        public int SubModuleId { get; set; }

        [Display(Name = "Sub Module")]
        public String SubModuleName { get; set; }

        [Display(Name = "View")]
        public bool SubModuleView { get; set; }

        [Display(Name = "Add")]
        public bool SubModuleAdd { get; set; }

        [Display(Name = "Edit")]
        public bool SubModuleEdit { get; set; }

        [Display(Name = "Delete")]
        public bool SubModuleDelete { get; set; }

        public UserSubModuleActivityRightCBE()
        {
            this.Id = 0;
            this.UserId = 0;
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.SubModuleId = 0;
            this.SubModuleName = String.Empty;
            this.SubModuleView = false;
            this.SubModuleAdd = false;
            this.SubModuleEdit = false;
            this.SubModuleDelete = false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id =" + this.Id + Environment.NewLine);
            sb.Append("UserId = " + this.UserId + Environment.NewLine);
            sb.Append("Sub Module Id =" + this.SubModuleId + Environment.NewLine);
            sb.Append("Sub Module View =" + (this.SubModuleView ? "Yes" : "No") + Environment.NewLine);
            sb.Append("Sub Module Add =" + (this.SubModuleAdd ? "Yes" : "No") + Environment.NewLine);
            sb.Append("Sub Module Edit =" + (this.SubModuleEdit ? "Yes" : "No") + Environment.NewLine);
            sb.Append("Sub Module Delete =" + (this.SubModuleDelete ? "Yes" : "No") + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class UserSubModuleActivityRightCollection : CollectionBase
    {
        public UserSubModuleActivityRightCollection()
        {
        }
        public UserSubModuleActivityRightCBE this[int index]
        {
            get { return (UserSubModuleActivityRightCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(UserSubModuleActivityRightCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(UserSubModuleActivityRightCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, UserSubModuleActivityRightCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(UserSubModuleActivityRightCBE value)
        {
            List.Remove(value);
        }
    }
}
