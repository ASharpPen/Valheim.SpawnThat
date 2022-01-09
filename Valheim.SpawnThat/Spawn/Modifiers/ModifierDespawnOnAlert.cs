﻿using UnityEngine;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierDespawnOnAlert : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_despawn_on_alert";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool DespawnOnAlert { get; }

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
