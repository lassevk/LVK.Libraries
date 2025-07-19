using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVK.Jobs.PostgreSQL;

[Table("jobs")]
internal class JobEntity
{
    [Key]
    [MaxLength(32)]
    [Column("id")]
    public required string Id { get; init; }

    [Column("group")]
    [MaxLength(256)]
    public required string? Group { get; init; }

    [Column("json", TypeName = "json")]
    public required string Json { get; set; }

    [Column("status")]
    public required JobStatus Status { get; set; }

    public List<JobEntity>? DependsOn { get; init; }

    public List<JobEntity>? DependsOnMe { get; init; }

    [Column("queued")]
    public DateTimeOffset WhenQueued { get; set; } = DateTimeOffset.UtcNow;

    [Column("started")]
    public DateTimeOffset? WhenStarted { get; set; }

    [Column("completed")]
    public DateTimeOffset? WhenCompleted { get; set; }
}