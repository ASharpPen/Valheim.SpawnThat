using HarmonyLib;

namespace SpawnThat.Lifecycle.Patches;

[HarmonyPatch]
internal static class FejdStartup_TriggerLifecycle_Patch
{
    /// <summary>
    /// Singleplayer
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.OnWorldStart))]
    [HarmonyPrefix]
    private static void InitSingleplayer() => LifecycleManager.InitSingleplayer();

    /// <summary>
    /// Multiplayer
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.JoinServer))]
    [HarmonyPrefix]
    private static void InitMultiplayer() => LifecycleManager.InitMultiplayer();

    /// <summary>
    /// Server
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.ParseServerArguments))]
    [HarmonyPrefix]
    private static void InitDedicated() => LifecycleManager.InitDedicated();
}
