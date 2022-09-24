using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Exceptions;

public class NotFoundException : Exception
{
    public override string Message { get; }

    public NotFoundException(string entity)
    {
        Message = $"{entity} not found";
    }
}
