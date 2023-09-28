namespace FreeStuff.Domain.Item;

public interface IItemRepository
{
    bool CreateAsync(ItemEntity itemEntity);

    ItemEntity? GetAsync(Guid id);

    ItemEntity? GetByTitleAsync(string title);

    IEnumerable<ItemEntity> GetAllAsync();

    ItemEntity? UpdateAsync(Guid id, string title, string description, string condition);

    bool DeleteAsync(Guid id);
}
