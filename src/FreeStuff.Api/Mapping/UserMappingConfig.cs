using FreeStuff.Contracts.User;
using FreeStuff.Domain.User;
using Mapster;

namespace FreeStuff.Api.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserEntity, UserResponse>().Map(dest => dest.Id, src => src.Id.Value);
    }
}
