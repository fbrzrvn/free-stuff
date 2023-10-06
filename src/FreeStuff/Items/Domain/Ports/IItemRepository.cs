using FreeStuff.Items.Domain.ValueObjects;

namespace FreeStuff.Items.Domain.Ports;

public interface IItemRepository
{
    Task CreateAsync(Item item, CancellationToken cancellationToken);

    Task<Item?> GetAsync(ItemId id);

    Task<IEnumerable<Item>?> GetAllAsync(int page, int limit);

    void Update(Item item);

    void Delete(Item item);

    Task SaveChangesAsync();

    int GetCount();
}
