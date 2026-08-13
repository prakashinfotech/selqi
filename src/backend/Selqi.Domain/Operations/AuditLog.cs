namespace Selqi.Domain.Operations;

public sealed class AuditLog
{
    public Guid Id { get; set; }
    public Guid ActorUserId { get; set; }
    public string ActorEmail { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
