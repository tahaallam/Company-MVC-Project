using Demo.BLL.Interfaces;
using Demo.DAL.DbContexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository :GenericRepository<Department> ,IDepartmentRepository
    {
        private readonly MvcAppDbContext _dbContext;

        private MvcAppDbContext DbContext;
        public DepartmentRepository(MvcAppDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
