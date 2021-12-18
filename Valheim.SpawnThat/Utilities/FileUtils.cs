using System.IO;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Utilities;

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
