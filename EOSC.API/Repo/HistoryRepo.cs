using EOSC.Common.SharedRepository;
using Npgsql;

namespace EOSC.API.Repo;

public class HistoryRepo() : IHistoryRepo
{
    
    public static void UpdateHistory(string userName, string toolName, string input, string output)
    {
        string sql = @"
            DO $$
            DECLARE
                user_id INT;
                tool_id INT;
            BEGIN
                SELECT UserId INTO user_id FROM Users WHERE UserName = @UserName;

                IF user_id IS NULL THEN
                    INSERT INTO Users (UserName) VALUES (@UserName) RETURNING UserId INTO user_id;
                END IF;

                SELECT ToolId INTO tool_id FROM Tool WHERE ToolName = @ToolName;

                IF tool_id IS NULL THEN
                    INSERT INTO Tool (ToolName) VALUES (@ToolName) RETURNING ToolId INTO tool_id;
                END IF;

                INSERT INTO History (Input, Output, ActionDate, UserId, ToolId)
                VALUES (@Input, @Output, CURRENT_DATE, user_id, tool_id);
            END $$;";

        using (var connection = Connection.GetConnection())
        {
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@ToolName", toolName);
                cmd.Parameters.AddWithValue("@Input", input);
                cmd.Parameters.AddWithValue("@Output", output);

                cmd.ExecuteNonQuery();
            }
        }
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
                        history.Add($"Input: {input}, Output: {output}, ActionDate: {actionDate}, ToolName: {toolName}");
                    }
                }
            }
        }

        return history;
    }


    
}