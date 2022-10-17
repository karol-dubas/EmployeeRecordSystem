using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Exceptions
{
    public class ForbidException : Exception
    {
        public override string Message { get; } = "User not authorized.";

        public ForbidException() { }

        public ForbidException(string details)
        {
            Message += $" {details}";
        }
    }
}
