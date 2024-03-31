using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EOSC.Bot.Interfaces.Commands;
using System.Net.NetworkInformation;
using EOSC.Bot.Interfaces.Classes;
using System;

namespace EOSC.Bot.Classes
{
    public class DiscordBot : IDiscordBot
    {
        private readonly IConfiguration _configuration;

        private readonly DiscordSocketClient _client;
        private string discordToken;

        private readonly CommandService _commands;
        private ServiceProvider? _serviceProvider;

        #region ctor
        public DiscordBot()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
            discordToken = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord token");

            _client = new DiscordSocketClient();
            _client.MessageReceived += HandleCommandAsync;

            _commands = new CommandService();
        }
        #endregion

        public async Task StartAsync(ServiceProvider services)
        {
            _serviceProvider = services;
            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _client.StartAsync();
            _client.Log += LogFuncAsync;
            await Task.Delay(-1);

            //Log all events happening to bot
            async Task LogFuncAsync(LogMessage message) =>
                await Console.Out.WriteLineAsync(message.ToString());
                
        }

        public async Task StopAsync()
        {
            if (_client != null)
            {
                await _client.LogoutAsync();
                await _client.StopAsync();
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Ignore messages from bots
            if (arg is not SocketUserMessage message || message.Author.IsBot)
            {
                return;
            }
            Console.WriteLine(message.Content);

            // Check if the message starts with !
            int position = 0;
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
            {
                // Execute the command if it exists in the ServiceCollection
                await _commands.ExecuteAsync(new SocketCommandContext(_client, message), position, _serviceProvider);
                return;
            }
        }
    }
}
