namespace Shopzy.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private init; } = Guid.NewGuid();
}
