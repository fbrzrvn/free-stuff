using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreeStuff.Identity.Api.Domain;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Name           = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name           = "User",
                NormalizedName = "USER"
            }
        );
    }
}
