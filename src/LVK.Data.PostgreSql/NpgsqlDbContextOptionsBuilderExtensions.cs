using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace LVK.Data.PostgreSql;

public static class NpgsqlDbContextOptionsBuilderExtensions
{
    public static NpgsqlDbContextOptionsBuilder Configure(this NpgsqlDbContextOptionsBuilder builder) => builder.UseNodaTime();
}