using FreeStuff.Domain.Item;

namespace FreeStuff.Infrastructure.Item;

public class ItemRepository : IItemRepository
{
    private static readonly List<ItemEntity> _items = new();

    public void Create(ItemEntity itemEntity) { _items.Add(itemEntity); }

    public ItemEntity? GetItem(Guid id) { return _items.SingleOrDefault(item => item.Id.Value == id); }

    public ItemEntity? GetItemByTitle(string title) { return _items.SingleOrDefault(item => item.Title == title); }
}
