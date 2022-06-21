using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Queries
{
    public class WithdrawalRequestQuery
    {
        public Guid? Id { get; init; }
        public Guid? EmployeeId { get; init; }
        public string WithdrawalRequestStatus { get; init; }
    }
}
