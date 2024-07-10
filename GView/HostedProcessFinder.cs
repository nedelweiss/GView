using System.Diagnostics;

namespace GView;

// https://stackoverflow.com/questions/39702704/connecting-uwp-apps-hosted-by-applicationframehost-to-their-real-processes
public class HostedProcessFinder
{
    public Process Find()
    {
        return Process.GetProcessById(WinApiFunctions.GetWindowProcessId(WinApiFunctions.GetforegroundWindow()));
    }
}