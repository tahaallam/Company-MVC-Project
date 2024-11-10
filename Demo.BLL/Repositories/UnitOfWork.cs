using Demo.BLL.Interfaces;
using Demo.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDepartmentRepository departmentRepository { get; set; }
        public IEmployeeRepository employeeRepository { get; set; }

        private readonly MvcAppDbContext _context;

        public UnitOfWork(MvcAppDbContext context)
        {
           employeeRepository = new EmployeeRepository(context);
            departmentRepository = new DepartmentRepository(context);
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
