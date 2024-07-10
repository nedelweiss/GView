using System.IO;
using GView.XmlParsing;

namespace GView.Screenpresso;

public class ScreenshotsPathProvider
{
    private const string SpSettingsTargetTag = "ScreenpressoSettings/TempFolderScreenShots";
    private const string SpDefaultDirPathPart = @"\Pictures\Screenpresso";
    private const string SpSettingsFileName = "settings.xml";
    private const string SpParentDir = "Learnpulse";
    private const string SpTargetDir = "Screenpresso";
    
    private readonly XmlParser _xmlParser = new();

    public string GetPath()
    {
        string spSettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            SpParentDir,
            SpTargetDir,
            SpSettingsFileName
        );
        string? path = _xmlParser.GetNodeText(spSettingsFilePath, SpSettingsTargetTag);
        return path ?? BuildDefaultFolderPath();
    }

    private static string BuildDefaultFolderPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + SpDefaultDirPathPart;
    }
}