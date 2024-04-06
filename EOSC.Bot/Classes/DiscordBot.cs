using System.Net.WebSockets;
using System.Text;
using EOSC.Bot.Config;
using EOSC.Bot.Util;
using EOSC.Bot.Commands;

namespace EOSC.Bot.Classes
{
    public class DiscordBot
    {

        private string discordToken;
        private ClientWebSocket socket;

        #region ctor

        public DiscordBot(DiscordToken token)
        {
            discordToken = token.Token;
            socket = new ClientWebSocket();
        }

        #endregion
        public HelloCommand helloCommand = new HelloCommand();
        #region commands

        #endregion

        public async Task StartAsync()
        {
            string gatewayUrl = "wss://gateway.discord.gg/?v=9&encoding=json";
            CancellationTokenSource cts = new CancellationTokenSource();


            try
            {
                await socket.ConnectAsync(new Uri(gatewayUrl), cts.Token);
                Console.WriteLine("Connected to Discord WebSocket Gateway.");
                string identifyPayload = $"{{\"op\":2,\"d\":{{\"token\":\"{discordToken}\",\"intents\":513,\"properties\":{{\"$os\":\"linux\",\"$browser\":\"my_library\",\"$device\":\"my_library\"}}}}}}";
                byte[] payloadBytes = Encoding.UTF8.GetBytes(identifyPayload);
                await socket.SendAsync(new ArraySegment<byte>(payloadBytes), WebSocketMessageType.Text, true, cts.Token);
                _ = Task.Run(async () => await ReceiveMessages(socket, cts.Token, discordToken));
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

        public async Task ReceiveMessages(ClientWebSocket socket, CancellationToken cancellationToken, string _discordToken)
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        //Console.WriteLine(message);

                        string channelId = Parser.GetChannelId(message);
                        string username = Parser.GetUsername(message);
                        string content = Parser.GetContent(message);
                        if(channelId != null && username != "" && content != null)
                        {
                            Console.WriteLine($"{username} sent {content} in {channelId}");
                            (string command, string argument) = Parser.ParseCommand(content);
                            switch(command)
                            {
                                case "hello":
                                    helloCommand.SendCommand(content, channelId, _discordToken);
                                    break;
                                default:
                                    Console.WriteLine("passing");
                                    break;

                            }
                        }

                        //// Get the root element
                        //JsonElement root = doc.RootElement;

                        //// Get the 'd' object
                        //JsonElement dObject = root.GetProperty("d");

                        //// Check if 'channel_id' exists
                        //if (dObject.TryGetProperty("channel_id", out JsonElement channelIdElement))
                        //{
                        //    string channelId = channelIdElement.GetString();
                        //    Console.WriteLine($"Channel ID: {channelId}");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Channel ID not found.");
                        //}

                        //await ProcessMessageAsync(" ", socket);
                        //handle msg here
                        //await ProcessMessageAsync("fdsgfd", "d", _discordToken);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
                    }
                }
            }
            catch (WebSocketException)
            {
                // Handle WebSocket exceptions
            }
            catch (OperationCanceledException)
            {
                // Handle operation cancellation
            }
        }

    }
}