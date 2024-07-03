using System.Windows;
using MinecraftScreenshotsSender.Discord;
using Nito.AsyncEx;

namespace MinecraftScreenshotsSender;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly KeyInterceptor _keyInterceptor;

    public MainWindow()
    {
        InitializeComponent();

        // ScreenshotProvider screenshotProvider = new ScreenshotProvider();
        // screenshotProvider.Provide();

        Console.WriteLine("Minecraft Screenshot Sender has been started...");
        
        _keyInterceptor = new KeyInterceptor();
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler(() =>
        {
            AsyncContext.Run(new DiscordFileUploader().Upload);
            Console.WriteLine("Perfect! Time: " + DateTime.Now);
            
            // TODO4*: check if selected area is inside Minecraft Window coordinates 
        });
    }
}