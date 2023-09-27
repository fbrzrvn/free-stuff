using ErrorOr;
using MediatR;

namespace FreeStuff.Application.Item.Commands.Delete;

public record DeleteItemCommand
(
    Guid Id
) : IRequest<ErrorOr<bool>>;
