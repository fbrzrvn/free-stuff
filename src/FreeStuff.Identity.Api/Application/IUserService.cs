using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;

namespace FreeStuff.Identity.Api.Application;

public interface IUserService
{
    Task<IResult> RegisterUser(RegisterUserRequest request);

    Task<IResult> AdminOnly(ClaimsPrincipal claims);
}
