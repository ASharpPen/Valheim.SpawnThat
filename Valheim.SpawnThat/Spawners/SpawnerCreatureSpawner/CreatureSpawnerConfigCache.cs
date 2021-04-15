using System.Collections.Generic;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal static class CreatureSpawnerConfigCache
    {
        private static Dictionary<string, Dictionary<string, CreatureSpawnerConfig>> ConfigLookupTable = null;

        static CreatureSpawnerConfigCache()
        {
            StateResetter.Subscribe(() =>
            {
                ConfigLookupTable = null;
            });
        }

        public static CreatureSpawnerConfig SearchConfig(string locationName, string creatureName)
        {
            if(ConfigLookupTable == null)
            {
                InitializeConfigs();
            }

            if(ConfigLookupTable.TryGetValue(locationName.ToUpperInvariant(), out Dictionary<string, CreatureSpawnerConfig> locationTable))
            {
#if DEBUG
                Log.LogDebug($"Found config for location {locationName}");
#endif

                if (locationTable.TryGetValue(creatureName.ToUpperInvariant(), out CreatureSpawnerConfig config))
                {
#if DEBUG
                    Log.LogDebug($"Found config for {locationName}:{creatureName}");
#endif

                    return config;
                }
            }

            return null;
        }

        private static void InitializeConfigs()
        {
            var configs = ConfigurationManager.CreatureSpawnerConfig;

            ConfigLookupTable = new Dictionary<string, Dictionary<string, CreatureSpawnerConfig>>();

            foreach (var config in configs)
            {
                var cleanedLocationName = config.Key.Trim().ToUpperInvariant();

                Dictionary<string, CreatureSpawnerConfig> locationTable;
                if (ConfigLookupTable.TryGetValue(cleanedLocationName, out Dictionary<string, CreatureSpawnerConfig> existingTable))
                {
                    locationTable = existingTable;
                }
                else
                {
                    locationTable = new Dictionary<string, CreatureSpawnerConfig>();
                    ConfigLookupTable.Add(cleanedLocationName, locationTable);
                }

                foreach (var creature in config.Value.Sections)
                {
                    var cleanedCreatureName = creature.Key.Trim().ToUpperInvariant();

                    if (locationTable.ContainsKey(cleanedCreatureName))
                    {
                        Log.LogWarning($"Multiple creature spawner configurations detected for {config.Key}.{creature.Key}, overriding last seen.");
                    }

                    locationTable[cleanedCreatureName] = creature.Value;
                }
            }
        }
    }
}
