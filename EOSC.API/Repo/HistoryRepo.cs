using EOSC.Common.SharedRepository;
using Microsoft.Extensions.Primitives;
using Npgsql;

namespace EOSC.API.Repo;

public class HistoryRepo : IHistoryRepo
{
    public static async Task UpdateHistory(string userName, string toolName, string input, string output)
    {
        await using var connection = Connection.GetConnection();
        await using var cmd =
            new NpgsqlCommand("SELECT InsertOrUpdateHistory(@UserName, @ToolName, @Input, @Output)", connection);
        cmd.Parameters.AddWithValue("@UserName", userName);
        cmd.Parameters.AddWithValue("@ToolName", toolName);
        cmd.Parameters.AddWithValue("@Input", input);
        cmd.Parameters.AddWithValue("@Output", output);
        cmd.ExecuteNonQuery();
    }

    public static List<string> GetHistory(string username)
    {
        string sql = @"
            SELECT Input, Output, ActionDate, ToolName
            FROM History
            JOIN Users ON History.UserId = Users.UserId
            JOIN Tool ON History.ToolId = Tool.ToolId
            WHERE Users.UserName = @Username;";
        List<string> history = new List<string>();

        using (var connection = Connection.GetConnection())
        {
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string input = reader.GetString(0);
                        string output = reader.GetString(1);
                        DateTime actionDate = reader.GetDateTime(2);
                        string toolName = reader.GetString(3);
                        history.Add(
                            $"Input: {input}, Output: {output}, ActionDate: {actionDate}, ToolName: {toolName}");
                    }
                }
            }
        }

        return history;
    }
}