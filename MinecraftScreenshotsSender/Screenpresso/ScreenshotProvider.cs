using System.IO;

namespace MinecraftScreenshotsSender.Screenpresso;

public class ScreenshotProvider
{
    private readonly ScreenshotsPathProvider _screenshotsPathProvider = new();

    public string? Provide()
    {
        var spScreenshotsPath = _screenshotsPathProvider.GetPath();
        DirectoryInfo directoryInfo = new DirectoryInfo(spScreenshotsPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(key => key.CreationTime).ToArray();
        FileInfo fileInfo = fileInfos[0]; // includes path to needed screenshot
        var datetimeResidual = DateTime.Now - fileInfo.CreationTime; // TODO: perhaps need to split last modified and last created?
        return datetimeResidual < TimeSpan.FromSeconds(10) 
            ? fileInfo.FullName 
            : null;
    }
}