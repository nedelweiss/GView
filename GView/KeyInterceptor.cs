using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using GView.FileUtils;
using GView.properties;
using GView.Screenpresso;

namespace GView;

public class KeyInterceptor
{
    public delegate void PrintScreenHandler(string pathToFile);

    public event PrintScreenHandler OnPrintScreen;

    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private readonly IntPtr _hookId;
    private readonly HostedProcessFinder _hostedProcessFinder;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private readonly Properties _properties;

    private LowLevelKeyboardProc _proc; // don't convert it into a local variable cuz of this problem https://stackoverflow.com/questions/6193711/call-has-been-made-on-garbage-collected-delegate-in-c

    public KeyInterceptor(Properties properties)
    {
        _properties = properties;
        
        _fileSystemWatcher = new FileSystemWatcher(new ScreenshotsPathProvider().GetPath());
        _fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes 
                                          | NotifyFilters.CreationTime
                                          | NotifyFilters.DirectoryName
                                          | NotifyFilters.FileName
                                          | NotifyFilters.LastAccess
                                          | NotifyFilters.LastWrite
                                          | NotifyFilters.Security
                                          | NotifyFilters.Size;
        _fileSystemWatcher.IncludeSubdirectories = false;
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
        string mainWindowTitle = hostedProcess.MainWindowTitle;

        // TODO: get rid of second part in condition
        // TODO: compare GAME_TITLE and title of the current active process (mainWindowTitle) as case insensitive
        
        var gameTitle = _properties.GameTitle;
        if (mainWindowTitle.Contains(gameTitle)) 
        {
            if (nCode >= 0 && wParam == WmKeydown)
            {
                int keyCode = Marshal.ReadInt32(lParam);
                Keys code = (Keys)keyCode;
                if (code == Keys.PrintScreen)
                {
                    _fileSystemWatcher.Created += OnCreate;
                }
            }
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private void OnCreate(object sender, FileSystemEventArgs eventArgs)
    {
        var pathToFile = eventArgs.FullPath;
        while (FileUsageChecker.IsFileLocked(new FileInfo(pathToFile)))
        {
            Task.Delay(10);
        }

        // TODO: check if event has no subscribers
        OnPrintScreen(pathToFile); 

        Console.WriteLine($"Created: {pathToFile}");

        _fileSystemWatcher.Created -= OnCreate;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
}