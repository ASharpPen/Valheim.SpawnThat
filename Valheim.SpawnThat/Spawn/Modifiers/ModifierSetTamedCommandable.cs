using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierSetTamedCommandable : ISpawnModifier
{
    public const string ZdoFeature = "spawnthat_tamed_commandable";
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool Commandable { get; set; }

    public ModifierSetTamedCommandable()
    { }

    public ModifierSetTamedCommandable(bool commandable)
    {
        Commandable = commandable;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
{
        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

        var tameable = ComponentCache.Get<Tameable>(entity);

        if (tameable is not null && tameable)
        {
#if DEBUG
            Log.LogDebug($"Setting tamed commandable");
#endif
            tameable.m_commandable = Commandable;
            entityZdo?.Set(ZdoFeatureHash, Commandable);
        }
    }
}