namespace FreeStuff.Api.Controllers.Items.Requests;

public record GetAllItemsRequest
(
    int Page  = 1,
    int Limit = 10
);
