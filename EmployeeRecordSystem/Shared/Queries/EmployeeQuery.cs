namespace EmployeeRecordSystem.Shared.Queries;

public class EmployeeQuery
{
    public Guid? GroupId { get; init; }
    public bool WithoutGroup { get; init; }
}
