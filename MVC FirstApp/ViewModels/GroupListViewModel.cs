using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class GroupListViewModel
    {
        public IEnumerable<GroupItemListViewModel> Accounts { get; set; }
    }
}
