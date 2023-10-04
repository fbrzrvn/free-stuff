using ErrorOr;
using FreeStuff.Items.Application.Shared;
using MediatR;

namespace FreeStuff.Items.Application.Create;

public record CreateItemCommand
(
    string Title,
    string Description,
    string Condition,
    Guid   UserId
) : IRequest<ErrorOr<ItemDto>>;
