using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_FirstApp.Constants
{
    public enum AccountOperation
    {
        [Description("wypłata")]
        Withdrawal,
        [Description("należność")]
        Salary
    }
}
