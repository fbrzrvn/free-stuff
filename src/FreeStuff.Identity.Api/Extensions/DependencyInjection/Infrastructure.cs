using System.Text;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Enum;
using FreeStuff.Identity.Api.Infrastructure.EntityFramework;
using FreeStuff.Identity.Api.Infrastructure.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FreeStuff.Identity.Api.Extensions.DependencyInjection;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDb(services, configuration);
        AddIdentity(services, configuration);

        return services;
    }

    private static IServiceCollection AddDb(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<FreeStuffIdentityDbContext>(
            options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        return services;
    }

       private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettings = new TokenConfig();
        configuration.Bind("Jwt", tokenSettings);

        services.AddSingleton(Options.Create(tokenSettings));
        services.AddSingleton<ITokenManager, TokenManager>();

        services
            .AddIdentityCore<User>(
                options =>
                {
                    options.Password.RequireDigit           = true;
                    options.Password.RequireLowercase       = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase       = true;
                    options.Password.RequiredLength         = 10;

                    options.User.RequireUniqueEmail = true;

                    // options.Tokens.AuthenticatorIssuer            = "FreeStuff.Identity.API";
                    // options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    // options.Tokens.PasswordResetTokenProvider     = TokenOptions.DefaultEmailProvider;
                    // options.Tokens.ChangeEmailTokenProvider       = TokenOptions.DefaultEmailProvider;

                    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    // options.SignIn.RequireConfirmedEmail = true;
                }
            )
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FreeStuffIdentityDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddHttpContextAccessor()
            .AddAuthorization(
                options =>
                {
                    options.AddPolicy("Admin", policy => policy.RequireRole(Role.Admin.ToString()));
                }
            )
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenSettings.Issuer,
                        ValidAudience = tokenSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret))
                    };
                }
            );

        return services;
    }
}
