using Microsoft.Extensions.Configuration;
using EOSC.Bot.Classes;
using EOSC.Bot.Config;
using EOSC.Bot.Interfaces.Classes;

namespace EOSC.Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Create a config that allows for user secrets(for dev) and environment variables(for prod).
            IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets<DiscordToken>()
                .AddEnvironmentVariables()
                .Build();

            var discordToken = configuration.GetSection("Discord").Get<DiscordToken>();

            //Microsoft.Extensions.DependencyInjection library :(
            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton(discordToken!)
            //    .AddScoped<IDiscordBot, DiscordBot>()
            //    .BuildServiceProvider();

            try
            {
                //IDiscordBot bot = serviceProvider.GetRequiredService<IDiscordBot>();
                IDiscordBot bot = new DiscordBot(discordToken!);
                await bot.StartAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }
    }
}