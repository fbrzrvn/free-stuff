using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public sealed class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, ErrorOr<List<ItemDto>>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public GetAllItemsQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<List<ItemDto>>> Handle
        (GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAllAsync(
            request.Page,
            request.Limit,
            cancellationToken
        );

        var result = _mapper.Map<List<ItemDto>>((items ?? Array.Empty<Item>()).ToList());

        return result;
    }
}
