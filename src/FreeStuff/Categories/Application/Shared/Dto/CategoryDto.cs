using FreeStuff.Items.Application.Shared.Dto;

namespace FreeStuff.Categories.Application.Shared.Dto;

public class CategoryDto
{
    public Guid          Id    { get; }
    public string        Name  { get; }
    public List<ItemDto> Items { get; }

    public CategoryDto(Guid id, string name, List<ItemDto> items)
    {
        Id    = id;
        Name  = name;
        Items = items;
    }
}
