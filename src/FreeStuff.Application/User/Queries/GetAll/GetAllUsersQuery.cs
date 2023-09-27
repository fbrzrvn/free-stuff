using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.User.Queries.GetAll;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserEntity>>;
