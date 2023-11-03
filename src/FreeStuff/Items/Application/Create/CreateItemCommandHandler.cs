using ErrorOr;
using FreeStuff.Categories.Domain.Errors;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Create;

public sealed class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ErrorOr<ItemDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IItemRepository     _itemRepository;
    private readonly IMapper             _mapper;

    public CreateItemCommandHandler(
        ICategoryRepository categoryRepository,
        IItemRepository     itemRepository,
        IMapper             mapper
    )
    {
        _categoryRepository = categoryRepository;
        _itemRepository     = itemRepository;
        _mapper             = mapper;
    }

    public async Task<ErrorOr<ItemDto>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.CategoryName, cancellationToken);

        if (category is null)
        {
            return Errors.Category.NotFound(request.CategoryName);
        }

        var item = Item.Create(
            request.Title,
            request.Description,
            category,
            request.Condition.MapExactStringToItemCondition(),
            request.UserId
        );

        await _itemRepository.CreateAsync(item, cancellationToken);
        await _itemRepository.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<ItemDto>(item);

        return result;
    }
}
