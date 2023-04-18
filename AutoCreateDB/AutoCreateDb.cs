using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.Data;
using NTLBookStore.Models;
using System;
using System.Linq;

namespace NTLBookStore.AutoCreateDB
{
    public class AutoCreateDb : IAutoCreateDb
    {
        private readonly NTLBookStoreContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AutoCreateDb(NTLBookStoreContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void CreateDb()
        {
            // checking database, if not migration then migrate
            try
            {
                if (_db.Database.GetPendingMigrations().Any()) 
                {
                    _db.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error applying migrations: " + e.Message);
                throw;
            }

            // checking in table Role, if yes then return, if not deploy the codes after these conditions
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.User)) return;
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.StoreOwner)) return;
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.Admin)) return;

            // this will deploy if there no have any role yet
            _roleManager.CreateAsync(new IdentityRole(Helpers.Roles.User)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helpers.Roles.StoreOwner)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helpers.Roles.Admin)).GetAwaiter().GetResult();

            // create user admin
            var admin = new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                FullName = "Admin",
                HomeAddress = "Admin123",
                PhoneNumber = "0869502968",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var result = _userManager.CreateAsync(admin, "Nguyen123@").GetAwaiter().GetResult();

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create admin user: {string.Join(",", result.Errors)}");
            }

            // finding the user which is just have created
            admin = _db.Users.FirstOrDefault(a => a.Email == "admin@gmail.com");

            // add that user (admin) to admin role
            _userManager.AddToRoleAsync(admin, Helpers.Roles.Admin).GetAwaiter().GetResult();
        }
    }
}
