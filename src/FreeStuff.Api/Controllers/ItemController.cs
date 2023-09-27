using FreeStuff.Application.Item.Commands.Create;
using FreeStuff.Application.Item.Commands.Delete;
using FreeStuff.Application.Item.Queries.Get;
using FreeStuff.Application.Item.Queries.GetAll;
using FreeStuff.Contracts.Item;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers;

[Route("items")]
public class ItemController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public ItemController(IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ItemRequest request)
    {
        var command = _mapper.Map<CreateItemCommand>(request);
        var result  = await _sender.Send(command);

        return result.Match(
            item => Ok(_mapper.Map<ItemResponse>(item)),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var query  = new GetItemQuery(id);
        var result = await _sender.Send(query);

        return result.Match(
            item => Ok(_mapper.Map<ItemResponse>(item)),
            errors => Problem(errors)
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var query  = new GetAllItemsQuery();
        var result = await _sender.Send(query);

        return Ok(_mapper.Map<List<ItemResponse>>(result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var query  = new DeleteItemCommand(id);
        var result = await _sender.Send(query);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}
