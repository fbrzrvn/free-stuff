using FreeStuff.Api.Controllers.Items.Requests;
using FreeStuff.Items.Application.Update;
using Mapster;

namespace FreeStuff.Api.Mapping.Items;

public class ItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid Id, UpdateItemRequest Request), UpdateItemCommand>()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest, src => src.Request);
    }
}
