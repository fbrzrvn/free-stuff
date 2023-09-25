using System.Reflection;
using FluentValidation;
using FreeStuff.Application.common;
using FreeStuff.Application.Item;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FreeStuff.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}