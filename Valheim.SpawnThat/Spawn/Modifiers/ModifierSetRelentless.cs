using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierSetRelentless : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_relentless";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool Relentless { get; }

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
