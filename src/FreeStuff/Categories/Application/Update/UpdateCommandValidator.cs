using FluentValidation;

namespace FreeStuff.Categories.Application.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.NewName).NotEmpty().NotEqual(request => request.Name);
    }
}
