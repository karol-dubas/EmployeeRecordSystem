namespace EmployeeRecordSystem.Data.Entities;

public class Announcement
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
	
	public Guid? CreatedById { get; set; }
	public virtual Employee CreatedBy { get; set; }
}
