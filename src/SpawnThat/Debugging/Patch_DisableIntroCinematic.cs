#if DEBUG

using HarmonyLib;

namespace SpawnThat.Debugging;

/// <summary>
/// Please IG... I just want to load the game...
/// </summary>
[HarmonyPatch(typeof(CinematicsManager))]
internal class Patch_DisableIntroCinematic
{
    [HarmonyPatch(nameof(CinematicsManager.Awake))]
    [HarmonyPostfix]
    internal static void Disable()
    {
        CinematicsManager.s_instance.m_introOnStartup = false;
        CinematicsManager.s_instance.m_introOnNewWorld = false;
    }
}

[HarmonyPatch(typeof(SceneLoader))]
internal class Patch_DisableIntroSplash()
{
    [HarmonyPatch("Start")]
    [HarmonyPrefix]
    internal static bool Disable(SceneLoader __instance)
    {
        __instance._showLogos = false;

        return true;
    }
}

#endif