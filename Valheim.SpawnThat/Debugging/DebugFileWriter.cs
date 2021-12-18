using System.Collections.Generic;
using System.IO;
using BepInEx;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Debugging;

internal static class DebugFileWriter
{
    public static void WriteFile(string content, string filename, string fileDescription)
    {
        var filePath = Prepare(filename, fileDescription);

        File.WriteAllText(filePath, content);
    }

    public static void WriteFile(List<string> content, string filename, string fileDescription)
    {
        var filePath = Prepare(filename, fileDescription);

        File.WriteAllLines(filePath, content);
    }

    public static void WriteFile(byte[] content, string filename, string fileDescription)
    {
        var filePath = Prepare(filename, fileDescription);

        File.WriteAllBytes(filePath, content);
    }

    private static string Prepare(string filename, string fileDescription)
    {
        string debugDir = "Debug";

        if (ConfigurationManager.GeneralConfig?.DebugFileFolder is not null)
        {
            debugDir = Path.Combine(ConfigurationManager.GeneralConfig.DebugFileFolder.Value.SplitBySlash());
        }

        string filePath = Path.Combine(Paths.BepInExRootPath, debugDir, filename);

        FileUtils.EnsureDirectoryExistsForFile(filePath);

        Log.LogInfo($"Writing {fileDescription} to file {filePath}.");

        return filePath;
    }
}
