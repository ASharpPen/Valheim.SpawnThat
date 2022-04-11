using SpawnThat.Utilities.Extensions;
using UnityEngine;

namespace SpawnThat.Options.Modifiers;

public class ModifierSetHuntPlayer : ISpawnModifier
{
    public bool HuntPlayer { get; set; }

    public ModifierSetHuntPlayer()
    { }

    public ModifierSetHuntPlayer(bool huntPlayer)
    {
        HuntPlayer = huntPlayer;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        entityZdo.SetHuntPlayer(HuntPlayer);
    }
}
