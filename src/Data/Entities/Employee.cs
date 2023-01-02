using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Data.Entities;

public class Employee : IdentityUser<Guid>
{
    public Employee() { }
    public Employee(string userName) : base(userName) { }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BankAccountNumber { get; set; }
    public string Note { get; set; }
    public string Role { get; set; }
    public string FullName 
    {
        get => $"{FirstName} {LastName}";
    }

    public Guid? GroupId { get; set; }
    public virtual Group Group { get; set; }

    public virtual EmployeeBilling EmployeeBilling { get; set; } = new();

    public virtual List<WithdrawalRequest> WithdrawalRequests { get; set; } = new();

    public virtual List<BalanceLog> BalanceLogs { get; set; } = new();
    
    public virtual List<Announcement> Announcements { get; set; } = new();
}