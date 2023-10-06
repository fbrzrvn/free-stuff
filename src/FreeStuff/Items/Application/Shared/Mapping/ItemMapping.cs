using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Domain;
using Mapster;

namespace FreeStuff.Items.Application.Shared.Mapping;

public class ItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Item, ItemDto>()
              .Map(dest => dest.Id, src => src.Id.Value)
              .Map(dest => dest.UserId, src => src.UserId.Value)
              .Map(dest => dest.Condition, src => src.Condition.MapItemConditionToString());

        config.NewConfig<(List<Item> Items, GetAllItemQuery Request, int TotalItems), ItemsDto>()
              .Map(dest => dest.Data, src => src.Items)
              .Map(dest => dest.Page, src => src.Request.Page)
              .Map(dest => dest.Limit, src => src.Request.Limit)
              .Map(dest => dest.TotalResults, src => src.TotalItems);
    }
}
