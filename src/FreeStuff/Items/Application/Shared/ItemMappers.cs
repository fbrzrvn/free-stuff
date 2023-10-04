using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Items.Application.Shared;

public static class ItemMappers
{
    private static readonly Dictionary<ItemCondition, string> ConditionMapping = new()
    {
        { ItemCondition.New, "New" },
        { ItemCondition.FairCondition, "Fair condition" },
        { ItemCondition.GoodCondition, "Good condition" },
        { ItemCondition.AsGoodAsNew, "As good as new" },
        { ItemCondition.HasGivenItAll, "Has given it all" }
    };

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

    public static ItemCondition MapStringToItemCondition(this string conditionString)
    {
        foreach (var kvp in ConditionMapping)
        {
            if (kvp.Value.Equals(conditionString, StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }
        }

        throw new ArgumentException("Invalid condition string");
    }

    private static string MapItemConditionToString(this ItemCondition condition)
    {
        if (ConditionMapping.TryGetValue(condition, out var conditionString))
        {
            return conditionString;
        }

        throw new ArgumentException("Invalid ItemCondition value");
    }
}
