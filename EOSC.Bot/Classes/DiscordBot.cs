using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EOSC.Bot.Config;
using EOSC.Bot.Interfaces.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public DiscordBot(DiscordToken token)
        {
            discordToken = token.Token;
            DiscordSocketConfig config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            _client = new DiscordSocketClient(config);
            _client.MessageReceived += HandleCommandAsync;
            _commands = new CommandService();
        }

        #endregion

        public async Task StartAsync(ServiceProvider services)
        {
            string gatewayUrl = "wss://gateway.discord.gg/?v=9&encoding=json"; // Replace with your bot token
            CancellationTokenSource cts = new CancellationTokenSource();
            ClientWebSocket socket = new ClientWebSocket();

            try
            {
                await socket.ConnectAsync(new Uri(gatewayUrl), cts.Token);
                Console.WriteLine("Connected to Discord WebSocket Gateway.");

                // Send identify payload after connection
                string identifyPayload = $"{{\"op\":2,\"d\":{{\"token\":\"{discordToken}\",\"intents\":513,\"properties\":{{\"$os\":\"linux\",\"$browser\":\"my_library\",\"$device\":\"my_library\"}}}}}}";
                byte[] payloadBytes = Encoding.UTF8.GetBytes(identifyPayload);
                await socket.SendAsync(new ArraySegment<byte>(payloadBytes), WebSocketMessageType.Text, true, cts.Token);

                // Start listening for incoming messages
                _ = Task.Run(async () => await ReceiveMessages(socket, cts.Token));

                // Keep the application running until cancelled
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.Connecting)
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                socket.Dispose();
            }
        }

        static async Task ReceiveMessages(ClientWebSocket socket, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");
            }
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