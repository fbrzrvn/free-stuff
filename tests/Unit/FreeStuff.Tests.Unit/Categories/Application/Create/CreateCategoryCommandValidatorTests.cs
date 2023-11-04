using FluentValidation.TestHelper;
using FreeStuff.Categories.Application.Create;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Tests.Unit.Categories.Application.Create;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    [Fact]
    public void CreateCategoryCommandValidator_ShouldNotThrowAValidationError_WhenCommandIsValid()
    {
        // Arrange
        var createCategoryCommand = new CreateCategoryCommand(Constants.Category.Name);

        // Act
        var actual = _validator.TestValidate(createCategoryCommand);

        // Assert
        actual.ShouldNotHaveValidationErrorFor(request => request.Name);
    }

    [Fact]
    public void CreateCategoryCommandValidator_ShouldThrowAValidationError_WhenCommandIsInvalid()
    {
        // Arrange
        var createCategoryCommand = new CreateCategoryCommand(string.Empty);

        // Act
        var actual = _validator.TestValidate(createCategoryCommand);

        // Assert
        actual.ShouldHaveValidationErrorFor(request => request.Name);
    }
}
