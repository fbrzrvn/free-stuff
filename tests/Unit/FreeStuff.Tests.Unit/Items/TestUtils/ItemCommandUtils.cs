using FreeStuff.Items.Application.Create;
using FreeStuff.Items.Application.Update;
using FreeStuff.Tests.Unit.TestUtils.Constants;

namespace FreeStuff.Tests.Unit.Items.TestUtils;

public static class ItemCommandUtils
{
    public static CreateItemCommand NewCreateItemCommand()
    {
        var command = new CreateItemCommand(
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Item.Condition,
            Constants.Item.UserId
        );

        return command;
    }

    public static CreateItemCommand NewCreateItemCommand
    (
        string? title,
        string? description,
        string? condition,
        Guid?   userId
    )
    {
        var command = new CreateItemCommand(
            title       ?? Constants.Item.Title,
            description ?? Constants.Item.Description,
            condition   ?? Constants.Item.Condition,
            userId      ?? Constants.Item.UserId
        );

        return command;
    }

    public static UpdateItemCommand NewUpdateItemCommand()
    {
        var command = new UpdateItemCommand(
            Guid.NewGuid(),
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Item.EditedCondition,
            Constants.Item.UserId
        );

        return command;
    }

    public static UpdateItemCommand NewUpdateItemCommand
    (
        Guid?   itemId,
        string? title,
        string? description,
        string? condition,
        Guid?   userId
    )
    {
        var command = new UpdateItemCommand(
            itemId      ?? Guid.NewGuid(),
            title       ?? Constants.Item.EditedTitle,
            description ?? Constants.Item.EditedDescription,
            condition   ?? Constants.Item.EditedCondition,
            userId      ?? Constants.Item.UserId
        );

        return command;
    }
}
