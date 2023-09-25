using ErrorOr;

namespace FreeStuff.Domain.common.Errors;

public static partial class Errors
{
    public static class Item
    {
        public static Error DuplicateTitleError => Error.Conflict("Item.DuplicateTitle", "Item title already exists");
    }
}
