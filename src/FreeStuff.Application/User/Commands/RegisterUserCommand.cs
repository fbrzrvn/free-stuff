using ErrorOr;
using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.User.Commands;

public record RegisterUserCommand
(
    string FullName,
    string Username,
    string Email,
    string Password
) : IRequest<ErrorOr<UserEntity>>;
