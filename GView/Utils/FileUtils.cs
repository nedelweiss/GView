using System.IO;

namespace GView.Utils;

public static class FileUtils
{
    private const string MainAppDir = "GView";
    private static readonly string ApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static readonly string GViewDirPath = Path.Combine(ApplicationDataDirectory, MainAppDir);

    public static void WriteToFile(string fileName, string contents)
    {
        File.WriteAllText(Path.Combine(GViewDirPath, fileName), contents);
    } 
    
    public static void AppendToFile(string fileName, string contents)
    {
        File.AppendAllText(Path.Combine(GViewDirPath, fileName), contents);
    } 
}