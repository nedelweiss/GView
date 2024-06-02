using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MinecraftScreenshotsSender;

public class KeyInterceptor
{
    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private readonly IntPtr _hookId;
    private readonly FindHostedProcess _findHostedProcess;

    public KeyInterceptor()
    {
        _findHostedProcess = new FindHostedProcess();
        _hookId = SetHook(HookCallback);
    }
    
    ~KeyInterceptor()
    {
        UnhookWindowsHookEx(_hookId);
    }
    
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        return SetWindowsHookEx(WhKeyboardLl, proc, IntPtr.Zero, 0);
    }
    
    // private static IntPtr SetHook(LowLevelKeyboardProc proc)
    // {
    //     var openWindowProvider = new OpenWindowProvider();
    //     var openWindows = openWindowProvider.GetOpenWindows();
    //     
    //     foreach (var (windowHandler, windowTitle) in openWindows)
    //     {
    //         if (windowTitle.Contains("Minecraft") && !windowTitle.Contains("MinecraftScreenshotsSender"))
    //         {
    //             IntPtr pid = IntPtr.Zero;
    //             uint windowThreadProcessId = GetWindowThreadProcessId(windowHandler, out pid);
    //             Process p = Process.GetProcessById((int) pid);
    //             var storeModuleFileName = p.MainModule.FileName;
    //
    //             var realProcess = _findHostedProcess.GetRealProcess(p);
    //             string mainModuleFileName = realProcess.MainModule.FileName;
    //             
    //             return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(mainModuleFileName), Convert.ToUInt32(realProcess.Id));
    //         }
    //     }
    //
    //     return IntPtr.Zero;
    // }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        Process processTest = _findHostedProcess.GetProcessTest();
        string processTestMainWindowTitle = processTest.MainWindowTitle;

        if (processTestMainWindowTitle.Contains("Minecraft") && !processTestMainWindowTitle.Contains("MinecraftScreenshotsSender"))
        {
            if (nCode >= 0 && wParam == WmKeydown) 
            {
                int keyCode = Marshal.ReadInt32(lParam);
                Keys code = (Keys)keyCode;
                
                Console.WriteLine(code); // TODO: send screenshot to Telegram
                // TODO1: find place where screenpresso stores screenshots
                // TODO2: get last screenshot
                // TODO3: send to TG
                
                // TODO4*: check if selected area is inside Minecraft Window coordinates 
            }
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
}