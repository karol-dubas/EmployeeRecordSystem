namespace EmployeeRecordSystem.Server.Exceptions;

public sealed class ForbidException : Exception, IHttpException
{
	public ForbidException() : base("User not authorized.") { }

	public ForbidException(string details) : base($"User not authorized. {details}") { }

	public string FieldName => "User";
}
