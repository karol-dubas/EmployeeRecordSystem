using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models
{
    public class BillingEntity
    {
        public int Id { get; set; }
        public double HourlyPay { get; set; }
        public long MinutesWorked { get; set; }
        public decimal Balance { get; set; }
    }
}
