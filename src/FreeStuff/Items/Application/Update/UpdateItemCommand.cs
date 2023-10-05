using ErrorOr;
using FreeStuff.Items.Application.Shared;
using MediatR;

namespace FreeStuff.Items.Application.Update;

public record UpdateItemCommand
(
    Guid   Id,
    string Title,
    string Description,
    string Condition,
    Guid   UserId
) : IRequest<ErrorOr<ItemDto>>;
