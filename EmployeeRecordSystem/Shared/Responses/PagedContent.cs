namespace EmployeeRecordSystem.Shared.Responses;

public class PagedContent<TContent>
{
	public List<TContent> Items { get; set; }
	public int TotalItemsCount { get; set; }

	private PagedContent() { } // Serialization use only
	public PagedContent(List<TContent> items, int totalItemsCount)
	{
		Items = items;
		TotalItemsCount = totalItemsCount;
	}
}
