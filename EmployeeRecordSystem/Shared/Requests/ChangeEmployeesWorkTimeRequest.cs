using System;

namespace EmployeeRecordSystem.Shared.Requests;

public class ChangeEmployeesWorkTimeRequest
{
    public List<Guid> EmployeeIds { get; init; }
    public string WorkTimeOperation { get; init; }
    public TimeSpan WorkTime { get; init; }
}