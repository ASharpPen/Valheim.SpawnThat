using UnityEngine;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// When creature is alerted, it will trigger default AI
/// for attempting to run away and depsawn.
/// </summary>
/// <remarks>Requires MonsterAI component on entity.</remarks>
public class ModifierDespawnOnAlert : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_despawn_on_alert";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool DespawnOnAlert { get; set; }

    internal ModifierDespawnOnAlert()
    { }

    public ModifierDespawnOnAlert(bool despawnOnAlert)
    {
        DespawnOnAlert = despawnOnAlert;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entityZdo is null)
        {
            return;
        }

        if (DespawnOnAlert)
        {
            entityZdo.Set(ZdoFeature, DespawnOnAlert);
        }
    }
}
