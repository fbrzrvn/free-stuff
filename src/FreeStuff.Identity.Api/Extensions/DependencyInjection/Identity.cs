using System.Text;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Ports;
using FreeStuff.Identity.Api.Infrastructure.Authentication;
using FreeStuff.Identity.Api.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FreeStuff.Identity.Api.Extensions.DependencyInjection;

public static class Identity
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind("Jwt", jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .AddIdentityCore<User>(options =>
                {
                    options.Password.RequireDigit           = true;
                    options.Password.RequireLowercase       = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase       = true;
                    options.Password.RequiredLength         = 10;
                    options.User.RequireUniqueEmail         = true;
                }
            )
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FreeStuffIdentityDbContext>();

        services
            .AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = jwtSettings.Issuer,
                        ValidAudience            = jwtSettings.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                }
            );

        return services;
    }
}
