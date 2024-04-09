namespace EOSC.Bot.Classes;

public enum GatewayOpCode : byte
{
    Dispatch = 0,
    Heartbeat = 1,
    Identify = 2,
    Reconnect = 7,
    InvalidSession = 9,
    Hello = 10,
    HeartbeatAck = 11,
}