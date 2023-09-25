using FreeStuff.Domain.common.Models;
using FreeStuff.Domain.User;

namespace FreeStuff.Domain.Item;

public sealed class ItemEntity : AggregateRoot<ItemId>
{
    public string   Title           { get; }
    public string   Description     { get; }
    public string   Condition       { get; }
    public UserId   UserId          { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    private ItemEntity
    (
        ItemId   id,
        string   title,
        string   description,
        string   condition,
        UserId   userId,
        DateTime createdDateTime,
        DateTime updatedDateTime
    ) : base(id)
    {
        Title           = title;
        Description     = description;
        Condition       = condition;
        UserId          = userId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static ItemEntity Create(string title, string description, string condition, UserId userId)
    {
        return new ItemEntity(
            ItemId.CreateUnique(),
            title,
            description,
            condition,
            userId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }
}
