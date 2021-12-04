using System;
using System.Collections.Generic;
using HarmonyLib;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

[HarmonyPatch]
internal static class TickService
{
    private static List<Action> UpdateSubscribers = new();

    static TickService()
    {
        StateResetter.Subscribe(() =>
        {
            Configuration.Multiplayer.ConfigMultiplayerPatch.EnableSync = false;
            UpdateSubscribers = new();
        });
    }

    [HarmonyPatch(typeof(ZNet), nameof(ZNet.Update))]
    [HarmonyPostfix]
    public static void ZNetUpdate()
    {
        Update();
    }

    private static void Update()
    {
        foreach (var action in UpdateSubscribers)
        {
            action();
        }
    }

    internal static void SubscribeToUpdate(Action action)
    {
        UpdateSubscribers.Add(action);
    }
}
