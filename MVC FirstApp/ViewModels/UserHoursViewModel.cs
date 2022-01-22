using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class UserHoursViewModel
    {
        public List<HoursEditUserListViewModel> Users { get; set; }

        [Display(Name = "Godziny")]
        [Range(0, int.MaxValue, ErrorMessage = "Wartość godzin musi być dodatnia")]
        public int HoursToEdit { get; set; }

        [Display(Name = "Minuty")]
        [Range(0, int.MaxValue, ErrorMessage = "Wartość minut musi być dodatnia")]
        public int MinutesToEdit { get; set; }

        public bool AddHours { get; set; }
    }
}
