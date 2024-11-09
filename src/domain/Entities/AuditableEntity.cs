namespace Shopzy.Domain.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedUtc { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? DeletedUtc { get; set; }
    public string? DeletedBy { get; set; }
}
