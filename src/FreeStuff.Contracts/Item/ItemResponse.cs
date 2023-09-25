using FreeStuff.Domain.Item;

namespace FreeStuff.Contracts.Item;

public record ItemResponse
(
    Guid   Id,
    string Title,
    string Description,
    string Condition,
    string UserId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime
);
