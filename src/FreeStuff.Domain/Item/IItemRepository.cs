namespace FreeStuff.Domain.Item;

public interface IItemRepository
{
    Task<bool> CreateAsync(ItemEntity itemEntity);

    Task<ItemEntity?> GetAsync(Guid id);

    Task<ItemEntity?> GetByTitleAsync(string title);

    Task<IEnumerable<ItemEntity>?> GetAllAsync();

    Task<bool> UpdateAsync(ItemEntity itemEntity);

    Task<bool> DeleteAsync(Guid id);
}
