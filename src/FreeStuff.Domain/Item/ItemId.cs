using FreeStuff.Domain.common.Models;

namespace FreeStuff.Domain.Item;

public sealed class ItemId : ValueObject
{
    private ItemId(Guid value) { Value = value; }

    public ItemId() { }

    public Guid Value { get; }

    public static ItemId CreateUnique() { return new ItemId(Guid.NewGuid()); }

    public static ItemId Create(Guid id) { return new ItemId(id); }

    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
}