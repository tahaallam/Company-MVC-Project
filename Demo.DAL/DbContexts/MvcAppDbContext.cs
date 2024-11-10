using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.DbContexts
{
    public class MvcAppDbContext:IdentityDbContext<ApplicationUser>
    {
        public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options ):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("Server = 'DESKTOP-SGO0H8A\\SQLEXPRESS' ; Database= MVCAPPWITHALT ; Trusted_Connection=true;encrypt=false;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
       // DbSet<Department> Departments = new DbSet<Department>();
       public DbSet<Department>  Departments   {  get; set; }  
       public DbSet<Employee>  Employees   {  get; set; }
       
    }
}
