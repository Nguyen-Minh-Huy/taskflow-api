using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Team : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
