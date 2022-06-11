using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities
{
    public partial class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() { }
        public ApplicationUser(string userName) : base(userName) { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountNumber { get; set; }
        public string Role { get; set; }

        public Guid? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public Guid UserBillingId { get; set; }
        public virtual UserBilling UserBilling { get; set; } = new();

        public virtual List<Request> Requests { get; set; } = new();

        public virtual List<UserOperation> UserOperations { get; set; } = new();
    }
}
