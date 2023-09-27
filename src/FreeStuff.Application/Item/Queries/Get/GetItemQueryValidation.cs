using FluentValidation;

namespace FreeStuff.Application.Item.Queries.Get;

public class GetItemQueryValidation : AbstractValidator<GetItemQuery>
{
    public GetItemQueryValidation() { RuleFor(x => x.Id).NotNull().NotEmpty(); }
}
