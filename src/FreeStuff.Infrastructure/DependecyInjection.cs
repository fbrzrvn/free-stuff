using FreeStuff.Domain.Item;
using FreeStuff.Infrastructure.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FreeStuff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<FreeStuffDbContext>(
            options => options.UseSqlServer(
                "Server=localhost,1433;Database=FreeStuff;User Id=sa;Password=Password123!;Encrypt=False"
            )
        );

        services.AddScoped<IItemRepository, ItemRepository>();

        return services;
    }
}