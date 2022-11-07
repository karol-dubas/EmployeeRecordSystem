namespace EmployeeRecordSystem.Server.Exceptions;

public sealed class BadRequestException : Exception, IHttpException
{
	public string FieldName { get; }

	public BadRequestException(string fieldName, string message) : base(message)
	{
		FieldName = fieldName;
	}
}
