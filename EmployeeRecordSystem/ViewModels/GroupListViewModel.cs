using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.ViewModels
{
    public class GroupListViewModel
    {
        public GroupViewModel GroupDetails { get; set; }
        public IEnumerable<UserViewModel> UsersInGroup { get; set; }
    }
}
