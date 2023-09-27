using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Delete;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ErrorOr<bool>>
{
    private readonly IItemRepository _itemRepository;

    public DeleteItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var isItemDeleted = _itemRepository.DeleteAsync(request.Id);

        if (!isItemDeleted) return Errors.Item.NotFoundError(request.Id);

        return true;
    }
}
