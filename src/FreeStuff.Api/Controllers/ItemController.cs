using FreeStuff.Application.Item.Commands.Create;
using FreeStuff.Contracts.Item;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers;

public class ItemController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ItemController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("items")]
    public async Task<IActionResult> Create([FromBody] ItemRequest request)
    {
        var command = _mapper.Map<CreateItemCommand>(request);
        var result  = await _sender.Send(command);

        return result.Match(
            item => Ok(_mapper.Map<ItemResponse>(item)),
            errors => Problem(errors)
        );
    }
}
