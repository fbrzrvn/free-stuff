using FreeStuff.Domain.Item;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Infrastructure.Item;

public class ItemRepository : IItemRepository
{
    private readonly FreeStuffDbContext _dbContext;

    public ItemRepository(FreeStuffDbContext dbContext) { _dbContext = dbContext; }

    public async Task<bool> CreateAsync(ItemEntity itemEntity)
    {
        _dbContext.Add(itemEntity);

        await _dbContext.SaveChangesAsync();

        return await GetByTitleAsync(itemEntity.Title) is not null;
    }

    public async Task<ItemEntity?> GetAsync(Guid id)
    {
            var item = await _dbContext.Items.FirstOrDefaultAsync();
            return item;

    }

    public async Task<ItemEntity?> GetByTitleAsync(string title)
    {
        return await _dbContext.Items.SingleOrDefaultAsync(
            item => item.Title == title
        );
    }

    public async Task<IEnumerable<ItemEntity>?> GetAllAsync() { return await _dbContext.Items.ToListAsync(); }

    public async Task<bool> UpdateAsync(ItemEntity itemEntity)
    {
        _dbContext.Items.Update(itemEntity);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await GetAsync(id);

        if (item is null) return false;

        _dbContext.Items.Remove(item);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
