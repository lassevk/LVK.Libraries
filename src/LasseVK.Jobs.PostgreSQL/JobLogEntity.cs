using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace LasseVK.Jobs.PostgreSQL;

[Index(nameof(JobId), Name = "logs_job_id")]
internal class JobLogEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [Column("job")]
    [MaxLength(32)]
    public required string JobId { get; init; }

    public JobEntity? Job { get; init; }

    [Column("when")]
    public required DateTimeOffset WhenLogged { get; init; }

    [Column("log")]
    [MaxLength(4096)]
    public required string Line { get; init; }
}