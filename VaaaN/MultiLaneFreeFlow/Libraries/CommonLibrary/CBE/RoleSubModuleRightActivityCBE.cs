using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class RoleSubModuleRightActivityCBE
    {
      
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }

        [Display(Name = "Module")]
        public string ModuleName { get; set; }

        public int SubModuleId { get; set; }

        [Display(Name = "Sub Module")]
        public string SubModuleName { get; set; }

        [Display(Name = "View")]
        public bool SubModuleView { get; set; }

        [Display(Name = "Add")]
        public bool SubModuleAdd { get; set; }

        [Display(Name = "Edit")]
        public bool SubModuleEdit { get; set; }

        [Display(Name = "Delete")]
        public bool SubModuleDelete { get; set; }

      

        public RoleSubModuleRightActivityCBE()
        {
            this.Id = 0;
            this.RoleId = 0;
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.SubModuleId = 0;
            this.SubModuleName = string.Empty;
            this.SubModuleView = false;
            this.SubModuleAdd = false;
            this.SubModuleEdit = false;
            this.SubModuleDelete = false;
        }
    }

    public class RoleSubModuleRightActivityCollection : CollectionBase
    {
        public RoleSubModuleRightActivityCollection()
        {
        }
        public RoleSubModuleRightActivityCBE this[int index]
        {
            get { return (RoleSubModuleRightActivityCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(RoleSubModuleRightActivityCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(RoleSubModuleRightActivityCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, RoleSubModuleRightActivityCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(RoleSubModuleRightActivityCBE value)
        {
            List.Remove(value);
        }
    }
}
