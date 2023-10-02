using FreeStuff.Domain.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FreeStuff.Infrastructure;

public class FreeStuffDbContext : DbContext
{
    public FreeStuffDbContext
    (
        DbContextOptions options
    ) : base(options)
    {
    }

    public DbSet<ItemEntity> Items { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreeStuffDbContext).Assembly);

        // Setting to avoid generating primary key for all entities
        modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetProperties())
                    .Where(p => p.IsPrimaryKey())
                    .ToList()
                    .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        base.OnModelCreating(modelBuilder);
    }
}
