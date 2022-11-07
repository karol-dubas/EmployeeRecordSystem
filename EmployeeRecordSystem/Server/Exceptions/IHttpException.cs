namespace EmployeeRecordSystem.Server.Exceptions;

public interface IHttpException
{
	string FieldName { get; }
	string Message { get; }
}
