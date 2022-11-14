using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities;

public class BalanceLog
{
    public Guid Id { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }
}