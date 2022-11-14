namespace EmployeeRecordSystem.Shared.Requests;

public class EditEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BankAccountNumber { get; set; }
    public string Note { get; set; }
}
