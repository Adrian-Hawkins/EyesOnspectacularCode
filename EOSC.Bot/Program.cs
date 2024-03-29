using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EOSC.Bot.Classes;
using EOSC.Bot.Interfaces.Classes;
namespace EOSC.Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IDiscordBot Bot = new DiscordBot();
            await Bot.StartAsync();
        }
           
    }
}