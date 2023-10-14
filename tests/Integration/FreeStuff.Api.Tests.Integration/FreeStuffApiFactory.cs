using FreeStuff.Shared.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;

namespace FreeStuff.Api.Tests.Integration;

public class FreeStuffApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
                                                   .WithImage("mysql:latest")
                                                   .WithDatabase("fs-db-test")
                                                   .WithUsername("root")
                                                   .WithPassword("password")
                                                   .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll(typeof(DbContextOptions<FreeStuffDbContext>));

                var connectionString = _dbContainer.GetConnectionString();
                services.AddDbContext<FreeStuffDbContext>(
                    options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
            }
        );
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        EnsureDatabaseCreated();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private void EnsureDatabaseCreated()
    {
        using var scope     = Services.CreateScope();
        var       dbContext = scope.ServiceProvider.GetRequiredService<FreeStuffDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
