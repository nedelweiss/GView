using System.Windows;
using System.Windows.Controls;
using GView.Discord;
using GView.properties;
using Microsoft.Win32;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Threading.Timer;

namespace GView; // test

public partial class MainWindow : Window
{
    private const string RegistryAppDir = @"SOFTWARE\GView";
    private const string GameTitleRKey = "GameTitle";
    private const string ServerIdRKey = "ServerId";
    private const string ChannelIdRKey = "ChannelId";
    
    private readonly KeyInterceptor _keyInterceptor;
    private readonly Timer _timer;
    
    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine("Minecraft Screenshot Sender has been started...");
        
        _timer = new Timer(_ =>
        {
            // Dispatcher tells that this action has to be performed in the GUI thread
            Dispatcher.Invoke(() => {
                var gameTitle = GameTitle.GetBindingExpression(TextBox.TextProperty);
                gameTitle.UpdateSource();
                var serverId = ServerId.GetBindingExpression(TextBox.TextProperty);
                serverId.UpdateSource();
                var channelId = ChannelId.GetBindingExpression(TextBox.TextProperty);
                channelId.UpdateSource();
                
                WriteToRegistry();
            });
        });

        var properties = CreatePropertiesFromRegistry();
        ServerId.DataContext = properties;
        GameTitle.DataContext = properties;
        ChannelId.DataContext = properties;
        
        var discordFileUploader = new DiscordFileUploader(properties);

        _keyInterceptor = new KeyInterceptor(properties);
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler((pathToFile) =>
        {
            discordFileUploader.Upload(pathToFile);
            Console.WriteLine("Perfect! Time: " + DateTime.Now);
            
            // TODO: check if selected area is inside f.e. Minecraft Window coordinates 
        });
    }

    private void WriteToRegistry()
    {
        RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryAppDir);
        registryKey.SetValue(GameTitleRKey, GameTitle.Text);
        registryKey.SetValue(ServerIdRKey, ServerId.Text);
        registryKey.SetValue(ChannelIdRKey, ChannelId.Text);
        registryKey.Close();
    }

    private Properties CreatePropertiesFromRegistry()
    {
        var properties = new Properties();
        RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryAppDir);
        properties.GameTitle = registryKey.GetValue(GameTitleRKey)?.ToString() ?? string.Empty;
        
        var serverId = registryKey.GetValue(ServerIdRKey)?.ToString() ?? string.Empty;
        if (!string.IsNullOrEmpty(serverId))
        {
            properties.ServerId = ulong.Parse(serverId);
        }
        
        var channelId = registryKey.GetValue(ChannelIdRKey)?.ToString() ?? string.Empty;
        if (!string.IsNullOrEmpty(channelId))
        {
            properties.ChannelId = ulong.Parse(channelId);
        }
        
        registryKey.Close();

        return properties;
    }

    private void GameTitle_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        _timer.Change(TimeSpan.FromMilliseconds(500), Timeout.InfiniteTimeSpan);
    }
}