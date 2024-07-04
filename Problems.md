## Global & local hooks problem description

Firstly I have tried to create global hook, but it has worked for all active windows. I wanted code to work only with
Minecraft application. Also, I have tried to extract MinecraftProcessId from the current process whose name contains "Minecraft",
but it didn't work cuz an MinecraftProcessId doesn't fit for ```SetWindowsHookEx(...)``` method (4th parameter).
As a result it wasn't a pure Minecraft process, and it was **ApplicationFrameHost** process. As it turned out, the processId was taken for this process (not for MC).

So I have written a local hook. But the code didn't work (perhaps the reason is realProcessId, but I'm not sure).
```https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes```

```
private static IntPtr SetHook(LowLevelKeyboardProc proc)
{
    var openWindowProvider = new OpenWindowProvider();
    var openWindows = openWindowProvider.GetOpenWindows();
    
    foreach (var (windowHandler, windowTitle) in openWindows)
    {
        if (windowTitle.Contains("Minecraft") && !windowTitle.Contains("MinecraftScreenshotsSender"))
        {
            IntPtr pid = IntPtr.Zero;
            uint windowThreadProcessId = GetWindowThreadProcessId(windowHandler, out pid);
            Process p = Process.GetProcessById((int) pid);
            // var storeModuleFileName = p.MainModule.FileName;

            var realProcess = _findHostedProcess.GetRealProcess(p);
            string realModuleFileName = realProcess.MainModule.FileName;
            
            return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(realModuleFileName), Convert.ToUInt32(realProcess.Id));
        }
    }

    return IntPtr.Zero;
}
```

I have removed the previous code and returned to global hook. For now, I check if the active window is Minecraft window.
In this case there was a bug (perhaps a bug) when the application failed after a while (has been called UnhookWindowsHookEx() method)
cuz GC collected LowLevelKeyBoardProc. The solution of this problem is following:
```https://stackoverflow.com/questions/6193711/call-has-been-made-on-garbage-collected-delegate-in-c```
