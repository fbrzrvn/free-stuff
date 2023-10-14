using FluentAssertions;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Unit.TestUtils.Constants;
using FreeStuff.User.Domain.ValueObjects;

namespace FreeStuff.Tests.Unit.Items.Domain;

public class ItemTests
{
    [Fact]
    public void Create_ShouldCreateItem_WhenInputAreValid()
    {
        // Act
        var actual = ItemUtils.CreateItem();

        // Assert
        actual.Should().NotBeNull();
        actual.Title.Should().Be(Constants.Item.Title);
        actual.Description.Should().Be(Constants.Item.Description);
        actual.Condition.Should().Be(Constants.Item.Condition.MapStringToItemCondition());
        actual.UserId.Should().Be(UserId.Create(Constants.Item.UserId));
        actual.CreatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
        actual.UpdatedDateTime.Should().Be(actual.CreatedDateTime);
    }

    [Fact]
    public void Update_ShouldUpdateItem_WhenInputAreValid()
    {
        // Arrange
        var actual = ItemUtils.CreateItem();

        // Act
        actual.Update(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Item.EditedCondition.MapStringToItemCondition()
        );

        // Assert
        actual.Title.Should().Be(Constants.Item.EditedTitle);
        actual.Description.Should().Be(Constants.Item.EditedDescription);
        actual.Condition.Should().Be(Constants.Item.EditedCondition.MapStringToItemCondition());
        actual.UpdatedDateTime.Should().NotBe(actual.CreatedDateTime);
        actual.UpdatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
    }
}