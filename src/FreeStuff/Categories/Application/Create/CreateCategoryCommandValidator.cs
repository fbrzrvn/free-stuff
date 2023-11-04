using FluentValidation;

namespace FreeStuff.Categories.Application.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
    }
}
