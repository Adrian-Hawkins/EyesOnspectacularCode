namespace EOSC.Bot.Classes;

public enum GatewayOpCode : byte
{
    Dispatch = 0,
    Heartbeat = 1,
    Reconnect = 7,
    Hello = 10,
    HeartbeatAck = 11,
}