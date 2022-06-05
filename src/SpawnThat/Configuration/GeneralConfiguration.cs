using BepInEx.Configuration;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Configuration;

internal class GeneralConfiguration
{
    public GeneralConfiguration()
    { }

    private ConfigFile Config;

    #region Debug

    public ConfigurationEntry<bool> DebugLoggingOn = new(false, "Enable debug logging.");

    public ConfigurationEntry<bool> TraceLoggingOn = new(false, "Enables in-depth logging. Note, this might generate a LOT of log entries.");

    public ConfigurationEntry<bool> PrintAreaMap = new(false, "Prints a set of pngs showing the area id's assigned by Spawn That to each biome 'area'. Each pixel's hex value corresponds to an area id, when converted into a decimal.");

    public ConfigurationEntry<bool> PrintBiomeMap = new(false, "Prints a map of the biome of each zone.");

    public ConfigurationEntry<bool> PrintFantasticBeastsAndWhereToKillThem = new(false, "Prints maps marking where each configured world spawn template can spawn. This will be done for every config entry.");

    public ConfigurationEntry<string> DebugFileFolder = new("Debug", "Folder path to write to. Root folder is BepInEx.");

    #endregion

    #region General

    #endregion

    #region Simple

    public ConfigurationEntry<bool> InitializeWithCreatures = new(true, "If true, fills in simple cfg with a list of creatures when file is created.");

    #endregion

    #region WorldSpawner

    public ConfigurationEntry<bool> ClearAllExisting = new(false, "If true, removes all existing world spawner templates.");

    public ConfigurationEntry<bool> AlwaysAppend = new(false, "If true, will never override existing spawners, but add all custom configurations to the list.");

    public ConfigurationEntry<bool> WriteSpawnTablesToFileBeforeChanges = new(false, "Writes world spawner templates to a file, before applying custom changes.");

    public ConfigurationEntry<bool> WriteSpawnTablesToFileAfterChanges = new(false, "Writes world spawner templates to a file after applying configuration changes.");

    public ConfigurationEntry<bool> WriteWorldSpawnerConfigsToFile = new(false, "Writes world spawner configs loaded in Spawn That to file, before applying to spawners in game.\nThese are the configs loaded from files and set by other mods, after being merged and prepared internally in Spawn That.");

    public ConfigurationEntry<bool> WorldSpawnerAddCommentsToFile = new(false, "Add comments to settings in debug files.");

    #endregion

    #region LocalSpawner

    public ConfigurationEntry<bool> WriteCreatureSpawnersToFile = new(false, "Writes local spawners to a file before applying configuration changes.");

    public ConfigurationEntry<bool> DontCollapseFile = new(false, "If true, locations with multiple spawners with duplicate creatures will be listed individually, instead of being only one of each creature.");

    public ConfigurationEntry<bool> EnableLocalSpawner = new(true, "Toggles if Spawn That changes to local spawners will be run or not.");

    #endregion

    #region DestructibleSpawner

    public ConfigurationEntry<bool> WriteDestructibleSpawnersToFile = new(false, "Writes destructible spawners to a file before applying configuration changes.");

    public ConfigurationEntry<bool> WriteDestructibleConfigsToFile = new(false, "Writes destructible spawner configs loaded in Spawn That to file, before applying to spawners in game.\nThese are the configs loaded from files and set by other mods, after being merged and prepared internally in Spawn That.");

    public ConfigurationEntry<bool> DestructibleAddCommentsToFile = new(false, "Add comments to settings in debug files.");

    #endregion

    #region Datamining

    public ConfigurationEntry<bool> WriteLocationsToFile = new(false, "Writes all locations loaded to a file, sectioned by the biome in which they can appear.");

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
        WriteWorldSpawnerConfigsToFile.Bind(Config, "WorldSpawner", "WriteConfigsToFile");

        InitializeWithCreatures.Bind(Config, "Simple", "InitializeWithCreatures");

        WriteDestructibleSpawnersToFile.Bind(Config, "DestructibleSpawner", "WriteSpawnTablesToFileBeforeChanges");
        WriteDestructibleConfigsToFile.Bind(Config, "DestructibleSpawner", "WriteConfigsToFile");
        //DestructibleAddCommentsToFile.Bind(Config, "DestructibleSpawner", "AddCommentsToFile"); // TODO: Hm... Not sure about this one.

        WriteLocationsToFile.Bind(Config, "Datamining", "WriteLocationsToFile");
    }
}
