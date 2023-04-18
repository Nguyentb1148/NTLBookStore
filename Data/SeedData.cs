using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.Helpers;
using NTLBookStore.Models;

namespace NTLBookStore.Data;

public class SeedData
{
    public async static Task SeedAsync(IServiceProvider sp)
    {
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(Roles.User))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(Roles.StoreOwner))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.StoreOwner));
        }

        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

        if (await userManager.FindByNameAsync("admin@gmail.com") == null)
        {
            var admin = new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                FullName = "Administrator",
                HomeAddress = "abc",
                PhoneNumber = "123456789",
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(admin, "Asd@123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var db = sp.GetRequiredService<NTLBookStoreContext>();

        List<Category> categories = new()
        {
            new Category()
            {
                Name = "Comic"
            },
            new Category()
            {
                Name = "Horror",
            },
            new Category()
            {
                Name= "Fantasy"
            }
        };

        foreach (var category in categories)
        {
            if (!await db.Categories.AnyAsync(c => c.Name == category.Name))
            {
                await db.Categories.AddAsync(category);
                await db.SaveChangesAsync();
            }
        }
    }
}
