using FreeStuff.Domain.Item;
using ErrorOr;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Create;

public record CreateItemCommand
(
    string Title,
    string Description,
    string Condition
) : IRequest<ErrorOr<ItemEntity>>;
