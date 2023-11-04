using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Tests.Unit.Categories.Domain;

public class CategoryTests
{
    [Fact]
    public void Create_ShouldCreateCategory_WhenInputAreValid()
    {
        // Act
        var actual = Category.Create(Constants.Category.Name);

        // Assert
        actual.Should().NotBeNull();
        actual.Name.Should().Be(Constants.Category.Name);
    }

    [Fact]
    public void Update_ShouldUpdateCategory_WhenInputAreValid()
    {
        // Arrange
        var actual = Category.Create(Constants.Category.Name);

        // Act
        actual.Update(Constants.Category.EditedName);

        // Assert
        actual.Name.Should().Be(Constants.Category.EditedName);
    }
}
