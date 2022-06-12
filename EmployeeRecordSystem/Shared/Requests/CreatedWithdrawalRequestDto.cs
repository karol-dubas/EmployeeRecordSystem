using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Requests
{
    public class CreatedWithdrawalRequestDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public decimal Amount { get; set; }
        public string WithdrawalRequestStatus { get; set; }
    }
}
