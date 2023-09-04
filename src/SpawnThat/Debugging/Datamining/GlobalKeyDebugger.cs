#if DEBUG

using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SpawnThat.Core;

namespace SpawnThat.Debugging.Datamining;

[HarmonyPatch]
internal sealed class GlobalKeyDebugger
{
    [HarmonyPatch(typeof(Terminal), nameof(Terminal.TryRunCommand))]
    [HarmonyPostfix]
    private static void FindAndPrintGlobalKeys(string text)
    {
        if (text != "print keys")
        {
            return;
        }

        HashSet<string> keys = new();

        foreach(var prefab in ZNetScene.instance.m_namedPrefabs.Values)
        {
            if (prefab.TryGetComponent<Character>(out var comp))
            {
                keys.Add(comp.m_defeatSetGlobalKey);
            }
        }

        foreach (var entry in RandEventSystem.instance.m_events)
        {
            foreach (var key in entry.m_notRequiredGlobalKeys)
            {
                keys.Add(key);
            }

            foreach (var key in entry.m_requiredGlobalKeys)
            {
                keys.Add(key);
            }
        }

        foreach (var list in SpawnSystem.m_instances.First().m_spawnLists)
        {
            foreach (var entry in list.m_spawners)
            {
                keys.Add(entry.m_requiredGlobalKey);
            }
        }

        foreach (var key in keys)
        {
            Log.LogInfo(key);
        }
    }
}

#endif