using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Contracts.Items.Responses;
using FreeStuff.Items.Application.Create;
using FreeStuff.Items.Application.Delete;
using FreeStuff.Items.Application.Get;
using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Application.Search;
using FreeStuff.Items.Application.Update;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers.Items;

[Route("items")]
public class ItemsController : ApiController
{
    private readonly ISender _bus;
    private readonly IMapper _mapper;

    public ItemsController(ISender bus, IMapper mapper)
    {
        _bus    = bus;
        _mapper = mapper;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateItemCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            item => CreatedAtAction(
                nameof(Get),
                new { id = item.Id },
                item
            ),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}", Name = "Get")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query  = new GetItemQuery(id);
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            item => Ok(_mapper.Map<ItemResponse>(item)),
            errors => Problem(errors)
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllItemsRequest request, CancellationToken cancellationToken)
    {
        var query  = new GetAllItemsQuery(request.Page, request.Limit);
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            // items.Count must be come from the handler to have the real total count of items in db.
            // at the moment is just the total of the result send by the handler
            items => Ok(_mapper.Map<ItemsResponse>((items, request, items.Count))),
            errors => Problem(errors)
        );
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchItemsRequest request, CancellationToken cancellationToken)
    {
        var query = new SearchItemsQuery(
            request.Title,
            request.Condition,
            request.SortBy
        );
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            items => Ok(_mapper.Map<ItemsResponse>((items, request, items.Count))),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateItemRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<UpdateItemCommand>((id, request));
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            item => Ok(_mapper.Map<ItemResponse>(item)),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteItemCommand(id);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
