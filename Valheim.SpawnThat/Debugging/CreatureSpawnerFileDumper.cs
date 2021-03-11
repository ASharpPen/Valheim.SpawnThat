using BepInEx;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Debugging
{
    public static class CreatureSpawnerFileDumper
    {
        private const string FileName = "default_local_spawners.txt";

        public static void WriteToFile(List<ZoneSystem.ZoneLocation> zoneLocations)
        {
            List<string> spawnersSerialized = new List<string>();

            var orderedLocations = zoneLocations
                .OrderBy(x => x.m_biome)
                .ThenBy(x => x.m_prefabName);

            foreach (var location in orderedLocations)
            {
                var locPrefab = location.m_prefab;

                if (locPrefab is null)
                {
                    Log.LogTrace("LocPrefab is null: " + location.m_prefabName);
                    continue;
                }

                var spawners = locPrefab.GetComponentsInChildren<CreatureSpawner>();

                if (spawners is null)
                {
                    continue;
                }

                if (spawners != null && spawners.Length > 0)
                {
                    spawnersSerialized.Add(Serialize(location, spawners));
                }
            }

            string filePath = Path.Combine(Paths.PluginPath, FileName);

            Log.LogDebug($"Writing defalt local spawn configurations to {filePath}");

            File.WriteAllLines(filePath, spawnersSerialized);
        }

        private static string Serialize(ZoneSystem.ZoneLocation location, CreatureSpawner[] spawners)
        {
            var biome = location.m_biome;
            var locationName = location.m_prefabName;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < spawners.Length; ++i)
            {
                var spawner = spawners[i];

                stringBuilder.AppendLine($"[{biome}_{locationName}.{i}]");

                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.PrefabName)}={spawner.m_creaturePrefab.name}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.Enabled)}={spawner.enabled}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.Locations)}={locationName}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtDay)}={spawner.m_spawnAtDay}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtNight)}={spawner.m_spawnAtNight}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMin)}={spawner.m_minLevel}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMax)}={spawner.m_maxLevel}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelUpChance)}={spawner.m_levelupChance}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RespawnTime)}={spawner.m_respawnTimeMinuts}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerDistance)}={spawner.m_triggerDistance}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerNoise)}={spawner.m_triggerNoise}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnInPlayerBase)}={spawner.m_spawnInPlayerBase}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SetPatrolPoint)}={spawner.m_setPatrolSpawnPoint}");
                //stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RequireSpawnArea)}={spawner.m_requireSpawnArea}"); //Disabled for now, due to not seemingly being used.
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
