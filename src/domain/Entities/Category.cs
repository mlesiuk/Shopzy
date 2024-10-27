namespace Shopzy.Domain.Entities;

public class Category : AuditableEntity
{
    private Category()
    {
    }

    public static Category Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new Category
        {
            Name = name
        };
    }

    public string Name { get; set; } = string.Empty;

    public Category? Parent { get; set; }
}
