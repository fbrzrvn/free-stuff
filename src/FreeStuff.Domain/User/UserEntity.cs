using FreeStuff.Domain.common.Models;

namespace FreeStuff.Domain.User;

public sealed class UserEntity : AggregateRoot<UserId>
{
    private UserEntity
    (
        UserId   id,
        string   fullName,
        string   username,
        string   email,
        string   password,
        DateTime createdDateTime,
        DateTime updatedDateTime
    ) : base(id)
    {
        FullName        = fullName;
        Username        = username;
        Email           = email;
        Password        = password;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public string   FullName        { get; }
    public string   Username        { get; }
    public string   Email           { get; }
    public string   Password        { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    public static UserEntity Create(string fullName, string username, string email, string password)
    {
        return new UserEntity(
            UserId.CreateUnique(),
            fullName,
            username,
            email,
            password,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }
}
