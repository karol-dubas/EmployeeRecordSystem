using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public GroupEnum Group { get; set; }
        [Required]
        public PositionEnum Position { get; set; }
        [Required]
        public virtual BillingEntity Billing { get; set; }
    }
}
