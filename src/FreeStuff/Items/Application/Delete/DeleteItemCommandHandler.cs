using ErrorOr;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MediatR;

namespace FreeStuff.Items.Application.Delete;

public sealed class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ErrorOr<bool>>
{
    private readonly IItemRepository _itemRepository;

    public DeleteItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id));

        if (item is null) return Errors.Item.NotFoundError(request.Id);

        _itemRepository.Delete(item);

        await _itemRepository.SaveChangesAsync();

        return true;
    }
}
