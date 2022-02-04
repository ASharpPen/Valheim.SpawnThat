using SpawnThat.Core;
using SpawnThat.Spawners.LocalSpawner.Models;

namespace SpawnThat.Spawners.LocalSpawner.Services;

internal static class LocalSpawnerConfigurationService
{
    internal static void ConfigureSpawner(CreatureSpawner spawner, LocalSpawnTemplate template)
    {
        if (template is null)
        {
            return;
        }

        if (!template.TemplateEnabled)
        {
            return;
        }

        Log.LogDebug($"Found and applying config for local spawner {spawner.name}");

        var prefab = spawner.m_creaturePrefab;

        //Find creature prefab, if it needs to be overriden
        if (string.IsNullOrWhiteSpace(template.PrefabName) || 
            prefab.name != template.PrefabName)
        {
            prefab = ZNetScene.instance.GetPrefab(template.PrefabName);

            if (!prefab || prefab is null)
            {
                Log.LogWarning($"Unable to find prefab for {template.PrefabName}. Skipping configuration.");
                return;
            }
        }

        //Override existing config values:
        spawner.m_creaturePrefab = prefab;
        spawner.m_maxLevel = template.MaxLevel ?? spawner.m_maxLevel;
        spawner.m_minLevel = template.MinLevel ?? spawner.m_minLevel;
        //__instance.m_requireSpawnArea = config.RequireSpawnArea.Value; //Disabled for now, since it isn't being used by the game.
        spawner.m_respawnTimeMinuts = (float)(template.SpawnInterval?.TotalMinutes ?? spawner.m_respawnTimeMinuts);
        spawner.m_setPatrolSpawnPoint = template.SetPatrolSpawn ?? spawner.m_setPatrolSpawnPoint;
        spawner.m_spawnAtDay = template.SpawnAtDay ?? spawner.m_spawnAtDay;
        spawner.m_spawnAtNight = template.SpawnAtNight ?? spawner.m_spawnAtNight;
        spawner.m_triggerDistance = template.ConditionPlayerDistance ?? spawner.m_triggerDistance;
        spawner.m_triggerNoise = template.ConditionPlayerNoise ?? spawner.m_triggerNoise;
        spawner.m_spawnInPlayerBase = template.AllowSpawnInPlayerBase ?? spawner.m_spawnInPlayerBase;
    }
}
