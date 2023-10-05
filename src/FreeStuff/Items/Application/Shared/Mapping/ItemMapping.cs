using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Domain;

namespace FreeStuff.Items.Application.Shared.Mapping;

public static class ItemMapping
{
    public static ItemDto MapToItemDto(this Item item)
    {
        var itemDto = new ItemDto(
            item.Id.Value,
            item.Title,
            item.Description,
            item.Condition.MapItemConditionToString(),
            item.UserId.Value
        );

        return itemDto;
    }

    public static ItemsDto MapToItemsDto(this IEnumerable<Item> items, int page, int limit, int totalResults)
    {
        var itemsDto = new ItemsDto
        {
            Data         = items.Select(MapToItemDto),
            Page         = page,
            Limit        = limit,
            TotalResults = totalResults
        };

        return itemsDto;
    }
}
