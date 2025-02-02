using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LasseVK.Jobs.PostgreSQL;

[Table("job_groups")]
public class JobGroupEntity
{
    [Key]
    [Column("name")]
    [MaxLength(256)]
    public required string Name { get; init; }

    [Column("max_jobs")]
    public int? MaxConcurrentJobs { get; set; }
}