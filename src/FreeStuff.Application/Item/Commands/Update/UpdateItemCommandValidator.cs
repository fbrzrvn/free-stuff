using FluentValidation;

namespace FreeStuff.Application.Item.Commands.Update;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator() { RuleFor(x => x.Id).NotEmpty(); }
}
