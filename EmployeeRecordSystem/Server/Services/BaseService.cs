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
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper;

        protected BaseService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        protected async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
