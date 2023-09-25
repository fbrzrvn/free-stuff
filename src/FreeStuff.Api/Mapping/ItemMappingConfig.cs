using FreeStuff.Contracts.Item;
using FreeStuff.Domain.Item;
using Mapster;

namespace FreeStuff.Api.Mapping;

public class ItemMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ItemEntity, ItemResponse>()
              .Map(dest => dest.Id, src => src.Id.Value)
              .Map(dest => dest.UserId, src => src.UserId.Value);
    }
}
