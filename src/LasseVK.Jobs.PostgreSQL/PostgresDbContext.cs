using Microsoft.EntityFrameworkCore;

namespace LasseVK.Jobs.PostgreSQL;

internal class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options)
    {

    }

    public DbSet<JobEntity>? Jobs { get; set; }

    public DbSet<JobGroupEntity>? JobGroups { get; set; }
}