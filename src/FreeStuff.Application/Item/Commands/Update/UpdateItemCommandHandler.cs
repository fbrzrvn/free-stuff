using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Update;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ErrorOr<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public UpdateItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<ItemEntity>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var item = _itemRepository.UpdateAsync(request.Id, request.Title, request.Description, request.Condition);

        if (item is null) return Errors.Item.NotFoundError(request.Id);

        return item;
    }
}
