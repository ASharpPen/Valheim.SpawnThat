using System;
using System.Collections.Generic;
using HarmonyLib;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

[HarmonyPatch]
internal static class TickService
{
    private static List<UpdateAction> UpdateSubscribers = new();

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
        foreach (var updateAction in UpdateSubscribers)
        {
            try
            {
                if (updateAction is null)
                {
                    Log.LogError("Update action is null, wtf?");
                }

                var znet = ZNet.instance;

                if (updateAction.Frequency is null)
                {
                    updateAction.Action();
                }
                else if (znet != null && znet)
                {
                    var currentTime = znet.GetTimeSeconds();

                    if (currentTime - updateAction.LastRun > updateAction.Frequency.Value.TotalSeconds)
                    {
                        updateAction.LastRun = currentTime;
                        updateAction.Action();
                    }
                }
            }
            catch(Exception e)
            {
                Log.LogError("Error during attempt at running action", e);
#if DEBUG
                Log.LogError(e.StackTrace);
#endif
            }
        }
    }

    internal static void SubscribeToUpdate(Action action, TimeSpan? frequency = null)
    {
        var updateAction = new UpdateAction
        {
            Action = action,
            Frequency = frequency
        };

        UpdateSubscribers.Add(updateAction);
    }

    private class UpdateAction
    {
        public Action Action { get; set; }
        public TimeSpan? Frequency { get; set; }
        public double LastRun { get; set; }
    }
}
