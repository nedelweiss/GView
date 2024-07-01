using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MinecraftScreenshotsSender;

public class KeyInterceptor
{
    public delegate void PrintScreenHandler();

    public event PrintScreenHandler OnPrintScreen;
    
    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private readonly IntPtr _hookId;
    private readonly HostedProcessFinder _hostedProcessFinder;
    private LowLevelKeyboardProc _proc; // don't convert it into a local variable cuz of this problem https://stackoverflow.com/questions/6193711/call-has-been-made-on-garbage-collected-delegate-in-c
    
    public KeyInterceptor()
    {
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
                    OnPrintScreen();
                }
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