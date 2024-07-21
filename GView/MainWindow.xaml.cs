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

    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine("Minecraft Screenshot Sender has been started...");

        Properties properties = new Properties();
        var discordFileUploader = new DiscordFileUploader(properties);
        ServerId.DataContext = properties;
        ChannelId.DataContext = properties;

        _keyInterceptor = new KeyInterceptor();
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler((pathToFile) =>
        {
            discordFileUploader.Upload(pathToFile);
            Console.WriteLine("Perfect! Time: " + DateTime.Now);
            
            // TODO: check if selected area is inside Minecraft Window coordinates 
        });
    }
    
    
}