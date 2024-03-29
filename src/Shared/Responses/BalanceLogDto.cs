﻿namespace EmployeeRecordSystem.Shared.Responses;

public class BalanceLogDto
{
    public decimal Amount => BalanceAfter - BalanceBefore;
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string OperationType { get; set; }
}
