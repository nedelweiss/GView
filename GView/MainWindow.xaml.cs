using System.Windows;
using GView.Discord;
using GView.properties;

namespace GView;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly KeyInterceptor _keyInterceptor;
    private Properties _properties;

    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine("Minecraft Screenshot Sender has been started...");
        
        var discordFileUploader = new DiscordFileUploader(_properties);

        _keyInterceptor = new KeyInterceptor(_properties);
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler((pathToFile) =>
        {
            discordFileUploader.Upload(pathToFile);
            Console.WriteLine("Perfect! Time: " + DateTime.Now);
            
            // TODO: check if selected area is inside Minecraft Window coordinates 
        });
    }

    void SetCredential_Click(object sender, RoutedEventArgs e)
    {
        string gameTitle = GameTitle.Text;
        ulong serverId = Convert.ToUInt64(ServerId.Text);
        ulong channelId = Convert.ToUInt64(ChannelId.Text);

        _properties = new Properties(gameTitle, serverId, channelId);
    }
}