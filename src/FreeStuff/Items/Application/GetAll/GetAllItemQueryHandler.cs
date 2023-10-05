using ErrorOr;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain.Ports;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public class GetAllItemQueryHandler : IRequestHandler<GetAllItemQuery, ErrorOr<ItemsDto>>
{
    private readonly IItemRepository _itemRepository;

    public GetAllItemQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<ItemsDto>> Handle
        (GetAllItemQuery request, CancellationToken cancellationToken)
    {
        var items      = await _itemRepository.GetAllAsync(request.Page, request.Limit);
        var totalItems = _itemRepository.GetCount();

        var result = items?.MapToItemsDto(
                         request.Page,
                         request.Limit,
                         totalItems
                     ) ??
                     new ErrorOr<ItemsDto>();

        return result;
    }
}
