using FreeStuff.Domain.User;

namespace FreeStuff.Infrastructure.User;

public class UserRepository : IUserRepository
{
    private static readonly List<UserEntity> _users = new();

    public void Register(UserEntity user) { _users.Add(user); }

    public UserEntity? GetUser(Guid id) { return _users.SingleOrDefault(user => user.Id.Value == id); }

    public UserEntity? GetItemByEmail(string email) { return _users.SingleOrDefault(user => user.Email == email); }
}
