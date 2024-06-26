﻿using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSC.Bot.Commands
{
    [Command("JsonToYaml")]
    public class JsonToYaml : BaseCommand
    {
        public override async Task SendCommand(string discordToken, List<string> args, Message message)
        {
            string json = string.Join(" ", args).Replace("\"", "'");

            var request = new YamlToJsonRequest
            (
                json
            );
            _apiCallService.SetHeader(message.Author.GlobalName);
            _apiCallService.SetCustomHeader("bot", _botAuth.GetBotToken());

            var response =
            await _apiCallService.MakeApiCall<YamlToJsonRequest, YamlToJsonResponse>(
                "/api/JsonToYaml",
                request
            );

            await SendMessageAsync($"{response.JsonResult}", message, discordToken);
        }
    }
}
