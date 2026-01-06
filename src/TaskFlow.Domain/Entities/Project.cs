using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Project : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
