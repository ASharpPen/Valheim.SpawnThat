using UnityEngine;
using SpawnThat.Core;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// Forces eternally alerted.
/// </summary>
public class ModifierSetRelentless : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_relentless";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool Relentless { get; set; }

    internal ModifierSetRelentless()
    { }

    public ModifierSetRelentless(bool relentless)
    {
        Relentless = relentless;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entityZdo is null)
        {
            return;
        }

        if (Relentless)
        {
            Log.LogDebug("Setting relentless");
            entityZdo.Set(ZdoFeatureHash, Relentless);
        }
    }
}
