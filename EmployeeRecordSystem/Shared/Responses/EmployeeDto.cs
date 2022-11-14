using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Shared.Responses;

public class EmployeeInGroupDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public string HourlyPay { get; set; }
    public string TimeWorked { get; set; }
    public string Note { get; set; }
    public GroupDto Group { get; set; }
}