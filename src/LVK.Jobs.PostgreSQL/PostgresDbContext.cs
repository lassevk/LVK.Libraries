using Microsoft.EntityFrameworkCore;

namespace LVK.Jobs.PostgreSQL;

internal class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobEntity>()
           .HasMany(p => p.DependsOn)
           .WithMany(p => p.DependsOnMe)
           .UsingEntity<Dictionary<string, object>>("dependencies",
                l => l.HasOne<JobEntity>().WithMany().HasForeignKey("dependencyId").OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne<JobEntity>().WithMany().HasForeignKey("dependentId").OnDelete(DeleteBehavior.Cascade));
    }

    public DbSet<JobEntity>? Jobs { get; set; }

    public DbSet<JobLogEntity>? JobLogs { get; set; }
}