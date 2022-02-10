using EmployeeRecordSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PositionName { get; set; }
        public string TimeWorked { get; set; }
        public decimal HourlyPay { get; set; }
    }
}
