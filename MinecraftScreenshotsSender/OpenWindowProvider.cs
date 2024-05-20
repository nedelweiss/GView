using System.Runtime.InteropServices;
using System.Text;
using INT_PTR = System.IntPtr;

namespace MinecraftScreenshotsSender;

public class OpenWindowProvider
{
    public IDictionary<INT_PTR, string> GetOpenWindows()
    {
        INT_PTR shellWindow = GetShellWindow();
        Dictionary<INT_PTR, string> windows = new Dictionary<INT_PTR, string>();

        EnumWindows(delegate(INT_PTR hWnd, int lParam)
        {
            if (hWnd == shellWindow) return true;
            if (!IsWindowVisible(hWnd)) return true;

            int length = GetWindowTextLength(hWnd);
            if (length == 0) return true;

            StringBuilder builder = new StringBuilder(length);
            GetWindowText(hWnd, builder, length + 1);

            windows[hWnd] = builder.ToString();
            return true;

        }, 0);

        return windows;
    }

    private delegate bool EnumWindowsProc(INT_PTR hWnd, int lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(INT_PTR hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(INT_PTR hWnd);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(INT_PTR hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();
}