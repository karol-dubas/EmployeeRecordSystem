using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses
{
    public class WithdrawalRequestDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string WithdrawalRequestStatus { get; set; }
        public string UserFullName { get; set; }
        public Guid UserId { get; set; }
        public string UserBankAccountNumber { get; set; }
    }
}
