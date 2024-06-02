## Task Description

##### ---in progress

## Programming description

Firstly I have tried to create global hook, but it has worked for all active windows. I wanted code to work only with
Minecraft application. Also, I have tried to extract MinecraftProcessId from the current process whose name contains "Minecraft",
but it didn't work cuz an MinecraftProcessId doesn't fit for ```SetWindowsHookEx(...)``` method (4th parameter). 
As a result it wasn't a pure Minecraft process, and it was **ApplicationFrameHost** process. As it turned out, the processId was taken for this process (not for MC).

###### This process is related to Universal Windows Platform apps, also known as Store apps---the new type of app included with Windows 10.These are generally acquired from the Windows Store, although most of Windows 10's included apps like Mail, Calculator, OneNote, Movies & TV, Photos, and Groove Music are also UWP apps. Specifically, this process is responsible for displaying these applications in frames (windows) on your desktop, whether you're using Windows 10 in desktop mode or tablet mode. If you forcibly end this process, all your open UWP apps will close.


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

## Useful links

```https://learn.microsoft.com/en-us/archive/blogs/toub/low-level-keyboard-hook-in-c```

```https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes```

```https://stackoverflow.com/questions/21103669/how-do-i-create-an-event-loop-message-pipe-in-a-console-application```

```https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application```

```https://stackoverflow.com/questions/7268302/get-the-titles-of-all-open-windows```

```https://stackoverflow.com/questions/277085/how-do-i-getmodulefilename-if-i-only-have-a-window-handle-hwnd```