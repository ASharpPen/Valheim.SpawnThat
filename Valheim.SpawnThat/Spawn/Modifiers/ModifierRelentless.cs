using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierRelentless : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_relentless";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entityZdo is null)
        {
            return;
        }

        Log.LogDebug("Setting relentless");
        entityZdo.Set(ZdoFeatureHash, true);
    }
}
