using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Installers
{
    public class ServiceAttributes
    {
        public class ScopedRegistrationAttribute : Attribute { }
        public class SingletonRegistrationAttribute : Attribute { }
        public class TransientRegistrationAttribute : Attribute { }
    }
}
