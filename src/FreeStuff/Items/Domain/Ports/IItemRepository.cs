using FreeStuff.Items.Domain.ValueObjects;

namespace FreeStuff.Items.Domain.Ports;

public interface IItemRepository
{
    Task CreateAsync(Item item, CancellationToken cancellationToken);

    Task<Item?> GetAsync(ItemId id);

    Task<Item?> GetByTitleAsync(string title);

    Task<IEnumerable<Item>?> GetAllAsync();

    void Update(Item item);

    void Delete(Item item);

    Task SaveChangesAsync();
}
