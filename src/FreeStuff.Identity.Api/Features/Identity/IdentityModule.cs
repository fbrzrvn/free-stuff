using Carter;
using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Ports;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace FreeStuff.Identity.Api.Features.Identity;

public class IdentityModule : ICarterModule
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityModule(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Register, RegisterUser);
        app.MapPost(ApiEndpoints.Token, GetToken);

        app.MapGet(ApiEndpoints.Me, GetMe)
           .RequireAuthorization();
    }

    private static async Task<IResult> RegisterUser(
        RegisterUserRequest request, UserManager<User> userManager, IMapper mapper
    )
    {
        var user   = mapper.Map<User>(request);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        var roles = await userManager.GetRolesAsync(user);
        await userManager.AddToRolesAsync(user, roles);

        return Results.Created();
    }

    private async Task<IResult> GetToken(AuthenticateUserRequest request, UserManager<User> userManager)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.Forbid();
        }

        var roles = await userManager.GetRolesAsync(user);
        var jwt   = _jwtTokenGenerator.GenerateJwt(user, roles);

        return Results.Ok(new
            {
                AccessToken = jwt
            }
        );
    }

    private static async Task<IResult> GetMe(IHttpContextAccessor contextAccessor)
    {
        var user = contextAccessor.HttpContext!.User;

        await Task.CompletedTask;

        return Results.Ok(new
            {
                Claims = user.Claims.Select(s => new
                    {
                        s.Type,
                        s.Value
                    }
                ).ToList(),
                user.Identity!.Name,
                user.Identity.IsAuthenticated,
                user.Identity.AuthenticationType
            }
        );
    }
}
