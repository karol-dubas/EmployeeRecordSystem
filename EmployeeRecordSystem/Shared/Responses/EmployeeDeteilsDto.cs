namespace EmployeeRecordSystem.Shared.Responses;

public class EmployeeDeteilsDto
{
    public string Role { get; set; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string BankAccountNumber { get; init; }
    public GroupDto Group { get; init; }
    public UserBillingDto UserBilling { get; init; }

}
