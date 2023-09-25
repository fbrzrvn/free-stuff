using FreeStuff.Domain.Item;
using MediatR;

namespace FreeStuff.Application.Item.Queries.GetAll;

public record GetAllItemsQuery() : IRequest<IEnumerable<ItemEntity>>;
