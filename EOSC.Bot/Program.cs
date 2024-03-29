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
            //IDiscordBot Bot = new DiscordBot();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IDiscordBot, DiscordBot>()
                .BuildServiceProvider();
            //await Bot.StartAsync();
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