namespace FreeStuff.Domain.User;

public interface IUserRepository
{
    void Register(UserEntity user);

    UserEntity? GetUser(Guid id);

    UserEntity? GetItemByEmail(string email);
}
