using ErrorOr;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Update;

public record UpdateItemCommand
(
    Guid   Id,
    string Title,
    string Description,
    string Condition,
    Guid   UserId
) : IRequest<ErrorOr<ItemEntity>>;
