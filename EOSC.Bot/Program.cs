using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EOSC.Bot.Interfaces;
using EOSC.Bot.Classes;
namespace EOSC.Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IDiscordBot Bot = new DiscordBot();
            await Bot.StartAsync();
        }
           
    }
}