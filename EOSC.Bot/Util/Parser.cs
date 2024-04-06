using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EOSC.Bot.Util
{
    public class Parser
    {
        public static string GetChannelId(string jsonString)
        {
            string pattern = "\"channel_id\":\"(\\d+)\"";
            Match match = Regex.Match(jsonString, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetContent(string jsonString)
        {
            string pattern = "\"content\":\"([^\"]*)\"";
            Match match = Regex.Match(jsonString, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetUsername(string jsonString)
        {
            string pattern = "\"username\":\"([^\"]*)\"";
            Match match = Regex.Match(jsonString, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public static (string, string) ParseCommand(string commandString)
        {
            string[] parts = commandString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && parts[0].StartsWith('!'))
            {
                string command = parts[0].Substring(1);
                string[] arguments = new string[parts.Length - 1];
                Array.Copy(parts, 1, arguments, 0, arguments.Length);
                string argument = string.Join(" ", arguments);
                return (command, argument);
            }
            else
            {
                return (string.Empty, string.Empty);
            }
        }
    }
}
