namespace FreeStuff.Contracts.Item;

public record ItemRequest
(
    string Title,
    string Description,
    string Condition,
    Guid   UserId
);
