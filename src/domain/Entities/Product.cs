using Shopzy.Domain.Enums;
using Shopzy.Domain.ValueObjects;

namespace Shopzy.Domain.Entities;

public sealed class Product : AuditableEntity
{
    private Product()
    {        
    }

    public static Product Create(string name, Money price, string? description)
    {
        return new Product
        {
            Name = name,
            Price = price,
            Description = description
        };
    }

    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; } = new(0, Currency.PLN);
    public string? Description { get; private set; } = string.Empty;
    public Category? Category { get; private set; }
}
