using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(255)]
    public string Email { get; set; } = null!;
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = null!;
    [MaxLength(100)]
    public string DisplayName { get; set; } = null!;
    [MaxLength(50)]
    public string SystemRole { get; set; } = "User";

    public ICollection<Team> OwnedTeams { get; set; } = new List<Team>();
    public ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
    public ICollection<Task> TasksAssigned { get; set; } = new List<Task>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<FileAttachment> FileAttachments { get; set; } = new List<FileAttachment>();
    public ICollection<ActivitiesLog> Activities { get; set; } = new List<ActivitiesLog>();
}
