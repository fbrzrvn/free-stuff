using FluentValidation;

namespace FreeStuff.Application.Item.Commands.Delete;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator() { RuleFor(x => x.Id).NotEmpty(); }
}
