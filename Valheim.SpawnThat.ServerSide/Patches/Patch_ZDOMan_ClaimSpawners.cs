using System.Collections.Generic;
using HarmonyLib;
using Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Data;

namespace Valheim.SpawnThat.ServerSide.Patches;

[HarmonyPatch]
internal static class Patch_ZDOMan_ClaimSpawners
{
    [HarmonyPatch(typeof(ZDOMan), nameof(ZDOMan.CreateSyncList))]
    [HarmonyPostfix]
    private static void ClaimZDOsBeingSent(List<ZDO> toSync)
    {
        // Lets take a gander, and grab whats ours.

        var spawnSystemHash = GameConstants.SpawnSystemPrefabHash;
        var id = GameConstants.ServerID;

        var creatureSpawnerHashes = CreatureSpawnerPrefabData.CreatureSpawnerHashes;

        int spawnSystemClaims = 0;
        int creatureSpawnerClaims = 0;

        for (int i = 0; i < toSync.Count; ++i)
        {
            ZDO current = toSync[i];
            if (current.m_owner != id)
            {
                if (current.m_prefab == spawnSystemHash)
                {
                    current.SetOwner(id);
                    spawnSystemClaims++;
                }
                else if (creatureSpawnerHashes.Contains(current.m_prefab))
                {
                    current.SetOwner(id);
                    ++creatureSpawnerClaims;
                }
            }
        }

#if DEBUG
        if (spawnSystemClaims > 0)
        {
            Log.LogDebug($"Claimed {spawnSystemClaims} SpawnSystem zdo's");
        }
        if (creatureSpawnerClaims > 0)
        {
            Log.LogDebug($"Claimed {creatureSpawnerClaims} CreatureSpawner zdo's");
        }
#endif
    }
}
