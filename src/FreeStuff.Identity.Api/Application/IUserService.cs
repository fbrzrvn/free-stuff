using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;

namespace FreeStuff.Identity.Api.Application;

public interface IUserService
{
    Task<IResult> RegisterUserAsync(RegisterUserRequest request);

    Task<IResult> AdminOnly(ClaimsPrincipal claims);
}
