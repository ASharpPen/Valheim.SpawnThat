using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal class SimpleConfigurationFile : TomlConfigWithSubsections<SimpleConfig>, ITomlConfigFile
{
    protected override SimpleConfig InstantiateSubsection(string subsectionName)
    {
        return new SimpleConfig();
    }
}

internal class SimpleConfig : TomlConfig
{
    public TomlConfigEntry<string> PrefabName = new("PrefabName", "", "Prefab name of entity to modify.");

    public TomlConfigEntry<bool> Enable = new("Enable", true, "Enable/Disable this set of modifiers.");

    public TomlConfigEntry<float> SpawnMaxMultiplier = new("SpawnMaxMultiplier", 1, "Change maximum of total spawned entities. 2 means twice as many.");

    public TomlConfigEntry<float> GroupSizeMinMultiplier = new("GroupSizeMinMultiplier", 1, "Change min number of entities that will spawn at once. 2 means twice as many.");

    public TomlConfigEntry<float> GroupSizeMaxMultiplier = new("GroupSizeMaxMultiplier", 1, "Change max number of entities that will spawn at once. 2 means twice as many.");

    public TomlConfigEntry<float> SpawnFrequencyMultiplier = new("SpawnFrequencyMultiplier", 1, "Change how often the game will try to spawn in new creatures.\nHigher means more often. 2 is twice as often, 0.5 is double the time between spawn checks.");
}
