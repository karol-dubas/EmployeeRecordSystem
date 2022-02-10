using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public long? PositionId { get; set; }
        public virtual Position Position { get; set; }

        public int? UserBillingId { get; set; }
        public virtual UserBilling UserBilling { get; set; } = new();

        public virtual List<UserOperation> UserOperations { get; set; }
    }
}
