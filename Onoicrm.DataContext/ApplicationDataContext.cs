using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Onoicrm.DataContext;

public sealed class ApplicationDataContext: IdentityDbContext<IdentityUser>
{
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDataContext)) ?? throw new InvalidOperationException());
    }
}