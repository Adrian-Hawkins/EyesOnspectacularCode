using Npgsql;

namespace EOSC.Common.SharedRepository;

public class BaseRepo
{
    private readonly string _connectionString;

    public BaseRepo(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new NotSupportedException("You must specify a connection string.");
        _connectionString = connectionString;
    }

    public async Task<NpgsqlConnection> GetConnectionAsync()
    {
        NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<T> GetConnectionAsync<T>(Func<NpgsqlConnection, Task<T>> action)
    {
        await using var conn = await GetConnectionAsync();
        return await action(conn);
    }

    public async Task GetConnectionTransactionAsync(Func<NpgsqlConnection, NpgsqlTransaction, Task> action)
    {
        await using NpgsqlConnection conn = await GetConnectionAsync();
        await using NpgsqlTransaction transaction = await conn.BeginTransactionAsync();

        try
        {
            await action(conn, transaction);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}