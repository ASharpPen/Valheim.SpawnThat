using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Data;

[HarmonyPatch]
public static class CreatureSpawnerPrefabData
{
    private static Type CreatureSpawnerType = typeof(CreatureSpawner);
    public static HashSet<int> CreatureSpawnerHashes { get; } = new();

    static CreatureSpawnerPrefabData()
    {
        StateResetter.Subscribe(() =>
        {
            CreatureSpawnerHashes.Clear();
        });
    }

    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start))]
    [HarmonyPostfix]
    private static void ScanForCreatureSpawnerPrefabs()
    {
        var allPrefabs = ZNetScene.instance.m_prefabs;

        foreach (var prefab in allPrefabs)
        {
            if (prefab.TryGetComponent(CreatureSpawnerType, out var component))
            {
                CreatureSpawnerHashes.Add(prefab.name.GetStableHashCode());
            }
        }
    }
}
