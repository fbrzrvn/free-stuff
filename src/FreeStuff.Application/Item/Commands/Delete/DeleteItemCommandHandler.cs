using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Delete;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ErrorOr<bool>>
{
    private readonly IItemRepository _itemRepository;

    public DeleteItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<bool>> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var isItemDeleted = await _itemRepository.DeleteAsync(command.Id);

        if (!isItemDeleted) return Errors.Item.NotFoundError(command.Id);

        return true;
    }
}
