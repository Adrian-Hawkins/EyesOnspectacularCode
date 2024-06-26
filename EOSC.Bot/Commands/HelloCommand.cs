﻿using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;

namespace EOSC.Bot.Commands;

[Command("hello")]
public class HelloCommand : BaseCommand
{
    public override async Task SendCommand(string discordToken, List<string> args, Message message)
    {
        await SendMessageAsync($"Hello <@{message.Author.Id}>", message, discordToken);
    }
}