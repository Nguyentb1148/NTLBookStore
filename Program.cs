using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.AutoCreateDB;
using NTLBookStore.Controllers;
using NTLBookStore.Data;
using NTLBookStore.Helpers;
using NTLBookStore.Models;
using NTLBookStore.Repo;

var builder = WebApplication.CreateBuilder(args);
if (!Directory.Exists(FileUploadHelper.BookImageBaseDirectory))
{
    Directory.CreateDirectory(FileUploadHelper.BookImageBaseDirectory);
}
var connectionString = builder.Configuration.GetConnectionString("LocConnectionSql") ?? throw new InvalidOperationException("Connection string 'NTLBookStoreContextConnection' not found.");

builder.Services.AddDbContext<NTLBookStoreContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NTLBookStoreContext>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAutoCreateDb, AutoCreateDb>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IAutoCreateDb>();
    dbInitializer.CreateDb();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

}
else
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();