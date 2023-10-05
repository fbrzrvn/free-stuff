using FluentValidation;
using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Items.Application.Shared;

public static class ItemMappers
{
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
        foreach (var kvp in ItemConditionMapping.ConditionMapping)
        {
            if (kvp.Value.Equals(conditionString, StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }
        }

        throw new ValidationException($"Invalid item condition: {conditionString}");
    }

    private static string MapItemConditionToString(this ItemCondition condition)
    {
        if (ItemConditionMapping.ConditionMapping.TryGetValue(condition, out var conditionString))
        {
            return conditionString;
        }

        throw new ValidationException($"Invalid item condition: {condition}");
    }
}
