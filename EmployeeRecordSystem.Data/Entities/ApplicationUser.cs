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
        public string FullName 
        {
            get => $"{FirstName} {LastName}";
        }

        public Guid? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual UserBilling UserBilling { get; set; } = new();

        public virtual List<WithdrawalRequest> WithdrawalRequests { get; set; } = new();

        public virtual List<BalanceLog> BalanceLogs { get; set; } = new();
    }
}
