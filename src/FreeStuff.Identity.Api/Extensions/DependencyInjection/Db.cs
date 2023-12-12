using FreeStuff.Identity.Api.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Identity.Api.Extensions.DependencyInjection;

public static class Db
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<FreeStuffIdentityDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        return services;
    }
}
