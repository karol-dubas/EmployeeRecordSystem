using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class GroupListViewModel
    {
        public IEnumerable<GroupItemListViewModel> Accounts { get; set; }
    }
}
