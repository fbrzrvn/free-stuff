namespace FreeStuff.Domain.Item;

public interface IItemRepository
{
    bool CreateAsync(ItemEntity itemEntity);

    ItemEntity? GetAsync(Guid id);

    ItemEntity? GetByTitleAsync(string title);

    IEnumerable<ItemEntity> GetAllAsync();

    bool DeleteAsync(Guid id);
}
