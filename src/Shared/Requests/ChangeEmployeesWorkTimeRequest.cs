using System;

namespace EmployeeRecordSystem.Shared.Requests;

public class ChangeEmployeesWorkTimeRequest
{
    public List<Guid> EmployeeIds { get; set; } = new();
    public string WorkTimeOperation { get; set; }
    public TimeSpan WorkTime { get; set; }
}