using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Constants
{
    public enum AccountOperation
    {
        [Description("wypłata")]
        Withdrawal,
        [Description("należność")]
        Salary
    }
}
