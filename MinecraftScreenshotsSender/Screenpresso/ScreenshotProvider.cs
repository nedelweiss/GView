using System.IO;

namespace MinecraftScreenshotsSender.Screenpresso;

public class ScreenshotProvider
{
    private readonly ScreenshotsPathProvider _screenshotsPathProvider = new();

    public void Provide()
    {
        var spScreenshotsPath = _screenshotsPathProvider.GetPath();
        DirectoryInfo directoryInfo = new DirectoryInfo(spScreenshotsPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(key => key.CreationTime).ToArray();
        FileInfo fileInfo = fileInfos[0]; // path to needed screenshot

        // TODO: perhaps need to split last modified and last created? No?
        var datetimeResidual = DateTime.Now - fileInfo.CreationTime;
        if (datetimeResidual < TimeSpan.FromSeconds(5))
        {
            // TODO: get the screenshot or return the path to screenshot (depends on what is needed for the discord api)
        }
    }
}