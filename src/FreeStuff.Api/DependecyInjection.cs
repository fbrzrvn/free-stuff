using FreeStuff.Api.Mapping;

namespace FreeStuff.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();

        return services;
    }
}
