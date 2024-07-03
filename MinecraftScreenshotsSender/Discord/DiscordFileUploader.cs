using Discord;
using Discord.WebSocket;
using MinecraftScreenshotsSender.Screenpresso;

namespace MinecraftScreenshotsSender.Discord;

public class DiscordFileUploader
{
    private static readonly string? BotToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
    private static readonly ulong ServerId = Convert.ToUInt64(Environment.GetEnvironmentVariable("SERVER_ID"));
    private static readonly ulong ChannelId = Convert.ToUInt64(Environment.GetEnvironmentVariable("CHANNEL_ID"));
    
    private readonly DiscordSocketClient _client = new();
    private readonly ScreenshotProvider _screenshotProvider = new();

    public DiscordFileUploader()
    {
        // var config = new DiscordSocketConfig
        // {
        //     GatewayIntents = GatewayIntents.All
        // };
        // _client = new DiscordSocketClient(config);
    }

    public async Task Upload()
    {
        _client.Log += Log;
        _client.Ready += ReadyAsync;
        
        await _client.LoginAsync(TokenType.Bot, BotToken);
        await _client.StartAsync();
        
        await Task.Delay(-1);
    }

    private Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.ToString());
        return Task.CompletedTask;
    }
    
    private Task ReadyAsync()
    {
        var path = _screenshotProvider.Provide();
        if (path != null)
        {
            var caption = "Made by " + Environment.UserName + ": " + DateTime.Now;
            _client.GetGuild(ServerId)
                .GetTextChannel(ChannelId)
                .SendFileAsync(path, caption);    
        }
        
        return Task.CompletedTask;
    }

}