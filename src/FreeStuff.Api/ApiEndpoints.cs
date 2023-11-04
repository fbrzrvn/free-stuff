namespace FreeStuff.Api;

public static class ApiEndpoints
{
    public static class Items
    {
        public const string Base   = "api/items";
        public const string Create = Base;
        public const string Get    = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Search = $"{Base}/Search";
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Category
    {
        public const string Base   = "api/categories";
        public const string Create = Base;
        public const string GetAll = Base;
        public const string Update = Base;
    }
}
