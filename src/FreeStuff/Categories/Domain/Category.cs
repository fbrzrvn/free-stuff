using FreeStuff.Categories.Domain.ValueObjects;
using FreeStuff.Items.Domain;
using FreeStuff.Shared.Domain;

namespace FreeStuff.Categories.Domain;

public class Category : Entity<CategoryId>
{
    public string     Name       { get; private set; }
    public List<Item> Items      { get; private set; }

    public Category(CategoryId id, string name) : base(id)
    {
        Name  = name;
        Items = new List<Item>();
    }

    public static Category Create(string name)
    {
        var category = new Category(CategoryId.CreateUnique(), name);

        return category;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }
}
