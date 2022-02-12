using BepInEx;
using System;
using System.IO;

namespace SpawnThat.Core.Configuration;

/// <summary>
/// This config setup is getting really limiting...
/// 
/// This class will attempt to find a file from the binding, and will replace the existing value with the file content.
/// </summary>
internal class ConfigurationFileEntry : ConfigurationEntry<string>
{
    public ConfigurationFileEntry()
    {
    }

    public ConfigurationFileEntry(string defaultValue, string description = null) : base(defaultValue, description)
    {
    }

    protected override void PostBind()
    {
        if (!string.IsNullOrWhiteSpace(Value))
        {
            try
            {
                var filePath = Path.Combine(Paths.ConfigPath, Value);

                if (File.Exists(filePath))
                {

                    DefaultValue = File.ReadAllText(filePath);
                    Config = null;
#if DEBUG
                    Log.LogDebug("File content: " + DefaultValue);
#endif
                }
                else
                {
                    Log.LogWarning($"Unable to find MobAI json config file at '{filePath}'");
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error while attempting to read MobAI config.", e);
            }
        }
    }
}
