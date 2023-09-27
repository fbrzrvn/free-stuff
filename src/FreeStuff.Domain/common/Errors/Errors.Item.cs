using ErrorOr;

namespace FreeStuff.Domain.common.Errors;

public static partial class Errors
{
    public static class Item
    {
        public static Error DuplicateTitleError => Error.Conflict("Item.DuplicateTitle", "Item title already exists");

        public static Error NotFoundError(Guid id)
        {
            return Error.NotFound("Item.NotFoundError", $"Item not found with id: {id}");
        }
    }
}
