using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EOSC.Bot.Interfaces;
using EOSC.Bot.Commands;
using EOSC.Bot.Interfaces.Commands;
using System.Net.NetworkInformation;

namespace EOSC.Bot.Classes
{
    public class DiscordBot : IDiscordBot
    {
        private readonly IConfiguration _configuration;

        private readonly DiscordSocketClient _client;
        //private const string token = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord token");
        //string discordToken = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord token");
        private string discordToken;

        #region Commands

        private readonly IEchoCommand _echoCommand = new EchoCommand();

        #endregion

        #region ctor
        public DiscordBot()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
            discordToken = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord token");
            _client = new DiscordSocketClient();
            this._client.MessageReceived += _echoCommand.MessageHandler;
        }
        #endregion

        public async Task StartAsync(/*ServiceProvider services*/)
        {
            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();
            _client.Log += LogFuncAsync;
            await Task.Delay(-1);
            
            //Log all events happening to bot
            async Task LogFuncAsync(LogMessage message) =>
                await Console.Out.WriteLineAsync(message.ToString());
                
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
