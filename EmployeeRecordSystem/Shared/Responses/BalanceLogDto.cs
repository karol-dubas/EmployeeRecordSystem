namespace EmployeeRecordSystem.Shared.Responses;

public class BalanceLogDto
{
    public decimal BalanceBefore { get; init; }
    public decimal BalanceAfter { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
