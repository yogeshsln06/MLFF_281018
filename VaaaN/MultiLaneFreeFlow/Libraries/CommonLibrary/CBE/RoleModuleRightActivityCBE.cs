using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class RoleModuleRightActivityCBE
    {
     
        public int Id { get; set; }
        public int RoleId { get; set; }
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

        public RoleModuleRightActivityCBE()
        {
            this.Id = 0;
            this.RoleId = 0;
            this.ModuleId = 0;
            this.ModuleName = string.Empty;
            this.ModuleView = false;
            this.ModuleAdd = false;
            this.ModuleEdit = false;
            this.ModuleDelete = false;
      
        }
    }

    public class RoleModuleRightActivityCollection : CollectionBase
    {
        public RoleModuleRightActivityCollection()
        {
        }
        public RoleModuleRightActivityCBE this[int index]
        {
            get { return (RoleModuleRightActivityCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(RoleModuleRightActivityCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(RoleModuleRightActivityCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, RoleModuleRightActivityCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(RoleModuleRightActivityCBE value)
        {
            List.Remove(value);
        }
    }
}
