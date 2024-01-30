using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;

namespace FreeStuff.Identity.Api.Application;

public interface IAuthenticationService
{
    Task<IResult> LoginUser(LoginUserRequest request);

    Task<IResult> ProcessRefreshTokenRequestAsync(RefreshTokenRequest refreshTokenRequest, ClaimsPrincipal claims);
}
