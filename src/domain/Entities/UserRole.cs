namespace Shopzy.Domain.Entities;

public class UserRole : AuditableEntity
{
    private UserRole()
    {
    }

    public static UserRole Create(Guid userId, Guid roleId, string createdBy)
    {
        return new UserRole
        {
            RoleId = roleId,
            UserId = userId,
            CreatedBy = createdBy
        };
    }
    
    public Role? Role { get; set; }

    public Guid RoleId { get; set; }

    public User? User { get; set; }

    public Guid UserId { get; set; }
}
