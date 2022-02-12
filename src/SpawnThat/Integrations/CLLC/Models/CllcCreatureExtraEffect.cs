using CreatureLevelControl;

namespace SpawnThat.Integrations.CLLC.Models;

public enum CllcCreatureExtraEffect
{
    None,
    Aggressive,
    Quick,
    Regenerating,
    Curious,
    Splitting,
    Armored
}

internal static class CllcCreatureExtraEffectExtensions
{
    public static CreatureExtraEffect Convert(this CllcCreatureExtraEffect extraEffect)
    {
        return extraEffect switch
        {
            CllcCreatureExtraEffect.Aggressive => CreatureExtraEffect.Aggressive,
            CllcCreatureExtraEffect.Quick => CreatureExtraEffect.Quick,
            CllcCreatureExtraEffect.Regenerating => CreatureExtraEffect.Regenerating,
            CllcCreatureExtraEffect.Curious => CreatureExtraEffect.Curious,
            CllcCreatureExtraEffect.Splitting => CreatureExtraEffect.Splitting,
            CllcCreatureExtraEffect.Armored => CreatureExtraEffect.Armored,
            _ => CreatureExtraEffect.None,
        };
    }
}
