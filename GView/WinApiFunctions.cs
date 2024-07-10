using System.Runtime.InteropServices;

namespace GView;

public class WinApiFunctions
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();
    
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    public static int GetWindowProcessId(IntPtr hwnd)
    {
        int pid;
        GetWindowThreadProcessId(hwnd, out pid);
        return pid;
    }
    
    public static IntPtr GetforegroundWindow()
    {
        return GetForegroundWindow();
    }
}