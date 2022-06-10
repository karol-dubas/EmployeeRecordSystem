using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class Request
    {
        public Guid Id { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public decimal Amount { get; set; }

        public string RequestStatusTypeCode { get; set; }
        public RequestStatusType RequestStatusType { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
