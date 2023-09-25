using ErrorOr;

namespace FreeStuff.Domain.common.Errors;

public static partial class Errors
{
    public static class Item
    {
        public static Error DuplicateTitle => Error.Conflict("Item.DuplicateTitle", "Item title already exists");
    }
}
