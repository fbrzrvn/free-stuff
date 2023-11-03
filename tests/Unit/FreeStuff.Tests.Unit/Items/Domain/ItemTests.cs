using FluentAssertions;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Constants;
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
        actual.Category.Name.Should().Be(Constants.Item.CategoryName);
        actual.Condition.Should().Be(Constants.Item.Condition.MapExactStringToItemCondition());
        actual.UserId.Should().Be(UserId.Create(Constants.Item.UserId));
        actual.CreatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
        actual.UpdatedDateTime.Should().BeSameDateAs(actual.CreatedDateTime);
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
            Constants.Item.EditedCondition.MapExactStringToItemCondition()
        );

        // Assert
        actual.Title.Should().Be(Constants.Item.EditedTitle);
        actual.Description.Should().Be(Constants.Item.EditedDescription);
        actual.Condition.Should().Be(Constants.Item.EditedCondition.MapExactStringToItemCondition());
        actual.UpdatedDateTime.Should().NotBe(actual.CreatedDateTime);
        actual.UpdatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
    }
}
