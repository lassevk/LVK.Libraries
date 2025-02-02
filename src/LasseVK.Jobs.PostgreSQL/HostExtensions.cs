using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs.PostgreSQL;

public static class HostExtensions
{
    public static async Task DropAllJobsAsync(this IHost host)
    {
        IDbContextFactory<PostgresDbContext> dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<PostgresDbContext>>();
        await using PostgresDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Jobs!.RemoveRange(dbContext.Jobs);
        await dbContext.SaveChangesAsync();
    }
}