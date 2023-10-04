using ErrorOr;
using FreeStuff.Items.Application.Shared;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MediatR;

namespace FreeStuff.Items.Application.Get;

public sealed class GetItemQueryHandler : IRequestHandler<GetItemQuery, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<ItemDto>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id));

        if (item is null) throw new NullReferenceException();

        return item.MapToItemDto();
    }
}