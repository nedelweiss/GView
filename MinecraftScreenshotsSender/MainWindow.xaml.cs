using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        _keyInterceptor = new KeyInterceptor();
        _keyInterceptor.OnPrintScreen += new KeyInterceptor.PrintScreenHandler(() =>
        {
            // TODO1: find place where screenpresso stores screenshots
            // TODO2: get last screenshot
            // TODO3: send to TG
            // TODO4*: check if selected area is inside Minecraft Window coordinates 
        });
    }
}