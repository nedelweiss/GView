using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MinecraftScreenshotsSender;

public class KeyInterceptor
{
    private const int WhKeyboardLl = 13;
    private const int WmKeydown = 0x0100;
    private static readonly LowLevelKeyboardProc Proc = HookCallback;
    private static IntPtr _hookId = IntPtr.Zero;

    public KeyInterceptor()
    {
        // new Thread(() => 
        // {
        // Thread.CurrentThread.IsBackground = true; 
        _hookId = SetHook(Proc);
        // }).Start();
    }
    
    ~KeyInterceptor()
    {
        UnhookWindowsHookEx(_hookId);
    }

    // TODO: detect closed Minecraft
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        var openWindowProvider = new OpenWindowProvider();
        var openWindows = openWindowProvider.GetOpenWindows();
        
        foreach (var (key, value) in openWindows)
        {
            if (value.Contains("Minecraft") && !value.Contains("MinecraftScreenshotsSender"))
            {
                IntPtr pid = IntPtr.Zero;
                uint windowThreadProcessId = GetWindowThreadProcessId(key, out pid);
                Process p = Process.GetProcessById((int) pid);
                var mainModuleFileName = p.MainModule.FileName;
                // TODO: https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes
                

                return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(mainModuleFileName), windowThreadProcessId);
            }
        }

        return IntPtr.Zero;
    }

    private static IntPtr GetHMod(string processName)
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            Console.WriteLine(process.ProcessName);
    
            if (process.ProcessName.Contains(processName) && !process.ProcessName.Contains("MinecraftScreenshotsSender"))
            {
                using (ProcessModule currentModule = process.MainModule)
                {
                    return GetModuleHandle(currentModule.ModuleName);
                }
            }
        }

        return IntPtr.Zero;
    }

    // private static IntPtr SetHook(LowLevelKeyboardProc proc)
    // {
    //     Process[] processes = Process.GetProcesses();
    //     foreach (Process process in processes)
    //     {
    //         Console.WriteLine(process.ProcessName);
    //         
    //         if (process.ProcessName.Contains("Minecraft"))
    //         {
    //             IntPtr test = IntPtr.Zero;
    //             var processMainWindowHandle = process.MainWindowHandle;
    //             uint windowThreadProcessId = GetWindowThreadProcessId(processMainWindowHandle, out test);
    //
    //             using (ProcessModule currentModule = process.MainModule)
    //             {
    //                 // 4 Java devs - when key is pressed (event) then call method which is situated by proc reference/pointer
    //                 return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(currentModule.ModuleName), 0);
    //             }
    //         }
    //     }
    //
    //     return IntPtr.Zero;
    // }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        
        if (nCode >= 0 && wParam == (IntPtr) WmKeydown) 
        {
            int keyCode = Marshal.ReadInt32(lParam);
            Keys code = (Keys)keyCode;
            Console.WriteLine(code);
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }
    
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetWindowModuleFileName(IntPtr hWnd, [Out] StringBuilder lpBaseName, uint cchFileNameMax);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out IntPtr processId);
}