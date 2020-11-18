using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class HoursEditUserListViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool IsSelected { get; set; }
    }
}
