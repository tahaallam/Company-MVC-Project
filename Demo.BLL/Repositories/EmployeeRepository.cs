using Demo.BLL.Interfaces;
using Demo.DAL.DbContexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcAppDbContext _context;

        public EmployeeRepository(MvcAppDbContext context):base(context)
        {
            _context = context;
        }
        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _context.Employees.Where(E=>E.Address ==  address);
        }

        public IQueryable<Employee> GetEmployeeByName(string searchValue)
        {
            return (_context.Employees.Where(E => E.Name.ToLower().Contains(searchValue.ToLower())||
                                             E.Email.ToLower().Contains(searchValue.ToLower()) ||
                                             E.Address.ToLower().Contains(searchValue.ToLower()) ||
                                             E.PhoneNumber.ToLower().Contains(searchValue.ToLower()) 
                                             
            )).Include(E=>E.department);
        }


        //public int Add(Employee employee)
        ////{
        //    _context.Employees.Add(employee);
        //    return _context.SaveChanges();  
        //}

        //public int Delete(Employee employee)
        //{
        //    _context.Remove(employee);
        //    return _context.SaveChanges();
        //}
        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();   
        //}

        //public Employee GetById(int id)
        //{
        //  return  _context.Employees.Find(id);
        //}

        //public int Update(Employee employee)
        //{
        //    _context.Employees.Update(employee);
        //    return _context.SaveChanges();  
        //}
    }
}
 