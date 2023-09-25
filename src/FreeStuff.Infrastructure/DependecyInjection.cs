using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using FreeStuff.Infrastructure.Item;
using FreeStuff.Infrastructure.User;
using Microsoft.Extensions.DependencyInjection;

namespace FreeStuff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
