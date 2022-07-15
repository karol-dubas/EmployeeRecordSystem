using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses
{
    public class EmployeeBillingDto
    {
        public decimal HourlyPay { get; set; }
        public TimeSpan TimeWorked { get; set; }
        public decimal Balance { get; set; }
    }
}
