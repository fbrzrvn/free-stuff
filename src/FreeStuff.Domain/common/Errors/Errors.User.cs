using ErrorOr;

namespace FreeStuff.Domain.common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmailError =>
            Error.Conflict("User.DuplicateEmail", "User email already exists");
    }
}
