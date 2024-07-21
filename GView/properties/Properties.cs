using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GView.properties;

public class Properties : INotifyPropertyChanged
{
    private string _gameTitle;
    private ulong _serverId;
    private ulong _channelId;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public string GameTitle
    {
        get => _gameTitle;
        set
        {
            _gameTitle = value;
            OnPropertyChanged();
        }
    }

    public ulong ServerId
    {
        get => _serverId;
        set
        {
            _serverId = Convert.ToUInt64(value);
            OnPropertyChanged();
        }
    }

    public ulong ChannelId 
    {
        get => _channelId;
        set
        {
            _channelId = Convert.ToUInt64(value);
            OnPropertyChanged();
        }
    }

    // for ENVS
    // public Properties(string gameTitle, ulong serverId, ulong channelId)
    // {
    //     _gameTitle = gameTitle;
    //     _serverId = serverId;
    //     _channelId = channelId;
    // }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}