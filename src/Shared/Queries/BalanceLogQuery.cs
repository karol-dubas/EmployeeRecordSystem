namespace EmployeeRecordSystem.Shared.Queries;

public class BalanceLogQuery
{
	public string SortBy { get; set; }
	public string SortDirection { get; set; }
	public int PageSize { get; set; }
	public int PageNumber { get; set; }
}
