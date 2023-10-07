using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public sealed class GetAllItemQueryHandler : IRequestHandler<GetAllItemQuery, ErrorOr<List<ItemDto>>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public GetAllItemQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<List<ItemDto>>> Handle
        (GetAllItemQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAllAsync(request.Page, request.Limit, cancellationToken);

        var result = _mapper.Map<List<ItemDto>>((items ?? Array.Empty<Item>()).ToList());

        return result;
    }
}
