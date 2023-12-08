using FreeStuff.Identity.Api.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Identity.Api.Infrastructure;

public class FreeStuffIdentityDbContext : IdentityDbContext<User>
{
    public FreeStuffIdentityDbContext(DbContextOptions<FreeStuffIdentityDbContext> options) : base(options)
    {
    }
}
