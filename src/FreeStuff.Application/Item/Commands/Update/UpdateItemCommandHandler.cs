using ErrorOr;
using FreeStuff.Domain.common.Errors;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Update;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ErrorOr<ItemEntity>>
{
    private readonly IItemRepository _itemRepository;

    public UpdateItemCommandHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

    public async Task<ErrorOr<ItemEntity>> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var itemEntity = await _itemRepository.GetAsync(command.Id);

        if (itemEntity is null) return Errors.Item.NotFoundError(command.Id);

        await _itemRepository.UpdateAsync(itemEntity);

        return itemEntity;
    }
}
