using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public record GetAllItemQuery(int Page, int Limit) : IRequest<ErrorOr<List<ItemDto>>>;