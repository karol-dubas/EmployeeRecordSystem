using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class WithdrawalViewModel
    {
        public string CurrentBalance { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Wartość musi być dodatnia")]
        public decimal AmountToWithdraw { get; set; }
    }
}
