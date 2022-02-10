using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual List<User> Users { get; set; } = new();
    }
}
