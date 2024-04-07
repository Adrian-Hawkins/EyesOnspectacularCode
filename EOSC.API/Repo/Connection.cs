using EOSC.API.Config;
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
            if (_connection == null)
            {
                IConfiguration config = new ConfigurationBuilder()
                 .AddUserSecrets<DB>()
                 .AddEnvironmentVariables()
                 .Build();
                ConnectionString = config["database:connection"] ?? throw new Exception("Please provide database connection string");
                _connection = new NpgsqlConnection(ConnectionString);
                _connection.Open();
            }
            else if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
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
