using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Group Group { get; set; }
        public virtual Position Position { get; set; }
        public virtual Billing Billing { get; set; }
        public virtual List<AccountAction> AccountHistory { get; set; }
    }
}
