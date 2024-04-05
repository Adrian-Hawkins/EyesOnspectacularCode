using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using EOSC.Bot.Config;
using Newtonsoft.Json.Linq;

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

        public async Task StartAsync()
        {
            string gatewayUrl = "wss://gateway.discord.gg/?v=9&encoding=json";
            CancellationTokenSource cts = new CancellationTokenSource();


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
            try
            {
                byte[] buffer = new byte[1024];
                while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine(message);

                        //await ProcessMessageAsync(" ", socket);
                        //handle msg here
                        await ProcessMessageAsync("fdsgfd", socket);
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

        static async Task ProcessMessageAsync(string message, ClientWebSocket socket)
        {
            try
            {
                // Parse the received JSON data to extract channel_id
                //JObject jsonData = JObject.Parse(message);
                //string channelId = jsonData["d"]["channel_id"].ToString();
                //string channelId = "1093446156681490534";
                //string url = "https://discordapp.com/api/v9/channels/1093446156681490534/messages";
                string url = "https://discordapp.com/api/v9/channels/1223250423268249743/messages";

                // Bot token
                string botToken = "";

                // JSON payload for the POST request (example message content)
                string jsonPayload = "{\"content\": \"@everyone The bot fucking works without libraries, thank god!\"}";

                // Create a HttpClient instance
                using var httpClient = new HttpClient();

                // Set the authorization header with the bot token
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botToken}");

                // Create the HTTP content from JSON payload
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    // Make the POST request
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    // Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST request successful. Message sent.");
                    }
                    else
                    {
                        Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                // Send a message back to the channel
                //await SendMessageAsync(channelId, "Hello from the bot!", socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        static async Task SendMessageAsync(string channelId, string message, ClientWebSocket socket)
        {
            try
            {
                // Construct message payload

                string sendMessagePayload = $"{{\"t\": 4, \"op\": 1, \"d\": {{\"content\": \"{message}\", \"tts\":false, \"channel_id\": \"{channelId}\"}}}}";
                //string sendMessagePayload = "{\"t\": \"MESSAGE_CREATE\",\"op\": 0, \"d\": {\"content\": \"This is a message with components\", \"components\": [{\"type\": 1, \"components\": [{\"type\": 2, \"label\": \"Click me!\", \"style\": 1, \"custom_id\": \"click_one\"}]}]}}";
                byte[] payloadBytes = Encoding.UTF8.GetBytes(sendMessagePayload);

                // Send message to the channel
                await socket.SendAsync(new ArraySegment<byte>(payloadBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
        public static string GetGuildIdFromJson(string jsonString)
        {
            // Parse the JSON string into a JObject
            JObject jsonObject = JObject.Parse(jsonString);

            // Check if the "guilds" property exists and it is an array
            if (jsonObject["d"]?["guilds"] is JArray guildsArray)
            {
                // Check if the array is not empty and retrieve the first guild's ID
                if (guildsArray.Count > 0)
                {
                    string guildId = guildsArray[0]?["id"]?.ToString();
                    return guildId;
                }
                else
                {
                    Console.WriteLine("No guilds found in the JSON data.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("No 'guilds' property found in the JSON data.");
                return null;
            }
        }


    }
}