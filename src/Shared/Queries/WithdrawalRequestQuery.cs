using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Queries;

public class WithdrawalRequestQuery
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string WithdrawalRequestStatus { get; set; }
    public string NameSearch { get; set; }
    public string SortBy { get; set; }
    public string SortDirection { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}