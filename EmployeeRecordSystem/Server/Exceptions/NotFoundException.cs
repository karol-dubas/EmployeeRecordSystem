using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Exceptions;

public sealed class NotFoundException : Exception, IHttpException
{
    public string FieldName { get; }
    public override string Message { get; }

    public NotFoundException(string fieldName, string entity)
    {
        FieldName = fieldName;
        Message = $"{entity} not found";
    }
}