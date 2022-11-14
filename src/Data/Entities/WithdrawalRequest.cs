using EmployeeRecordSystem.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities;

public class WithdrawalRequest
{
    public Guid Id { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public decimal Amount { get; set; }

    public string WithdrawalRequestStatusTypeCode { get; set; }
    public WithdrawalRequestStatusType WithdrawalRequestStatusType { get; set; }

    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }

    public bool IsAlreadyProcessed()
    {
        return WithdrawalRequestStatusTypeCode != WithdrawalRequestStatusTypeCodes.Pending;
    }
}