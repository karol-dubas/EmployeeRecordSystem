using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class UserBilling
    {
        public Guid Id { get; set; }
        public decimal HourlyPay { get; set; }
        public TimeSpan TimeWorked { get; set; }
        public decimal Balance { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
