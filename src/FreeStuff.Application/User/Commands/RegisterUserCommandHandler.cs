using ErrorOr;
using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.User.Commands;

internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<UserEntity>>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository) { _userRepository = userRepository; }

    public async Task<ErrorOr<UserEntity>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var user = UserEntity.Create(command.FullName, command.Username, command.Email, command.Password);

        _userRepository.Register(user);

        return user;
    }
}
