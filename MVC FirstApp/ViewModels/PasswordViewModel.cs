using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class PasswordViewModel
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        public bool PasswordConfirmed()
        {
            return NewPassword == ConfirmNewPassword;
        }
    }
}
