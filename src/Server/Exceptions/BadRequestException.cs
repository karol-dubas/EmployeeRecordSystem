namespace EmployeeRecordSystem.Server.Exceptions;

public sealed class BadRequestException : Exception, IHttpException
{
	public BadRequestException(string fieldName, string message) : base(message)
	{
		FieldName = fieldName;
	}

	public string FieldName { get; }
}
