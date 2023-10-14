using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain;
using FreeStuff.Tests.Unit.TestUtils.Constants;

namespace FreeStuff.Tests.Unit.Items.TestUtils;

public static class ItemUtils
{
    public static Item CreateItem()
    {
        var item = Item.Create(
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Item.Condition.MapStringToItemCondition(),
            Constants.Item.UserId
        );

        return item;
    }

    public static ItemDto CreateItemDto()
    {
        var itemDto = new ItemDto(
            Guid.NewGuid(),
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Item.Condition,
            Constants.Item.UserId
        );

        return itemDto;
    }

    public static ItemDto MapItemToDto(this Item item)
    {
        var itemDto = new ItemDto(
            Guid.Parse(item.Id.Value.ToString()),
            item.Title,
            item.Description,
            item.Condition.MapItemConditionToString(),
            Guid.Parse(item.UserId.Value.ToString())
        );

        return itemDto;
    }
}