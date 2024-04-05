using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EOSC.Bot.Classes;
using EOSC.Bot.Config;
using EOSC.Bot.Interfaces.Classes;

namespace EOSC.Bot
{
    internal class Program
    {
        public class DatabaseSettings
        {
            public string Host { get; set; }
        }

        static async Task Main(string[] args)
        {
            // Create a config that allows for user secrets(for dev) and environment variables(for prod).
            IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets<DatabaseSettings>()
                .AddEnvironmentVariables()
                .Build();

            // Get our discord token from the config.
            var discordToken = configuration.GetSection("Discord").Get<DiscordToken>();

            DiscordBot bot = new DiscordBot(discordToken);
            await bot.StartAsync();

            //// TODO: @Adrian: handle this error more gracefully if possible else remove this comment.
            //if (discordToken == null) throw new Exception("Missing Discord token");

            //// Setup DI with discord token and bot.
            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton(discordToken)
            //    .AddScoped<IDiscordBot, DiscordBot>()
            //    .BuildServiceProvider();
            
            //try
            //{
            //    IDiscordBot bot = serviceProvider.GetRequiredService<IDiscordBot>();
            //    await bot.StartAsync(serviceProvider);
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //    Environment.Exit(-1);
            //}
        }
    }
}