using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Bot.Commands;
using EOSC.Bot.Config;
using EOSC.Bot.Interfaces.Classes;


namespace EOSC.Bot.Classes;

public class DiscordBot(DiscordToken token) : IDiscordBot
{
    private const string GatewayUrl = "wss://gateway.discord.gg/?v=9&encoding=json";

    private readonly Dictionary<string, BaseCommand> _commands = new();
    private readonly string _discordToken = token.Token;
    private ClientWebSocket _socket = null!;

    private int? _currentSequence;
    private BaseMessageTyped<IdentifyParams> _defaultParams = null!;


    // We need to make this buffer large enough to hold the entire message. 
    private const int ReceiveBufferSize = 1024 * 16;
    private const int HeartbeatInterval = 30 * 1000;
    private CancellationTokenSource _cts;

    public async Task StartAsync()
    {
        _defaultParams = CreateDefaultParams(_discordToken);
        LoadCommands();
        var cts = new CancellationTokenSource();
        _cts = new CancellationTokenSource();
        _socket = new ClientWebSocket();
        while (true)
        {
            try
            {
                await _socket.ConnectAsync(new Uri(GatewayUrl), cts.Token);
                SendWsMessageAsyncType(_defaultParams);
                _ = Task.Run(async () => await ReceiveMessages(cts.Token), cts.Token);
                _ = Task.Run(HeartbeatLoop, _cts.Token);
                await Task.Delay(Timeout.Infinite, cts.Token);
            }
            catch (ReconnectError)
            {
                Console.WriteLine("Restarting...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Maybe should exit the program here.
            }
            finally
            {
                if (_socket.State is WebSocketState.Open or WebSocketState.Connecting)
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                _socket.Dispose();
            }
        }
    }

    private async Task HeartbeatLoop()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                SendHeartbeat();
                await Task.Delay(HeartbeatInterval, _cts.Token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Heartbeat error: {ex.Message}");
            }
        }
    }

    private void SendHeartbeat()
    {
        string heartbeatMessage = "{\"op\": 1, \"d\": null}";

        byte[] bytes = Encoding.UTF8.GetBytes(heartbeatMessage);
        _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cts.Token);
    }



    private void LoadCommands()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var commandTypes = types.Where(t => t.GetCustomAttribute<CommandAttribute>() != null);
        foreach (var type in commandTypes)
        {
            var attribute = type.GetCustomAttribute<CommandAttribute>();
            var commandInstance = Activator.CreateInstance(type) as BaseCommand;
            _commands.Add(attribute!.CommandName, commandInstance!);
        }

        foreach (var kvp in _commands) Console.WriteLine($"Command: {kvp.Key}, Type: {kvp.Value.GetType().Name}");
    }


    private async Task ReceiveMessages(CancellationToken cancellationToken)
    {
        try
        {
            var buffer = new byte[ReceiveBufferSize];
            while (_socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result =
                    await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine(message);
                        HandleTextFromMessage(message, cancellationToken);
                        break;
                    case WebSocketMessageType.Close:
                        await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
                        break;
                    case WebSocketMessageType.Binary:
                    default:
                        continue;
                }
            }
        }
        catch (WebSocketException e)
        {
            // Handle WebSocket exceptions
            Console.WriteLine(e);
        }
        catch (OperationCanceledException e)
        {
            // Handle operation cancellation
            Console.WriteLine(e);
        }
        catch (ReconnectError)
        {
            // Rethrow here to handle this in the StartAsync method
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async void HandleTextFromMessage(string message, CancellationToken cancellationToken)
    {
        var baseMessage = JsonSerializer.Deserialize<BaseMessage>(message);
        // Unable to deserialize message this shouldn't happen.
        if (baseMessage == null) return;
        _currentSequence = baseMessage.SequenceNumber ?? _currentSequence;
        switch (baseMessage.OpCode)
        {
            case GatewayOpCode.Hello:
                HandleHeartbeat(baseMessage);
                break;
            case GatewayOpCode.Dispatch:
                HandleGatewayEvent(baseMessage);
                break;
            case GatewayOpCode.HeartbeatAck:
                Console.WriteLine("HeartbeatAck");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("We are Received HeartbeatAck");
                Console.WriteLine("Server HeartbeatAck");
                Console.WriteLine(baseMessage);
                Console.WriteLine("-----------------------------------");
                break;
            case GatewayOpCode.Heartbeat:
                Console.WriteLine("Heartbeat");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("We are trying to Heartbeat");
                Console.WriteLine("Server requested a Heartbeat");
                Console.WriteLine(baseMessage);
                Console.WriteLine("-----------------------------------");
                break;
            case GatewayOpCode.Reconnect:
                // Basically reconnect means that something seriously went wrong and the websocket has been invalidated. 
                // Seems like this only happens after a few hours. Not too sure if we are missing an event or if Heartbeat is not working.
                // Discord.Net seems to throw an exception when this happens. So I am assuming it's a Heartbeat issue.
                // For now we will just restart the entire DiscordBot and run it again.
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
                break;
            case GatewayOpCode.InvalidSession:
                Console.WriteLine("InvalidSession");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("We InvalidSession Received");
                Console.WriteLine("Re-identifying");
                SendWsMessageAsyncType(_defaultParams);
                Console.WriteLine("-----------------------------------");
                break;

            case GatewayOpCode.Identify:
            default:
                Console.WriteLine("Other Event");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(baseMessage);
                Console.WriteLine("-----------------------------------");
                break;
        }
    }

    private void HandleGatewayEvent(BaseMessage baseMessage)
    {
        if (baseMessage.EventName is "RESUMED") Console.WriteLine("We are trying to Resume maybe do something here?");

        // We dont super care about any message other that create.
        if (baseMessage.EventName is not "MESSAGE_CREATE") return;
        // We know deserialize will work here as this is how the docs defined it.
        var message = baseMessage.Data?.Deserialize<Message>()!;
        var messageContent = message.Content;

        // Only check commands starting with !
        if (!messageContent.StartsWith('!')) return;
        var parts = messageContent.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0][1..];
        var args = parts.Skip(1).ToList();


        if (_commands.TryGetValue(command, out var commandHandler))
            commandHandler.SendCommand(_discordToken, args, message);

        Console.WriteLine($"Command: {command}");
        Console.WriteLine("Args:");
        foreach (var s in args) Console.WriteLine(s);

        Console.WriteLine($"Username: {message.Author.Username}");
        Console.WriteLine($"MessageContent: {messageContent}");
        Console.WriteLine($"ChannelId: {message.ChannelId}");
    }

    private void SendWsMessageAsync(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        var segment = new ArraySegment<byte>(bytes);
        _socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private void SendWsMessageAsyncType<T>(T data)
    {
        var json = JsonSerializer.Serialize(data);
        if (json.Length > 4096)
        {
            // TODO: ERROR
        }

        Console.WriteLine("Sending message via socket: " + json);
        SendWsMessageAsync(json);
    }

    private void HandleHeartbeat(BaseMessage baseMessage)
    {
        var heartBeat = baseMessage.Data?.Deserialize<HelloEvent>();
        if (heartBeat?.HeartbeatInterval != null)
            _ = new Timer(_ =>
                {
                    var heartbeatJson = new BaseMessage
                    {
                        OpCode = GatewayOpCode.Heartbeat,
                        SequenceNumber = _currentSequence
                    };
                    SendWsMessageAsyncType(heartbeatJson);
                }, _socket, heartBeat.HeartbeatInterval,
                heartBeat.HeartbeatInterval);
    }

    private static BaseMessageTyped<IdentifyParams> CreateDefaultParams(string token)
    {
        return new BaseMessageTyped<IdentifyParams>
        {
            OpCode = GatewayOpCode.Identify,
            SequenceNumber = null,
            EventName = "IDENTIFY",
            Data = new IdentifyParams
            {
                Token = token,
                Properties = new Dictionary<string, string>
                {
                    { "$os", "linux" },
                    { "$device", "EOSC" },
                    { "$browser", "EOSC" }
                },
                ShardingParams = [0, 1],
                Intents = 3276799,
                Presence = new PresenceParams
                {
                    Activities =
                    [
                        new ActivityParams
                        {
                            Name = "hard to get ⭐",
                            Type = 0
                        }
                    ]
                }
            }
        };
    }
}