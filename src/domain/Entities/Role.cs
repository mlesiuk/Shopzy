namespace Shopzy.Domain.Entities;

public class Role : AuditableEntity
{
    public Role()
    {
    }

    public static Role Create(string name, string createdBy)
    {
        return new Role
        {
            Name = name,
            CreatedBy = createdBy
        };
    }

    public string Name { get; set; } = string.Empty;
    public virtual List<User> Users { get; set; } = new();
}
