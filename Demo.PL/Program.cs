using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.DbContexts;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.MappingProfiles;
using Demo.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
          builder.Services.AddDbContext<MvcAppDbContext>(Options =>
          {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
          
          );
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository , EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile() ));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile() ));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile() ));
            builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>(){
                new EmployeeProfile() , new DepartmentProfile() , new UserProfile() , new RoleProfile()
            }));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<MvcAppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "Account/Login";
                options.AccessDeniedPath = "Home/ Error";
            });
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddTransient<IMailService, EmailSettings>();
            builder.Services.AddTransient<ISmsService, SmsService>();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
