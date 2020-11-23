using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models
{
    public class AccountHistoryEntity
    {
        public int Id { get; set; }
        public string ActionType { get; set; }
        public double Amount { get; set; }
        public double BalanceAfter { get; set; }
        public DateTime Date { get; set; }
    }
}
