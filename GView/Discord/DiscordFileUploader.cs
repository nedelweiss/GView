using System.ComponentModel;
using System.Runtime.CompilerServices;
using Discord;
using Discord.WebSocket;
using GView.properties;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;

namespace GView.Discord;

public class DiscordFileUploader
{
    private const string DateTimeFormat = "dddd, d MMMM yyyy hh:mm tt";
    private static readonly string? BotToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
    
    private readonly DiscordSocketClient _client = new();
    private readonly Properties _properties;
    
    public DiscordFileUploader(Properties properties)
    {
        _properties = properties;
        AsyncContext.Run(Upload);
    }

    public void Upload(string? path)
    {
        if (path == null) return;
        var caption = "Made by " + Environment.UserName + ": " + DateTime.Now.ToString(DateTimeFormat);

        var serverId = _properties.ServerId;
        var channelId = _properties.ChannelId;
        Console.WriteLine(serverId);
        Console.WriteLine(channelId);
        
        _client.GetGuild(serverId)
            .GetTextChannel(channelId)
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