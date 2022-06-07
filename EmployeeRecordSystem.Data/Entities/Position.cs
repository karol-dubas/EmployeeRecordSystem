using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual List<ApplicationUser> Users { get; set; } = new();
    }
}
