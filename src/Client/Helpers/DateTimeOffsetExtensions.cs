namespace EmployeeRecordSystem.Client.Helpers;

public static class DateTimeOffsetExtensions
{
	public static string Humanize(this DateTimeOffset date)
	{
		return date.ToLocalTime()
			.ToString("dd.MM.yyyy HH:mm");
	}
}
