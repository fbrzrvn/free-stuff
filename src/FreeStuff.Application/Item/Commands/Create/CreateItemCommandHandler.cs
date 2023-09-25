using FreeStuff.Domain.Item;
using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Create;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ErrorOr<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<ItemEntity>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_itemRepository.GetByTitleAsync(command.Title) is not null) return Errors.Item.DuplicateTitleError;

        var userId = UserId.CreateUnique();

        var item = ItemEntity.Create(command.Title, command.Description, command.Condition, userId);

        _itemRepository.CreateAsync(item);

        return item;
    }
}
