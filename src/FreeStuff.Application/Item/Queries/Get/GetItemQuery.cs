using ErrorOr;
using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Queries.Get;

public record GetItemQuery(Guid Id) : IRequest<ErrorOr<ItemEntity>>;
