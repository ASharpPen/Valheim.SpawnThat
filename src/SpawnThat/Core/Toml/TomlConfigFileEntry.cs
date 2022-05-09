using BepInEx;
using System;
using System.IO;

namespace SpawnThat.Core.Toml;

/// <summary>
/// This config setup is getting really limiting...
/// 
/// This class will attempt to find a file from the binding, and will replace the existing value with the file content.
/// </summary>
internal class TomlConfigFileEntry : TomlConfigEntry<string>
{
    internal TomlConfigFileEntry()
    { }

    public TomlConfigFileEntry(string name, string defaultValue, string description = null)
    {
        Name = name;
        DefaultValue = defaultValue;
        Description = description;
    }

    public new string Name { get; set; }

    public new Type SettingType { get; } = typeof(string);

    private string _value;

    public new string Value { 
        get { return _value; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    var filePath = Path.Combine(Paths.ConfigPath, Value);

                    if (File.Exists(filePath))
                    {

                        _value = File.ReadAllText(filePath);
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
}
