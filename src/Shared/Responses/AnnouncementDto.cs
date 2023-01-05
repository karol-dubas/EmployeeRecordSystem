namespace EmployeeRecordSystem.Shared.Responses;

public class AnnouncementDto
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }
	public string CreatedBy { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
}
