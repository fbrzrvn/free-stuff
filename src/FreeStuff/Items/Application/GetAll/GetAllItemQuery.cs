using ErrorOr;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public record GetAllItemQuery(int Page, int Limit) : IRequest<ErrorOr<ItemsDto>>;
