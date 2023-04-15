using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.Areas.Identity.Data;

namespace NTLBookStore.Areas.Identity.Data;

public class NTLBookStoreContext : IdentityDbContext<ApplicationUser>
{
    public NTLBookStoreContext(DbContextOptions<NTLBookStoreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
