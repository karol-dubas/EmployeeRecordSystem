using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses
{
    public class EmployeeInGroupDto
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Role { get; set; }
    }
}
