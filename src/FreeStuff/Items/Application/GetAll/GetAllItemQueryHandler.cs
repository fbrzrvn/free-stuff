using ErrorOr;
using FreeStuff.Items.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public class GetAllItemQueryHandler : IRequestHandler<GetAllItemQuery, ErrorOr<ItemsDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public GetAllItemQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<ItemsDto>> Handle
        (GetAllItemQuery request, CancellationToken cancellationToken)
    {
        var items      = await _itemRepository.GetAllAsync(request.Page, request.Limit);
        var totalItems = _itemRepository.GetCount();

        var result = _mapper.Map<ItemsDto>(
            (
                items.ToList(),
                request,
                totalItems
            )
        );

        return result;
    }
}
