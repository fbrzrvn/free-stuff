using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Queries.Get;

public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ErrorOr<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemQueryHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<ItemEntity>> Handle(GetItemQuery query, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(query.Id);

        if (item is null) return Errors.Item.NotFoundError(query.Id);

        return item;
    }
}
