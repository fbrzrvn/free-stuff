using Mapster;
using MapsterMapper;

namespace FreeStuff.Identity.Api.Extensions.DependencyInjection;

public static class Mapper
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(Program).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
