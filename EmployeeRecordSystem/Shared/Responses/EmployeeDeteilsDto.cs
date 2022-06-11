namespace EmployeeRecordSystem.Shared.Responses;

public class EmployeeDeteilsDto
{
    public string Role { get; set; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string BankAccountNumber { get; init; }
    public string GroupName { get; init; }
    public string UserBillingHourlyPay { get; init; }
    public TimeSpan UserBillingTimeWorked { get; init; }
    public string UserBillingBalance { get; init; }
}
