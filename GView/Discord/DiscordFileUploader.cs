﻿using Discord;
using Discord.WebSocket;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;

namespace GView.Discord;

public class DiscordFileUploader
{
    private static readonly string? BotToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
    private static readonly ulong ServerId = Convert.ToUInt64(Environment.GetEnvironmentVariable("SERVER_ID"));
    private static readonly ulong ChannelId = Convert.ToUInt64(Environment.GetEnvironmentVariable("CHANNEL_ID"));
    private const string TimeFormat = "dddd, d MMMM yyyy hh:mm tt";
    
    private readonly DiscordSocketClient _client = new();
    
    public DiscordFileUploader()
    {
        AsyncContext.Run(Upload);
    }

    public void Upload(string? path)
    {
        if (path == null) return;
        var caption = "Made by " + Environment.UserName + ": " + DateTime.Now.ToString(TimeFormat);
        Console.WriteLine(caption);
        _client.GetGuild(ServerId)
            .GetTextChannel(ChannelId)
            .SendFileAsync(path, caption)
            .WaitAndUnwrapException();
    }

    private async Task Upload()
    {
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot, BotToken);
        await _client.StartAsync();
    }

    private Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.ToString());
        return Task.CompletedTask;
    }
}