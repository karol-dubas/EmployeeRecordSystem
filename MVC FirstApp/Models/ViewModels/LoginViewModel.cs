using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Pole 'Nazwa użytkownika' jest wymagane")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Pole 'Hasło' jest wymagane")]
        public string Password { get; set; }
    }
}
