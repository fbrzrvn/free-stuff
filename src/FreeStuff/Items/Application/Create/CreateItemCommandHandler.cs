using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using ErrorOr;
using FreeStuff.Items.Application.Shared;
using MediatR;

namespace FreeStuff.Items.Application.Create;

public sealed class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
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

        return item.MapToItemDto();
    }
}
