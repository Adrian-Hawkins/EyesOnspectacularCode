﻿using Discord.WebSocket;
using EOSC.Bot.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSC.Bot.Commands
{
    public class EchoCommand : IEchoCommand
    {
        public async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            await ReplyAsync(message, "Hello C#");
        }

        private async Task ReplyAsync(SocketMessage message, string v)
        {
            await message.Channel.SendMessageAsync(v);
        }
    }
}