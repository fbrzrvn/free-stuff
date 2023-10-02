using FreeStuff.Domain.common.Models;
using FreeStuff.Domain.User;

namespace FreeStuff.Domain.Item;

public sealed class ItemEntity : AggregateRoot<ItemId>
{
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

    private ItemEntity() { }

    public string   Title           { get; set; }
    public string   Description     { get; set; }
    public string   Condition       { get; set; }
    public UserId   UserId          { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }

    public static ItemEntity Create(string title, string description, string condition, UserId userId)
    {
        var item = new ItemEntity(
            ItemId.CreateUnique(),
            title,
            description,
            condition,
            userId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return item;
    }
}