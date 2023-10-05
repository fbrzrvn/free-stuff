using FreeStuff.Api.Controllers.Items.Requests;
using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Application.Shared;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Application.Update;
using FreeStuff.Items.Domain;
using Mapster;

namespace FreeStuff.Api.Controllers.Items.Mapping;

public class ItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid Id, UpdateItemRequest Request), UpdateItemCommand>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest, src => src.Request);

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
