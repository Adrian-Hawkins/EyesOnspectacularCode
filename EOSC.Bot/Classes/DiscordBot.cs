﻿using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using EOSC.Bot.Config;
using EOSC.Bot.Commands;
using EOSC.Bot.Classes.Deserializers;
using System.Reflection;
using EOSC.Bot.Attributes;
using EOSC.Bot.Interfaces.Classes;

namespace EOSC.Bot.Classes;

public partial class DiscordBot(DiscordToken token) : IDiscordBot
{
    private const string GatewayUrl = "wss://gateway.discord.gg/?v=9&encoding=json";
    private readonly string _discordToken = token.Token;
    private readonly ClientWebSocket _socket = new();

    private readonly Dictionary<string, BaseCommand> _commands = new();

    private readonly TimeSpan _heartbeatInterval = TimeSpan.FromSeconds(30);

    private int? _currentSequence;


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

        foreach (var kvp in _commands)
        {
            Console.WriteLine($"Command: {kvp.Key}, Type: {kvp.Value.GetType().Name}");
        }
    }


    public async Task StartAsync()
    {
        var cts = new CancellationTokenSource();
        LoadCommands();
        try
        {
            await _socket.ConnectAsync(new Uri(GatewayUrl), cts.Token);
            var identifyPayload = $$"""
                                    {
                                      "op": 2,
                                      "d": {
                                        "token": "{{_discordToken}}",
                                        "properties": {
                                          "$os": "linux",
                                          "$browser": "disco",
                                          "$device": "disco"
                                        },
                                        "compress": true,
                                        "large_threshold": 250,
                                        "shard": [0, 1],
                                        "presence": {
                                          "activities": [{
                                            "name": "hard to get ⭐",
                                            "type": 0
                                          }],
                                          "afk": false
                                        },
                                        "intents": 3276799
                                      }
                                    }
                                    """;

            SendWsMessageAsync(identifyPayload);
            _ = Task.Run(async () => await ReceiveMessages(cts.Token));
            /*_heartbeatTimer = new Timer(_ =>
            {
                var json = @"{""op"": 1, ""d"": null}";
                SendWsMessageAsync(json);
            }, null, TimeSpan.Zero, _heartbeatInterval);*/

            await Task.Delay(Timeout.Infinite, cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            if (_socket.State == WebSocketState.Open || _socket.State == WebSocketState.Connecting)
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            _socket.Dispose();
        }
    }


    public async Task ReceiveMessages(CancellationToken cancellationToken)
    {
        try
        {
            byte[] buffer = new byte[1024 * 4];
            while (_socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result =
                    await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        HandleTextFromMessage(message);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void HandleTextFromMessage(string message)
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
                Console.WriteLine("Reconnect");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("We are trying to reconnect");
                Console.WriteLine("Server requested a reconnect");
                Console.WriteLine(baseMessage);
                Console.WriteLine("-----------------------------------");
                break;
        }
    }

    private void HandleGatewayEvent(BaseMessage baseMessage)
    {
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
        {
            commandHandler.SendCommand(_discordToken, args, message);
        }

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

    //TODO: Remove if unused later
    private void SendWsMessageAsyncType<T>(T data)
    {
        var json = JsonSerializer.Serialize(data);
        SendWsMessageAsync(json);
    }

    private void HandleHeartbeat(BaseMessage baseMessage)
    {
        var heartBeat = baseMessage.Data?.Deserialize<HelloEvent>();
        if (heartBeat?.HeartbeatInterval != null)
            _ = new Timer(_ =>
                {
                    // Need this Ternary as null serialises to literally nothing and we need it to be null  
                    var heartbeatJson = _currentSequence == null
                        ? """{"op":1, "d": null}"""
                        : $$"""{"op":1, "d": {{_currentSequence}}}""";
                    Console.WriteLine(heartbeatJson);
                    SendWsMessageAsync(heartbeatJson);
                }, _socket, heartBeat.HeartbeatInterval,
                heartBeat.HeartbeatInterval);
    }
}