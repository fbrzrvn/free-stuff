using ErrorOr;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Create;

public record CreateItemCommand
(
    string Title,
    string Description,
    string Condition,
    Guid   UserId
) : IRequest<ErrorOr<ItemEntity>>;
