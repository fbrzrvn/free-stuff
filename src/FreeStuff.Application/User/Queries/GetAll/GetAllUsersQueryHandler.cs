using FreeStuff.Domain.User;
using MediatR;

namespace FreeStuff.Application.User.Queries.GetAll;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserEntity>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserEntity>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _userRepository.GetAllAsync().ToList();
    }
}
