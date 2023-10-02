using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Create;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ErrorOr<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<ItemEntity>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        if (await _itemRepository.GetByTitleAsync(command.Title) is not null) return Errors.Item.DuplicateTitleError;

        var item = ItemEntity.Create(
            command.Title,
            command.Description,
            command.Condition,
            UserId.Create(command.UserId)
        );

        await _itemRepository.CreateAsync(item);

        return item;
    }
}