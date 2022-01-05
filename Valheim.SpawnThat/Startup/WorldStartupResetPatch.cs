using HarmonyLib;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Startup;

[HarmonyPatch(typeof(FejdStartup))]
public static class WorldStartupResetPatch
{
    /// <summary>
    /// Singleplayer
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.OnWorldStart))]
    [HarmonyPrefix]
    private static void ResetState()
    {
        Log.LogDebug("Resetting configurations");
        StateResetter.Reset();
        ConfigurationManager.LoadAllConfigurations();
        SpawnSystemConfigManager.Wait = false;
        CreatureSpawnerConfigManager.Wait = false;
    }

    /// <summary>
    /// Multiplayer
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.JoinServer))]
    [HarmonyPrefix]
    private static void ResetStateMultiplayer()
    {
        Log.LogDebug("Resetting configurations");
        StateResetter.Reset();
    }

    /// <summary>
    /// Server
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.ParseServerArguments))]
    [HarmonyPrefix]
    private static void ResetStateServer()
    {
        Log.LogDebug("Resetting configurations");
        StateResetter.Reset();
        ConfigurationManager.LoadAllConfigurations();
        SpawnSystemConfigManager.Wait = false;
        CreatureSpawnerConfigManager.Wait = false;
    }
}
