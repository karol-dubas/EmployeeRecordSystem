namespace EmployeeRecordSystem.Shared.Responses;

public class EmployeeDeteilsDto
{
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BankAccountNumber { get; set; }
    public GroupDto Group { get; set; }
    public UserBillingDto UserBilling { get; set; }

}
