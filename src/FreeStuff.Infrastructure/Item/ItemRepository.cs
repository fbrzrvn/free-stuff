using FreeStuff.Domain.Item;

namespace FreeStuff.Infrastructure.Item;

public class ItemRepository : IItemRepository
{
    private static readonly List<ItemEntity> _items = new();

    public void CreateAsync(ItemEntity itemEntity) { _items.Add(itemEntity); }

    public ItemEntity? GetAsync(Guid id) { return _items.SingleOrDefault(item => item.Id.Value == id); }

    public ItemEntity? GetByTitleAsync(string title) { return _items.SingleOrDefault(item => item.Title == title); }

    public IEnumerable<ItemEntity> GetAllAsync() { return _items; }
}
