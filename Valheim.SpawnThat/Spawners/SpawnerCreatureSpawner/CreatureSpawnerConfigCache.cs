using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal static class CreatureSpawnerConfigCache
    {
        public static CreatureSpawnerConfig SearchConfig(string locationName, string creatureName)
        {
            if(ConfigurationManager.CreatureSpawnerConfig?.Subsections is null)
            {
                return null;
            }

            if(ConfigurationManager.CreatureSpawnerConfig.Subsections.TryGetValue(locationName.ToUpperInvariant(), out CreatureLocationConfig locationTable))
            {
#if DEBUG
                Log.LogDebug($"Found config for location {locationName}");
#endif

                if (locationTable.Subsections.TryGetValue(creatureName.Trim().ToUpperInvariant(), out CreatureSpawnerConfig config))
                {
#if DEBUG
                    Log.LogDebug($"Found config for {locationName}:{creatureName}");
#endif

                    return config;
                }
            }

            return null;
        }
    }
}
