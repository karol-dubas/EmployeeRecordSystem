using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models.ViewModels
{
    public class AccountHistoryViewModel
    {
        public string ActionType { get; set; }
        public string Amount { get; set; }
        public string BalanceAfter { get; set; }
        public DateTime Date { get; set; }
    }
}
