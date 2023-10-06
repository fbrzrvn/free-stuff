using ErrorOr;
using FreeStuff.Items.Application.Shared;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Update;

public sealed class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<ItemDto>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id));

        if (item is null) return Errors.Item.NotFoundError(request.Id);

        item.Update(
            request.Title,
            request.Description,
            request.Condition.MapStringToItemCondition()
        );
        _itemRepository.Update(item);
        await _itemRepository.SaveChangesAsync();

        var result = _mapper.Map<ItemDto>(item);

        return result;
    }
}
