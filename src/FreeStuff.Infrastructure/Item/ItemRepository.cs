using FreeStuff.Domain.Item;

namespace FreeStuff.Infrastructure.Item;

public class ItemRepository : IItemRepository
{
    private static readonly List<ItemEntity> _items = new();

    public bool CreateAsync(ItemEntity itemEntity)
    {
        _items.Add(itemEntity);

        return GetByTitleAsync(itemEntity.Title) is not null;
    }

    public ItemEntity? GetAsync(Guid id) { return _items.SingleOrDefault(item => item.Id.Value == id); }

    public ItemEntity? GetByTitleAsync(string title) { return _items.SingleOrDefault(item => item.Title == title); }

    public IEnumerable<ItemEntity> GetAllAsync() { return _items; }

    public bool DeleteAsync(Guid id)
    {
        var item = GetAsync(id);

        return item is not null && _items.Remove(item);
    }
}
