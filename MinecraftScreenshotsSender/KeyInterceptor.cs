using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MinecraftScreenshotsSender.Screenpresso;

namespace MinecraftScreenshotsSender;

public class KeyInterceptor
{
    public delegate void PrintScreenHandler(string pathToFile);

    public event PrintScreenHandler OnPrintScreen;
    
    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private readonly IntPtr _hookId;
    private readonly HostedProcessFinder _hostedProcessFinder;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private LowLevelKeyboardProc _proc; // don't convert it into a local variable cuz of this problem https://stackoverflow.com/questions/6193711/call-has-been-made-on-garbage-collected-delegate-in-c

    public KeyInterceptor()
    {
        _fileSystemWatcher = new FileSystemWatcher(new ScreenshotsPathProvider().GetPath());
        
        _fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;
        
        _fileSystemWatcher.IncludeSubdirectories = true;
        _fileSystemWatcher.EnableRaisingEvents = true;
        
        _proc = HookCallback;
        _hostedProcessFinder = new HostedProcessFinder();
        _hookId = SetHook(_proc);
    }
    
    ~KeyInterceptor()
    {
        UnhookWindowsHookEx(_hookId);
    }
    
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        return SetWindowsHookEx(WhKeyboardLl, proc, IntPtr.Zero, 0);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        Process hostedProcess = _hostedProcessFinder.Find();
        string processTestMainWindowTitle = hostedProcess.MainWindowTitle;

        // TODO: get rid of second part in condition
        if (processTestMainWindowTitle.Contains("Minecraft") && !processTestMainWindowTitle.Contains("MinecraftScreenshotsSender"))
        {
            if (nCode >= 0 && wParam == WmKeydown) 
            {
                int keyCode = Marshal.ReadInt32(lParam);
                Keys code = (Keys) keyCode;
                if (code == Keys.PrintScreen)
                {
                    // TODO: check if event has no subscribers
                    
                    _fileSystemWatcher.Created += OnCreated;
                }
            }
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }
    
    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var pathToFile = e.FullPath;
        OnPrintScreen(pathToFile);
        string value = $"Created: {pathToFile}";
        
        Console.WriteLine(value);
        
        _fileSystemWatcher.Created -= OnCreated;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
}