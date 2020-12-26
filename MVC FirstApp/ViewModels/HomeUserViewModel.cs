using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class HomeUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GroupEnum Group { get; set; }
        public PositionEnum Position { get; set; }
        public string HoursMinutesWorked { get; set; }
        public string Balance { get; set; }
    }
}
