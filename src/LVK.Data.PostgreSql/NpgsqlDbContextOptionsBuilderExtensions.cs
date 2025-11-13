using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace LVK.Data.PostgreSql;

public static class NpgsqlDbContextOptionsBuilderExtensions
{
    extension(NpgsqlDbContextOptionsBuilder builder)
    {
        public NpgsqlDbContextOptionsBuilder Configure() => builder.UseNodaTime();
    }
}