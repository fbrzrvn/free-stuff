using ErrorOr;
using FreeStuff.Items.Application.Shared;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MediatR;

namespace FreeStuff.Items.Application.Update;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;

    public UpdateItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<ItemDto>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var existingItem = await _itemRepository.GetAsync(ItemId.Create(request.Id));

        if (existingItem is null) return Errors.Item.NotFoundError(request.Id);

        existingItem.Update(request.Title, request.Description, request.Condition.MapStringToItemCondition());

        _itemRepository.Update(existingItem);

        await _itemRepository.SaveChangesAsync();

        return existingItem.MapToItemDto();
    }
}
