using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class Task : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskFlow.Domain.Enums.TaskStatus Status { get; set; } = TaskFlow.Domain.Enums.TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<FileAttachment> FileAttachments { get; set; } = new List<FileAttachment>();
}
