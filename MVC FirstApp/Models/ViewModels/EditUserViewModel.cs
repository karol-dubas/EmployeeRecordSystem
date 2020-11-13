using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole 'Imię' jest wymagane")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane")]
        public string LastName { get; set; }

        [Display(Name = "Grupa")]
        public GroupEnum Group { get; set; }

        [Display(Name = "Stanowisko")]
        public PositionEnum Position { get; set; }

    }
}
