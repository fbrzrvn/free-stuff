namespace FreeStuff.Contracts.Categories.Responses;

public class CategoryResponse
{
    public Guid   Id   { get; }
    public string Name { get; }

    public CategoryResponse(Guid id, string name)
    {
        Id   = id;
        Name = name;
    }
}
