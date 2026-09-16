using HarmonyLib;

namespace SpawnThat.Lifecycle.Patches;

[HarmonyPatch(typeof(AltBiomeWorldData))]
internal static class AltBiomeWorldData_TriggerLifecycle_Patch
{
    private static bool FirstTime = true;
    
    static AltBiomeWorldData_TriggerLifecycle_Patch()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            FirstTime = true;
        });
    }

    [HarmonyPatch(nameof(AltBiomeWorldData.VerifyBiomeData))]
    [HarmonyPostfix]
    private static void TriggerLifecycle()
    {
        if (FirstTime)
        {
            FirstTime = false;
            LifecycleManager.InitBiomesLoaded();
        }
    }
}
