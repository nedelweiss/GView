using Discord;
using Discord.WebSocket;
using GView.properties;
using GView.Utils;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;

namespace GView.Discord;

public class DiscordFileUploader
{
    private const string DateTimeFormat = "dddd, d MMMM yyyy hh:mm tt";
    private const string DsErrorLogsFileName = "discord_error_logs.txt";
    private const string DsLogsFileName = "discord_logs.txt";
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

        try
        {
            _client.GetGuild(_properties.ServerId)
                .GetTextChannel(_properties.ChannelId)
                .SendFileAsync(path, caption)
                .WaitAndUnwrapException();
        }
        catch (Exception exception)
        {
            FileUtils.AppendToFile(DsErrorLogsFileName, $"{exception.Message}\n{exception.StackTrace}\n");
            throw;
        }
    }

    private async Task Upload()
    {
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot, BotToken);
        await _client.StartAsync();
    }

    private Task Log(LogMessage logMessage)
    {
        FileUtils.AppendToFile(DsLogsFileName, $"{logMessage}\n");
        Console.WriteLine(logMessage.ToString());
        return Task.CompletedTask;
    }
}