using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Items.Infrastructure;

public class EfItemRepository : IItemRepository
{
    private readonly FreeStuffDbContext _context;

    public EfItemRepository(FreeStuffDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Item item, CancellationToken cancellationToken)
    {
        await _context.AddAsync(item, cancellationToken);
    }

    public async Task<Item?> GetAsync(ItemId id)
    {
        var item = await _context.Items.SingleOrDefaultAsync(i => i.Id == id);

        return item;
    }

    public async Task<IEnumerable<Item>?> GetAllAsync(int page, int limit)
    {
        var items = await _context.Items.OrderByDescending(item => item.CreatedDateTime)
                                  .Skip((page - 1) * limit)
                                  .Take(limit)
                                  .ToListAsync();

        return items;
    }

    public void Update(Item item)
    {
        _context.Items.Update(item);
    }

    public void Delete(Item item)
    {
        _context.Items.Remove(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public int GetCount()
    {
        var totalItems = _context.Items.Count();

        return totalItems;
    }
}
