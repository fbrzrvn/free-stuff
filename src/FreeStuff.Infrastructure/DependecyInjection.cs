using FreeStuff.Domain.Item;
using FreeStuff.Infrastructure.Item;
using Microsoft.Extensions.DependencyInjection;

namespace FreeStuff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();

        return services;
    }
}
