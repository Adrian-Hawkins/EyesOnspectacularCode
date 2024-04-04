using System.Reflection;
using EOSC.Common.Constant;
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
            Hello.f();
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<IDiscordBot, DiscordBot>()
                .BuildServiceProvider();
            try
            {
                IDiscordBot bot = serviceProvider.GetRequiredService<IDiscordBot>();
                await bot.StartAsync(serviceProvider);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }
           
    }
}