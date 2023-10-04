using FreeStuff.Api.Controllers.Items.Requests;
using FreeStuff.Items.Application.Create;
using FreeStuff.Items.Application.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers.Items;

[Route("items")]
public class ItemsController : ApiController
{
    private readonly ISender _bus;

    public ItemsController(ISender bus)
    {
        _bus = bus;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateItemCommand(
            request.Title,
            request.Description,
            request.Condition,
            request.UserId
        );
        var result = await _bus.Send(command, cancellationToken);

        return result.Match(
            item => CreatedAtAction(
                nameof(Get),
                new { id = item.Id },
                item
            ),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}", Name = "Get")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query  = new GetItemQuery(id);
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            item => Ok(item),
            errors => Problem(errors)
        );
    }
}
