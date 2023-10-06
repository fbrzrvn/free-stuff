using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Create;

public sealed class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public CreateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<ItemDto>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = Item.Create(
            request.Title,
            request.Description,
            request.Condition.MapStringToItemCondition(),
            request.UserId
        );

        await _itemRepository.CreateAsync(item, cancellationToken);
        await _itemRepository.SaveChangesAsync();

        var result = _mapper.Map<ItemDto>(item);

        return result;
    }
}
