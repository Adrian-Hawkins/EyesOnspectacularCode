using EOSC.API.Config;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EOSC.API.Repo
{
    public class Connection
    {
        private static string ConnectionString;
        private static NpgsqlConnection _connection;

        private Connection() {}

        public static NpgsqlConnection GetConnection()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<DB>()
                .AddEnvironmentVariables()
                .Build();
            ConnectionString = config["database:connection"] ?? throw new Exception("Please provide database connection string");

            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        public static void CloseConnection()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
}
