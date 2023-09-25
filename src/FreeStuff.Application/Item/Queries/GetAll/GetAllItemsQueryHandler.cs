using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Queries.GetAll;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public GetAllItemsQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<IEnumerable<ItemEntity>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _itemRepository.GetAllAsync().ToList();
    }
}
