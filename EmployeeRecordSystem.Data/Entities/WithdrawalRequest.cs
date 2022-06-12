using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class WithdrawalRequest
    {
        public Guid Id { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public decimal Amount { get; set; }

        public string WithdrawalRequestStatusTypeCode { get; set; }
        public WithdrawalRequestStatusType WithdrawalRequestStatusType { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
