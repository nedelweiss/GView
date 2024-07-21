using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GView.properties;

public class Properties : INotifyPropertyChanged
{
    private string _gameTitle;
    private ulong _serverId;
    private ulong _channelId;
    public event PropertyChangedEventHandler PropertyChanged;

    public string GameTitle
    {
        get { return _gameTitle; }
        set
        {
            _gameTitle = value;
            OnPropertyChanged(_gameTitle);
        }
    }

    public ulong ServerId
    {
        get { return _serverId; }
        set
        {
            _serverId = value;
            OnPropertyChanged();
        }
    }

    public ulong ChannelId
    {
        get { return _channelId; }
        set
        {
            _channelId = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string _gameTitle = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(this._gameTitle));
    }
}