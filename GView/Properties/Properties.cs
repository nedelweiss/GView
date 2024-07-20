namespace GView.properties;

public class Properties
{
    private readonly string _gameTitle;
    private readonly ulong _serverId;
    private readonly ulong _channelId;

    public Properties(string gameTitle, ulong serverId, ulong channelId)
    {
        _gameTitle = gameTitle;
        _serverId = serverId;
        _channelId = channelId;
    }

    public string GetGameTitle()
    {
        return _gameTitle;
    }

    public ulong GetServerId()
    {
        return _serverId;
    }

    public ulong GetChannelId()
    {
        return _channelId;
    }
}