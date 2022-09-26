using UnityEngine;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// Forces eternally alerted.
/// </summary>
public class ModifierSetRelentless : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_relentless";
    public static int ZdoFeatureHash { get; } = ZdoFeature.HashInteger();

    public bool Relentless { get; set; }

    public ModifierSetRelentless()
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
