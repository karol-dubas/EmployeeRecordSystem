using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.ViewModels
{
    public class AccountHistoryViewModel
    {
        public string OperationType { get; set; }
        public string Amount { get; set; }
        public string BalanceAfter { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
