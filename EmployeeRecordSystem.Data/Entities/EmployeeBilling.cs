using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities;

public class EmployeeBilling
{
    public Guid Id { get; set; }
    public decimal HourlyPay { get; set; }
    public TimeSpan TimeWorked { get; set; }
    public decimal Balance { get; set; }

    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }
}