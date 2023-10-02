using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreeStuff.Infrastructure.Item;

public class ItemConfigurations : IEntityTypeConfiguration<ItemEntity>
{
    public void Configure(EntityTypeBuilder<ItemEntity> builder) { ConfigureItemTable(builder); }

    private static void ConfigureItemTable(EntityTypeBuilder<ItemEntity> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasConversion(id => id.Value, value => ItemId.Create(value));
        builder.Property(i => i.Title).HasMaxLength(100);
        builder.Property(i => i.Description).HasMaxLength(500);
        builder.Property(i => i.UserId).HasConversion(id => id.Value, value => UserId.Create(value));
    }
}
