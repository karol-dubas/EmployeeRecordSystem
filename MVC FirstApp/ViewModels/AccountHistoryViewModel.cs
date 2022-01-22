using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.ViewModels
{
    public class AccountHistoryViewModel
    {
        public string ActionType { get; set; }
        public string Amount { get; set; }
        public string BalanceAfter { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
