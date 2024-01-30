using Carter;
using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Application;

namespace FreeStuff.Identity.Api.Features.Identity;

public class IdentityModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Register, RegisterUser);
        app.MapPost(ApiEndpoints.Login, LoginUser);
        app.MapPost(ApiEndpoints.RefreshToken, ProcessRefreshTokenRequestAsync).RequireAuthorization();
        app.MapPost(ApiEndpoints.AdminOnly, AdminOnly).RequireAuthorization();
    }

    private static async Task<IResult> RegisterUser(RegisterUserRequest request, IUserService userService)
    {
        return await userService.RegisterUser(request);
    }

    private static async Task<IResult> LoginUser(LoginUserRequest request, IAuthenticationService authenticationService)
    {
        return await authenticationService.LoginUser(request);
    }

    private static async Task<IResult> ProcessRefreshTokenRequestAsync(
        RefreshTokenRequest    request,
        IHttpContextAccessor   contextAccessor,
        IAuthenticationService authenticationService
    )
    {
        var claims = contextAccessor.HttpContext!.User;

        return await authenticationService.ProcessRefreshTokenRequestAsync(request, claims);
    }

    private static async Task<IResult> AdminOnly(IHttpContextAccessor contextAccessor, IUserService userService)
    {
        var claims = contextAccessor.HttpContext!.User;

        return await userService.AdminOnly(claims);
    }
}
