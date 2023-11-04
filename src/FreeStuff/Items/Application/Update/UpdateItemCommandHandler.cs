using ErrorOr;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Update;

public sealed class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ErrorOr<ItemDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IItemRepository     _itemRepository;
    private readonly IMapper             _mapper;

    public UpdateItemCommandHandler(
        ICategoryRepository categoryRepository,
        IItemRepository     itemRepository,
        IMapper             mapper
    )
    {
        _categoryRepository = categoryRepository;
        _itemRepository     = itemRepository;
        _mapper             = mapper;
    }

    public async Task<ErrorOr<ItemDto>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.CategoryName, cancellationToken);

        if (category is null)
        {
            return Categories.Domain.Errors.Errors.Category.NotFound(request.CategoryName);
        }

        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id), cancellationToken);

        if (item is null)
        {
            return Errors.Item.NotFound(request.Id);
        }

        item.Update(
            request.Title,
            request.Description,
            category,
            request.Condition.MapExactStringToItemCondition()
        );

        _itemRepository.Update(item);
        await _itemRepository.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<ItemDto>(item);

        return result;
    }
}
