using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Pole 'Nazwa użytkownika' jest wymagane")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Pole 'Hasło' jest wymagane")]
        [MinLength(4, ErrorMessage = "Hasło musi się składać z minimum 4 znaków")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Pole 'Powtórz hasło' jest wymagane")]
        public string ConfirmPassword { get; set; }

        public bool PasswordConfirmed()
        {
            return Password == ConfirmPassword;
        }

        [Required(ErrorMessage = "Pole 'Imię' jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane")]
        public string LastName { get; set; }
    }
}
