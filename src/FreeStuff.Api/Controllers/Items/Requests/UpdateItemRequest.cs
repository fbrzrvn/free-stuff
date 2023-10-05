namespace FreeStuff.Api.Controllers.Items.Requests;

public record UpdateItemRequest
(
    string Title,
    string Description,
    string Condition,
    Guid   UserId
);
