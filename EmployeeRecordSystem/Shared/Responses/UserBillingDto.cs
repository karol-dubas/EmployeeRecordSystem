using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses
{
    public class UserBillingDto
    {
        public string HourlyPay { get; init; }
        public TimeSpan TimeWorked { get; init; }
        public string Balance { get; init; }
    }
}
