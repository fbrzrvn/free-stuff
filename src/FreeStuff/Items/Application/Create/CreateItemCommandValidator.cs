using FluentValidation;
using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Items.Application.Create;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(request => request.Title).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Description).NotEmpty().MaximumLength(500);
        RuleFor(request => request.Condition).IsEnumName(typeof(ItemCondition)).WithMessage("Invalid condition value");
        RuleFor(request => request.UserId).NotEmpty();
    }
}
