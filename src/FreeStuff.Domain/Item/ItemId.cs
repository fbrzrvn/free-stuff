using FreeStuff.Domain.common.Models;

namespace FreeStuff.Domain.Item;

public sealed class ItemId : ValueObject
{
    public Guid Value { get; }

    private ItemId(Guid value)
    {
        Value = value;
    }

    public ItemId()
    {
    }

    public static ItemId CreateUnique()
    {
        return new ItemId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
