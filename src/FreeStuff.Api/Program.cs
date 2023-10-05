using System.Reflection;
using FluentValidation;
using FreeStuff;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Infrastructure;
using FreeStuff.Shared.Application.Behaviors;
using FreeStuff.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddMediatR(
        cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).GetTypeInfo().Assembly)
    );

    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
    builder.Services.AddValidatorsFromAssembly(typeof(IApplicationMarker).GetTypeInfo().Assembly);

    builder.Services.AddDbContext<FreeStuffDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

    builder.Services.AddScoped<IItemRepository, EfItemRepository>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/errors");

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
