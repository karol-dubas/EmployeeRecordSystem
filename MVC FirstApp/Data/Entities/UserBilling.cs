using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data.Entities
{
    public class UserBilling
    {
        public int Id { get; set; }
        public decimal HourlyPay { get; set; }
        public long MinutesWorked { get; set; }
        public decimal Balance { get; set; }
    }
}
