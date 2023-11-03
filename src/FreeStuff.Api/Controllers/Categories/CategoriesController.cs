using FreeStuff.Categories.Application.Create;
using FreeStuff.Categories.Application.GetAll;
using FreeStuff.Categories.Application.Update;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Contracts.Categories.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers.Categories;

[Route("categories")]
public class CategoriesController : ApiController
{
    private readonly ISender _bus;
    private readonly IMapper _mapper;

    public CategoriesController(ISender bus, IMapper mapper)
    {
        _bus    = bus;
        _mapper = mapper;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<CreateCategoryCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors)
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query  = new GetAllCategoriesQuery();
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            categories => Ok(_mapper.Map<List<CategoryResponse>>(categories)),
            errors => Problem(errors)
        );
    }

    [HttpPut("")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<UpdateCategoryCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors)
        );
    }
}
