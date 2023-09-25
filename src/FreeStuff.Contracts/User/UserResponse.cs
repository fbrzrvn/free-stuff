namespace FreeStuff.Contracts.User;

public record UserResponse
(
    Guid     Id,
    string   FullName,
    string   Username,
    string   Email,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime
);