﻿using HarmonyLib;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Lifecycle;

[HarmonyPatch(typeof(Game))]
internal static class Game_FindSpawnPoint_TriggerLifecycle_Patch
{
    private static bool FirstTime = true;

    static Game_FindSpawnPoint_TriggerLifecycle_Patch()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            FirstTime = true;
        });
    }

    [HarmonyPatch(nameof(Game.FindSpawnPoint))]
    [HarmonyPostfix]
    private static void TriggerLifecycle()
    {
        if (FirstTime)
        {
            FirstTime = false;
            LifecycleManager.OnFindSpawnPointFirstTime.RaiseSafely("Error during OnFindSpawnPointFirstTime event");
        }
    }
}
