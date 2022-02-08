using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class GroupListViewModel
    {
        public GroupViewModel Group { get; set; }
        public IEnumerable<UserViewModel> UsersInGroup { get; set; }
    }
}
