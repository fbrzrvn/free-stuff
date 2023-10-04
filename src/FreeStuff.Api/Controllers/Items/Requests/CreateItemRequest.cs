namespace FreeStuff.Api.Controllers.Items.Requests;

public record CreateItemRequest
(
    string Title,
    string Description,
    string Condition,
    Guid   UserId
);
