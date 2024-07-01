using System.Windows;
using MinecraftScreenshotsSender.Screenpresso;

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

        var screenshotsPathProvider = new ScreenshotsPathProvider();

        _keyInterceptor = new KeyInterceptor();
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler(() =>
        {
            var path = screenshotsPathProvider.GetPath();
            // TODO2: get last screenshot
            // TODO3: send to TG
            // TODO4*: check if selected area is inside Minecraft Window coordinates 
        });
    }
}