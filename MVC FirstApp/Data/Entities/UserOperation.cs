using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data.Entities
{
    public class UserOperation
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
