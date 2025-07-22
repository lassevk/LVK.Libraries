using System.Data;

using Dapper;

namespace LVK.Data;

public static class DbConnectionExtensions
{
    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The number of rows affected.</returns>
    public static Task<int> ExecuteAsync(this IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
        => cnn.ExecuteAsync(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
}