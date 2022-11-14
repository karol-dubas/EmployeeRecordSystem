namespace EmployeeRecordSystem.Server.Exceptions;

public sealed class NotFoundException : Exception, IHttpException
{
	public NotFoundException(string fieldName, string entity)
		: base($"{entity} not found")
	{
		FieldName = fieldName;
	}

	public string FieldName { get; }
}
