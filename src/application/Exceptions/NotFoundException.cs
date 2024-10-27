namespace Shopzy.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"Entity {entity} does not exists")
    {
    }

    public NotFoundException(string entity, Guid id) : base($"Entity {entity} with id {id} does not exists")
    {        
    }
}
