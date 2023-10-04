using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.User.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FreeStuff.Shared.Infrastructure;

public class FreeStuffDbContext : DbContext
{
    public FreeStuffDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreeStuffDbContext).Assembly);

        modelBuilder.Entity<Item>(
            entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Id).HasConversion(id => id.Value, value => ItemId.Create(value));
                entity.Property(item => item.Title).HasMaxLength(100);
                entity.Property(item => item.Description).HasMaxLength(500);
                entity.Property(item => item.UserId)
                      .HasConversion(userId => userId.Value, value => UserId.Create(value));
            }
        );

        // Setting to avoid generating primary key for all entities
        modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetProperties())
                    .Where(p => p.IsPrimaryKey())
                    .ToList()
                    .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        base.OnModelCreating(modelBuilder);
    }
}
