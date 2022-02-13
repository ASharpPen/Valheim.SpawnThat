using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal class DestructibleSpawnSessionManager
{
    private static List<SpawnData> _defaultSpawnData;

    public static void FilterSpawnData(SpawnArea spawner)
    {
        _defaultSpawnData = spawner.m_prefabs;

        var template = DestructibleSpawnerManager.GetTemplate(spawner);

        if (template is null)
        {
            return;
        }

        foreach (var spawn in template.Spawns)
        {

        }
    }

    /// <summary>
    /// Reassign un-filtered spawn data.
    /// </summary>
    public static void ResetSpawnData(SpawnArea spawner)
    {
        if (spawner.m_prefabs != _defaultSpawnData)
        {
            spawner.m_prefabs = _defaultSpawnData;
        }
    }
}
