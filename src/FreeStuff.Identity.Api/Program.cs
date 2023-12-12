using Carter;
using FreeStuff.Identity.Api.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDb(builder.Configuration);
    builder.Services.AddIdentity(builder.Configuration);
    builder.Services.AddMapper();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCarter();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseHttpsRedirection();

    app.MapCarter();

    app.Run();
}
