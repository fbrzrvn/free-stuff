using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Application;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Enum;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace FreeStuff.Identity.Api.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IMapper                   _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User>         _userManager;

    public UserService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper      = mapper;
    }

    public async Task<IResult> RegisterUserAsync(RegisterUserRequest request)
    {
        request = request with { Role = request.Role ?? Role.RegularUser.Name };

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            return Results.BadRequest($"Role '{request.Role}' does not exist.");
        }

        var user   = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        var role = await _userManager.AddToRoleAsync(user, request.Role);

        if (!role.Succeeded)
        {
            return Results.BadRequest(role.Errors);
        }

        return Results.Created();
    }

    public async Task<IResult> AdminOnly(ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUserAsync(claims);

        if (!await _userManager.IsInRoleAsync(user!, Role.Admin.Name))
        {
            return Results.Forbid();
        }

        return Results.Ok("You are an admin");
    }
}
