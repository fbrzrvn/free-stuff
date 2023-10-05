using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Items.Application.Shared.Mapping;

public static class ItemConditionMapping
{
    public static readonly Dictionary<ItemCondition, string> ConditionMapping = new()
    {
        { ItemCondition.New, "New" },
        { ItemCondition.FairCondition, "Fair condition" },
        { ItemCondition.GoodCondition, "Good condition" },
        { ItemCondition.AsGoodAsNew, "As good as new" },
        { ItemCondition.HasGivenItAll, "Has given it all" }
    };
}
