using FreeStuff.Identity.Api.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Identity.Api.Infrastructure.EntityFramework;

public class FreeStuffIdentityDbContext : IdentityDbContext<User>
{
    public FreeStuffIdentityDbContext(DbContextOptions<FreeStuffIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
