using System.ComponentModel.DataAnnotations;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class FileAttachment : AuditableEntity
{
    public Guid Id { get; set; }
    [MaxLength(255)]
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public long FileSize { get; set; }
    [MaxLength(100)]
    public string ContentType { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Task? Task { get; set; } = null!;
    public Guid UploadedByUserId { get; set; }
    public User? UploadedByUser { get; set; } // navigation property
}
