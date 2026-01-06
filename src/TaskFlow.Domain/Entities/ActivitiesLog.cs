using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class ActivitiesLog : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Action { get; set; } = null!; // Create, Update, Delete
    [MaxLength(50)]
    public string EntityType { get; set; } = null!; // Task, Project
    public Guid EntityId { get; set; }
    public string? Details { get; set; } // JSON hoặc văn bản
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
