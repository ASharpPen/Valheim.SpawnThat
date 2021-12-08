using System;
using HarmonyLib;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Simulators;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

namespace Valheim.SpawnThat.ServerSide.Debug;

[HarmonyPatch]
internal class InitializeDefaultCreatureSpawner
{
    private static CreatureSpawnerSimulator CreatureSpawnerSimulator { get; set; } = new();

    private static bool Initialized { get; set; }

    static InitializeDefaultCreatureSpawner()
    {
        StateResetter.Subscribe(() =>
        {
            CreatureSpawnerSimulator = new CreatureSpawnerSimulator();
            Initialized = false;
        });
    }

    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start))]
    [HarmonyPostfix]
    private static void InitSimulation()
    {
        // Probably completely redundant.
        if (Initialized)
        {
            return;
        }

        Initialized = true;

        TickService.SubscribeToUpdate(CreatureSpawnerSimulator.Update, TimeSpan.FromSeconds(1));
    }
}
