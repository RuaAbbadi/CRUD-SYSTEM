using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using OnlineLibrary.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
           
           
            builder.Services.AddRazorPages();


            //  register DbContext
            string connString = builder.Configuration.GetConnectionString("LibraryConn");
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(connString));
            
            builder.Services.AddIdentity<AppUser, AppRole>(
                            options =>
                            {
                                options.Lockout.MaxFailedAccessAttempts = 5;
                                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                            }

                            ).AddDefaultTokenProviders()
                            .AddEntityFrameworkStores<LibraryDbContext>();

           


            //Send Email

            builder.Services.AddTransient<IEmailSender, EcommerceEmailSender>();

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
            opt =>
            {
                //configure your other properties
                opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
                opt.LoginPath = "/Identity/Account/Login";

            });



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
            app.UseAuthentication();;

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}