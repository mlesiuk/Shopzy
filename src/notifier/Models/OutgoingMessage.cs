namespace Shopzy.Notifier.Models;

public class OutgoingMessage
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public string? Content { get; set; }
}
