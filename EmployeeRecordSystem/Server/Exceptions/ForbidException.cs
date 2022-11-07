using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Exceptions
{
    public sealed class ForbidException : Exception, IHttpException
    {
        public string FieldName { get; }
        public override string Message { get; } = "User not authorized.";

        public ForbidException()
        {
            FieldName = "User";
        }
        
        public ForbidException(string details) : this()
        {
            Message += $" {details}";
        }
    }
}
