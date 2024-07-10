using System.IO;

namespace GView.FileUtils;

public static class FileUsageChecker
{
    public static bool IsFileLocked(FileInfo fileInfo)
    {
        try
        {
            using FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            return true;
        }
        return false;
    }
}