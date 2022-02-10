using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Helpers
{
    public static class TimeSpanHelpers
    {
        public static string ToHoursAndMins(this TimeSpan ts)
        {
            return string.Format("{0:%h} h {0:%m} min.", ts);
        }
    }
}
