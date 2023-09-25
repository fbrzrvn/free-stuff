namespace FreeStuff.Domain.Item;

public interface IItemRepository
{
    void CreateAsync(ItemEntity itemEntity);

    ItemEntity? GetAsync(Guid id);

    ItemEntity? GetByTitleAsync(string title);

    IEnumerable<ItemEntity> GetAllAsync();
}
