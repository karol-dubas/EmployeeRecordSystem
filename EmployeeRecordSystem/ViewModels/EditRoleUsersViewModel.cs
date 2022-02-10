using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.ViewModels
{
    public class EditRoleUsersViewModel
    {
        public EditRoleUsersViewModel()
        {
            Users = new List<string>();
        }

        [Display(Name = "Nazwa roli")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
