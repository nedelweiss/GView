using System.Windows;
using System.Windows.Controls;
using GView.Discord;
using GView.properties;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Threading.Timer;

namespace GView;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly KeyInterceptor _keyInterceptor;
    private readonly Timer _timer;
    
    public MainWindow()
    {
        InitializeComponent();
        Console.WriteLine("Minecraft Screenshot Sender has been started...");

        var properties = new Properties();
        ServerId.DataContext = properties;
        GameTitle.DataContext = properties;
        ChannelId.DataContext = properties;
        
        var discordFileUploader = new DiscordFileUploader(properties);

        _keyInterceptor = new KeyInterceptor(properties);
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler((pathToFile) =>
        {
            discordFileUploader.Upload(pathToFile);
            Console.WriteLine("Perfect! Time: " + DateTime.Now);
            
            // TODO: check if selected area is inside Minecraft Window coordinates 
        });
        
        _timer = new Timer(_ =>
        {
            // Dispatcher tells that this action has to be performed in the GUI thread
            Dispatcher.Invoke(()=>{
                var expression = GameTitle.GetBindingExpression(TextBox.TextProperty);
                expression.UpdateSource();
            });
        });
    }

    private void GameTitle_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        _timer.Change(TimeSpan.FromMilliseconds(500), Timeout.InfiniteTimeSpan);
    }
}