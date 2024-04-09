﻿using System.Text.RegularExpressions;
using EOSC.Bot.Attributes;
using EOSC.Bot.Classes.Deserializers;
using EOSC.Common.Requests;
using EOSC.Common.Responses;
using EOSC.Common.Services;

namespace EOSC.Bot.Commands;

[Command("datetime")]
public class DateTimeCommand : BaseCommand
{
    private readonly ApiCallService _apiCallService = new();

    public override async Task SendCommand(string botToken, List<string> args, Message message)
    {
        var content = message.Content;
        var pattern = @"\((.*?)\)";
        var matches = Regex.Matches(content, pattern);
        var parsedList = matches.Select(m => m.Groups[1].Value).ToList();
        if (parsedList.Count() != 3)
        {
            await SendMessageAsync("Usage: !datetime <(dateTimeString)> <(originalFormat)> <(desiredFormat)>",
                message.ChannelId, botToken);
            return;
        }

        var dateTimeString = parsedList[0];
        var originalFormat = parsedList[1];
        var desiredFormat = parsedList[2];
        var request = new DatetimeRequest
        (
            dateTimeString,
            originalFormat,
            desiredFormat
        );
        var response =
            await _apiCallService.MakeApiCall<DatetimeRequest, DateTimeConversionResponse>(
                "/api/Datetime",
                request
            );

        try
        {
            await SendMessageAsync(response.ConvertedTime, message.ChannelId, botToken);
        }
        catch (Exception ex)
        {
            await SendMessageAsync($"Error: {ex.Message}", message.ChannelId, botToken);
        }
    }
}