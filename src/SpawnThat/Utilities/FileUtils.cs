using System.IO;
using SpawnThat.Core;

namespace SpawnThat.Utilities;

public static class FileUtils
{
    public static void EnsureDirectoryExistsForFile(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Log.LogTrace("Creating missing folders in path.");
            Directory.CreateDirectory(dir);
        }
    }
}
