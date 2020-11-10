using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_FirstApp.Models
{
    public class MvcDbContext : IdentityDbContext
    {
        public MvcDbContext(DbContextOptions options) : base(options) { }
    }
}
