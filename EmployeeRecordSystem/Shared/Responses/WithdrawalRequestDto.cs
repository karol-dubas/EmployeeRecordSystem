using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses;

public class WithdrawalRequestDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Amount { get; set; }
    public string WithdrawalRequestStatus { get; set; }
    public string EmployeeFullName { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeBankAccountNumber { get; set; }
}