using UnityEngine;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierDespawnOnAlert : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_despawn_on_alert";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entityZdo is null)
        {
            return;
        }

        entityZdo.Set(ZdoFeature, true);
    }
}
