namespace EmployeeRecordSystem.Shared.Responses;

public class EmployeeDetailsDto
{
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BankAccountNumber { get; set; }
    public string Note { get; set; }
    public GroupDto Group { get; set; }
    public EmployeeBillingDto EmployeeBilling { get; set; }

}
