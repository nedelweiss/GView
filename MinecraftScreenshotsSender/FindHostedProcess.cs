using System.Diagnostics;

namespace MinecraftScreenshotsSender;

// https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes
public class FindHostedProcess
{
    private Process _realProcess;

    public Process GetProcessTest()
    {
        return Process.GetProcessById(WinApiFunctions.GetWindowProcessId(WinApiFunctions.GetforegroundWindow()));
    }
}