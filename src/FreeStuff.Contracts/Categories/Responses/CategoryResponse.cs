namespace FreeStuff.Contracts.Categories.Responses;

public class CategoryResponse
{
    public Guid       Id      { get; }
    public string     Name    { get; }
    public List<Guid> ItemIds { get; }

    public CategoryResponse(Guid id, string name, List<Guid> itemIds)
    {
        Id      = id;
        Name    = name;
        ItemIds = itemIds;
    }
}
