using FluentValidation;

namespace FreeStuff.Application.Item.Commands.Create;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Condition).NotEmpty();
    }
}
