using FreeStuff.Application.User.Commands;
using FreeStuff.Contracts.User;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers;

public class UserController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UserController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] UserRequest request)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        var result  = await _sender.Send(command);

        return result.Match(
            user => Ok(_mapper.Map<UserResponse>(user)),
            errors => Problem(errors));
    }
}
