using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Comment : AuditableEntity
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
