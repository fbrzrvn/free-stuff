using FreeStuff.Domain.common.Models;

namespace FreeStuff.Domain.User;

public sealed class UserId : ValueObject
{
    private UserId(Guid value) { Value = value; }

    public UserId() { }

    public Guid Value { get; }

    public static UserId CreateUnique() { return new UserId(Guid.NewGuid()); }

    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
}