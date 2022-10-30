using AutoMapper;
using EmployeeRecordSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Server.Services
{
    public abstract class BaseService
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IMapper Mapper;

        protected BaseService(ApplicationDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        protected void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        protected async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
