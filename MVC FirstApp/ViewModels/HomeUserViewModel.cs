using MVC_FirstApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class HomeUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
        public string Position { get; set; }
        public string TimeWorked { get; set; }
        public string Balance { get; set; }
    }
}
