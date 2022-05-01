using BepInEx.Configuration;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Configuration;

internal class GeneralConfiguration
{
    public GeneralConfiguration()
    { }

    private ConfigFile Config;

    #region Debug

    public ConfigurationEntry<bool> DebugLoggingOn = new ConfigurationEntry<bool>(false, "Enable debug logging.");

    public ConfigurationEntry<bool> TraceLoggingOn = new ConfigurationEntry<bool>(false, "Enables in-depth logging. Note, this might generate a LOT of log entries.");

    public ConfigurationEntry<bool> PrintAreaMap = new ConfigurationEntry<bool>(false, "Prints a set of pngs showing the area id's assigned by Spawn That to each biome 'area'. Each pixel's hex value corresponds to an area id, when converted into a decimal.");

    public ConfigurationEntry<bool> PrintBiomeMap = new ConfigurationEntry<bool>(false, "Prints a map of the biome of each zone.");

    public ConfigurationEntry<bool> PrintFantasticBeastsAndWhereToKillThem = new ConfigurationEntry<bool>(false, "Prints maps marking where each configured world spawn template can spawn. This will be done for every config entry.");

    public ConfigurationEntry<string> DebugFileFolder = new("Debug", "Folder path to write to. Root folder is BepInEx.");

    #endregion

    #region General

    public ConfigurationEntry<bool> StopTouchingMyConfigs = new ConfigurationEntry<bool>(true, "Disables automatic updating and saving of configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.\nNote: Can have massive impact on load times.");

    #endregion

    #region Simple

    public ConfigurationEntry<bool> InitializeWithCreatures = new ConfigurationEntry<bool>(true, "If true, fills in simple cfg with a list of creatures when file is created.");

    #endregion

    #region WorldSpawner

    public ConfigurationEntry<bool> ClearAllExisting = new ConfigurationEntry<bool>(false, "If true, removes all existing world spawner templates.");

    public ConfigurationEntry<bool> AlwaysAppend = new ConfigurationEntry<bool>(false, "If true, will never override existing spawners, but add all custom configurations to the list.");

    public ConfigurationEntry<bool> WriteSpawnTablesToFileBeforeChanges = new ConfigurationEntry<bool>(false, "Writes world spawner templates to a file, before applying custom changes.");

    public ConfigurationEntry<bool> WriteSpawnTablesToFileAfterChanges = new ConfigurationEntry<bool>(false, "Writes world spawner templates to a file after applying configuration changes.");

    #endregion

    #region LocalSpawner

    public ConfigurationEntry<bool> WriteCreatureSpawnersToFile = new ConfigurationEntry<bool>(false, "Writes local spawners to a file before applying configuration changes.");

    public ConfigurationEntry<bool> DontCollapseFile = new ConfigurationEntry<bool>(false, "If true, locations with multiple spawners with duplicate creatures will be listed individually, instead of being only one of each creature.");

    public ConfigurationEntry<bool> EnableLocalSpawner = new ConfigurationEntry<bool>(true, "Toggles if Spawn That changes to local spawners will be run or not.");

    #endregion

    #region DestructibleSpawner

    public ConfigurationEntry<bool> WriteDestructibleSpawnersToFile = new ConfigurationEntry<bool>(false, "Writes destructible spawners to a file before applying configuration changes.");

    #endregion

    public void Load(ConfigFile configFile)
    {
        Config = configFile;

        DebugLoggingOn.Bind(Config, "Debug", "DebugLoggingOn");
        TraceLoggingOn.Bind(Config, "Debug", "TraceLoggingOn");
        PrintAreaMap.Bind(Config, "Debug", "PrintAreaMap");
        PrintBiomeMap.Bind(Config, "Debug", nameof(PrintBiomeMap));
        PrintFantasticBeastsAndWhereToKillThem.Bind(Config, "Debug", "PrintFantasticBeastsAndWhereToKillThem");
        DebugFileFolder.Bind(Config, "Debug", nameof(DebugFileFolder));

        WriteCreatureSpawnersToFile.Bind(Config, "LocalSpawner", "WriteSpawnTablesToFileBeforeChanges");
        DontCollapseFile.Bind(Config, "LocalSpawner", nameof(DontCollapseFile));
        EnableLocalSpawner.Bind(Config, "LocalSpawner", "Enable");

        ClearAllExisting.Bind(Config, "WorldSpawner", "ClearAllExisting");
        AlwaysAppend.Bind(Config, "WorldSpawner", "AlwaysAppend");
        WriteSpawnTablesToFileBeforeChanges.Bind(Config, "WorldSpawner", "WriteSpawnTablesToFileBeforeChanges");
        WriteSpawnTablesToFileAfterChanges.Bind(Config, "WorldSpawner", "WriteSpawnTablesToFileAfterChanges");

        InitializeWithCreatures.Bind(Config, "Simple", "InitializeWithCreatures");

        WriteDestructibleSpawnersToFile.Bind(Config, "DestructibleSpawner", "WriteSpawnTablesToFileBeforeChanges");

        StopTouchingMyConfigs.Bind(Config, "General", nameof(StopTouchingMyConfigs));
    }
}
