using ErrorOr;
using FreeStuff.Items.Application.Shared;
using MediatR;

namespace FreeStuff.Items.Application.Get;

public record GetItemQuery(Guid Id) : IRequest<ErrorOr<ItemDto>>;
