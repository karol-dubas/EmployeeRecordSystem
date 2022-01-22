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
        public Group Group { get; set; }
        public Position Position { get; set; }
        public string HoursMinutesWorked { get; set; }
        public string Balance { get; set; }
    }
}
