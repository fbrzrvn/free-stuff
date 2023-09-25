namespace FreeStuff.Domain.Item;

public interface IItemRepository
{
    void Create(ItemEntity itemEntity);

    ItemEntity? GetItem(Guid id);

    ItemEntity? GetItemByTitle(string title);
}
