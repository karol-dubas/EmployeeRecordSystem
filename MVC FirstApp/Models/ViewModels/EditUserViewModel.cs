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

        [Display(Name = "Stawka na godzinę [zł/h]")]
        [Required(ErrorMessage = "Pole 'Stawka na godzinę' jest wymagane")]
        [Range(0, double.MaxValue, ErrorMessage = "Wartość musi być dodatnia")]
        public double HourlyPay { get; set; }

        [Display(Name = "Czas przepracowany")]
        public string HoursMinutesWorked { get; set; }

        [Display(Name = "Saldo [zł]")]
        public decimal Balance { get; set; }

        [Display(Name = "Uprawnienia")]
        public string Roles { get; set; }
    }
}
