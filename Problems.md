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

## Unable to load solution in JetBrains Rider
If this problem occurs, this may be the solution.
```https://stackoverflow.com/questions/55564893/unable-to-load-solution-in-jetbrains-rider```

Error log:
```angular2html
No loaded projects
CONSOLE: Use build tool: C:\Program Files\JetBrains\JetBrains Rider 2024.1.4\tools\MSBuild\Current\Bin\amd64\MSBuild.exe
MSBuild version 17.8.3+195e7f5a3 for .NET Framework
MSBUILD : error MSB1004: Specify the name of the target.
    Full command line: /*...*/
Switches appended by response files:
'' came from 'C:\Program Files\JetBrains\JetBrains Rider 2024.1.4\tools\MSBuild\Current\Bin\amd64\MSBuild.rsp'
Switch: /t:

For switch syntax, type "MSBuild -help"
```

## Typing TextBox data & KeyInterceptor logic
When you enter the title of the game in the text field, in the Interceptor class, when checking for presence the entered title and the title taken from hostedProcess.MainWindowTitle, 
a next situation may occured when you enter one letter, and it is immediately checked for presence in the title taken from hostedProcess.MainWindowTitle.
It may not be a game at all, it can be some other application that contains _**this one letter**_ in its title.
And in this way, the screenshot can be sent to some completely different random application.

The problem was solved in the following way: ...

- ```https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/?view=netdesktop-8.0```
- ```https://stackoverflow.com/questions/28159636/binding-a-textbox-to-a-property-of-a-class```
- ```https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-control-when-the-textbox-text-updates-the-source?view=netframeworkdesktop-4.8```
- ```https://stackoverflow.com/questions/6700130/is-any-event-triggered-before-the-source-of-a-wpf-binding-is-updated```
- ```https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-control-when-the-textbox-text-updates-the-source?view=netframeworkdesktop-4.8```
- ```https://stackoverflow.com/questions/4253088/updating-gui-wpf-using-a-different-thread```